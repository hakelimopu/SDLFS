open SDL

open System
open Geometry
open Pixel
open SDL

let rec eventPump 
        (lastFrameTime:DateTimeOffset) 
        (eventSource:unit->'TEvent option) 
        (idleHandler:'TState->unit) 
        (eventHandler:'TEvent->'TState->'TState option) 
        (updateHandler:TimeSpan->'TState->'TState) 
        (drawHandler:TimeSpan->'TState->unit) 
        (state:'TState) :unit =
    let currentFrameTime = DateTimeOffset.Now
    let elapsed = currentFrameTime-lastFrameTime

    let state = updateHandler elapsed state

    drawHandler elapsed state

    match eventSource() with
    | Some event ->
        match (event, state) ||> eventHandler with
        | Some state -> state |> eventPump currentFrameTime eventSource idleHandler eventHandler updateHandler drawHandler
        | None -> ()
    | None ->
        state
        |> idleHandler

        state
        |> eventPump currentFrameTime eventSource idleHandler eventHandler updateHandler drawHandler

type ViewFunc<'TModel> = 'TModel -> unit 

type View<'TModel> =
    | Single of ViewFunc<'TModel>
    | Conditional of (('TModel->bool) * View<'TModel>)
    | Composite of View<'TModel> list

type State<'TModel> =
    {Model:'TModel;
    View:View<'TModel>}

let rec renderView (model:'TModel) (view:View<'TModel>) :unit =
    match view with
    | Single view -> 
        view model
    | Conditional (test,view) ->
        if test model then renderView model view
    | Composite views -> 
        views
        |> List.iter (fun view -> renderView model view)

type Sprite = 
    {SrcSurface:Surface.Surface;
    SrcRect:Rectangle;
    Anchor: Point}

let renderSprite (sprite:Sprite) (dstPoint:Point) (dstSurface:Surface.Surface) : unit =
    let dstRect= {sprite.SrcRect with X=dstPoint.X - sprite.Anchor.X;Y=dstPoint.Y - sprite.Anchor.Y}
    dstSurface
    |> Surface.upperBlit (sprite.SrcRect |> Some) sprite.SrcSurface (dstRect |> Some)
    |> ignore

type RenderingContext =
    {Renderer:Render.Renderer;
    Texture:Texture.Texture;
    Surface:Surface.Surface;
    DigitSprites:Map<String,Sprite>;
    ScaleX:int;
    ScaleY:int}

let clearScreen (context:RenderingContext) (model:'TModel) : unit =
    context.Renderer
    |> Render.setDrawColor (255uy, 0uy, 255uy, 255uy)
    |> ignore

    context.Renderer
    |> Render.clear
    |> ignore

    context.Surface
    |> Surface.fillRect None {Red=0uy;Green=0uy;Blue=0uy;Alpha=255uy}
    |> ignore

let presentScreen (context:RenderingContext) (model:'TModel) : unit =
    context.Texture
    |> Texture.update None context.Surface
    |> ignore

    context.Renderer
    |> Render.copy context.Texture None None
    |> ignore

    context.Renderer
    |> Render.present

let onIdle (state:State<'TModel>) : unit =
    ()

type JetLagState =
    | GameOver
    | Play

type JetLagDirection = Left | Right

type JetLagModel =
    {State:JetLagState;
    FrameCounter:int;
    HighScore:int;
    Score:int;
    Direction:JetLagDirection;
    RunLength:int;
    LeftWall:int;
    RightWall:int;
    Blocks:int list;
    Tail:int list}

let renderBlocks (context:RenderingContext) (model:JetLagModel) : unit =
    (0, model.Blocks)
    ||> List.fold 
        (fun row column -> 
            context.Surface
            |> Surface.fillRect ({Rectangle.X= column * context.ScaleX; Y= row * context.ScaleY; Width=context.ScaleX * 1; Height=context.ScaleY * 1} |> Some) {Color.Red = 255uy; Green=255uy; Blue=255uy; Alpha=255uy}
            |> ignore
            row + 1)
    |> ignore

let renderTail (context:RenderingContext) (model:JetLagModel) : unit =
    let headPosition = ((model.Tail |> List.length) - 1) * 1
    (0, model.Tail)
    ||> List.fold 
        (fun row column -> 
            let color = 
                if row=headPosition then
                    {Color.Red = 255uy; Green=0uy; Blue=0uy; Alpha=255uy}
                else
                    {Color.Red = 255uy; Green=255uy; Blue=0uy; Alpha=255uy}
            context.Surface
            |> Surface.fillRect ({Rectangle.X= column * context.ScaleX; Y= row * context.ScaleY; Width=context.ScaleX * 1; Height=context.ScaleY * 1} |> Some) color
            |> ignore
            row + 1)
    |> ignore

let renderWalls (context:RenderingContext) (model:JetLagModel) : unit =
    let blue = {Color.Red = 0uy; Green=0uy; Blue=255uy; Alpha=255uy}
    let rows = (model.Blocks |> List.length) * 1
    context.Surface
    |> Surface.fillRect ({Rectangle.X= model.LeftWall * context.ScaleX; Y= 0 * context.ScaleY; Width=context.ScaleX * 1; Height=context.ScaleY * rows} |> Some) blue
    |> ignore
    context.Surface
    |> Surface.fillRect ({Rectangle.X= model.RightWall * context.ScaleX; Y= 0 * context.ScaleY; Width=context.ScaleX * 1; Height=context.ScaleY * rows} |> Some) blue
    |> ignore

let renderScore (context:RenderingContext) (model:JetLagModel) : unit =
    let text = 
        model.Score
        |> int
        |> sprintf "%d"

    [for c in text -> c.ToString()]
    |> List.fold 
        (fun x c ->
            context.Surface
            |> renderSprite context.DigitSprites.[c] {X=x;Y=0}
            x+4) 16
    |> ignore

let JetLagColumns    = 40
let JetLagRows       = 30
let JetLagTailLength =  6

let resetJetLagModel (model:JetLagModel) : JetLagModel =
    {model with
        FrameCounter=0;
        Blocks=[1..(JetLagRows/1)] |> List.map(fun e-> 0);
        Tail=[1..(JetLagTailLength/1)] |> List.map(fun e-> JetLagColumns / 2 );
        Direction=Right;
        RunLength = 0;
        Score=0}

let onEventGameOver (event: Event.Event) (state:State<JetLagModel>) : State<JetLagModel> option = 
    match event with
    | Event.KeyDown k when k.Keysym.Scancode = Keyboard.ScanCode.Space ->
        Some {state with Model={(state.Model |> resetJetLagModel) with State = Play}}

    | _ -> 
        Some state

let calculateScore (runLength:int) : int =
    ([1 .. (runLength / 1)]
    |> List.reduce (+))
    * 1

let onEventPlay (event: Event.Event) (state:State<JetLagModel>) : State<JetLagModel> option = 
    match event with
    | Event.KeyDown k when k.Keysym.Scancode = Keyboard.ScanCode.Left ->
        Some {state with Model={state.Model with Direction=Left; RunLength=0; Score = state.Model.Score + calculateScore state.Model.RunLength}}

    | Event.KeyDown k when k.Keysym.Scancode = Keyboard.ScanCode.Right ->
        Some {state with Model={state.Model with Direction=Right; RunLength=0; Score = state.Model.Score + calculateScore state.Model.RunLength}}

    | _ -> 
        Some state

let eventMap =
    [(GameOver,onEventGameOver);(Play,onEventPlay);]
    |> Map.ofList

let onEvent (event: Event.Event) (state:State<JetLagModel>) : State<JetLagModel> option =
    if event.isQuitEvent then
        None
    else
        eventMap
        |> Map.tryFind (state.Model.State)
        |> Option.bind(fun handler-> handler event state)

let FramesPerSecond = 15
let MillisecondsPerFrame = 1000 / FramesPerSecond

let addToFrameCounter (delta:int) (model:JetLagModel) : JetLagModel =
    if model.State = GameOver then
        model
    else
        {model with FrameCounter = model.FrameCounter + delta}

let rec scrollLines (random:Random) (model:JetLagModel) : JetLagModel =
    if model.State = Play && model.FrameCounter >= (MillisecondsPerFrame * 1) then
        let frameCounter = model.FrameCounter - MillisecondsPerFrame * 1
        //scroll blocks
        let blocks = 
            [random.Next((model.LeftWall/1)+1, (model.RightWall/1)) * 1]
            |> List.append
                (model.Blocks
                |> List.skip 1)
        //scroll tail
        let head = (model.Tail |> List.last) + (if model.Direction = Left then (-1) else 1)
        let tail = 
            [head]
            |> List.append
                (model.Tail
                |> List.skip 1)
        //check for game over
        let state = if (blocks |> List.item ((tail |> List.length) - 1))=head || head=model.LeftWall || head=model.RightWall  then GameOver else Play
        {model with FrameCounter = frameCounter; Tail = tail; Blocks = blocks; State = state; RunLength = model.RunLength + 1}
        |> scrollLines random
    else
        model

let onUpdateModel (random:Random) (delta:int) (model:JetLagModel) : JetLagModel =
    model
    |> addToFrameCounter (delta)
    |> scrollLines random

let onUpdate (random:Random) (delta:TimeSpan) (state:State<JetLagModel>) : State<JetLagModel>=
    {state with Model=state.Model |> onUpdateModel random (delta.Milliseconds * 1)}

let onDraw (delta:TimeSpan) (state:State<'TModel>) : unit =
    renderView state.Model state.View

let createJetLagModel () :JetLagModel =
    {State=GameOver;
    FrameCounter=0;
    HighScore=0;
    LeftWall=0;
    RightWall=JetLagColumns - 1;
    Score=0;
    RunLength=0;
    Direction=Right;
    Blocks=[];
    Tail=[]}
    |> resetJetLagModel
    
let ScreenWidth  = 640
let ScreenHeight = 480

let runGame () =
    use system = new Init.System(Init.Init.Video ||| Init.Init.Events)

    use window = Window.create "JetLag" Window.Position.Centered (ScreenWidth,ScreenHeight) Window.Flags.None

    use renderer = Render.create window -1 Render.Flags.Accelerated

    use surface = Surface.createRGB (ScreenWidth,ScreenHeight,32) (0x00FF0000u, 0x0000FF00u, 0x000000FFu, 0x00000000u)

    use digitsSurface = Surface.loadBmp Pixel.RGB888Format "Content/digits.bmp"

    Surface.setColorKey (Some {Color.Red=255uy;Green=0uy;Blue=255uy;Alpha=0uy}) digitsSurface
    |> ignore

    use texture = Texture.create Pixel.RGB888Format Texture.Access.Streaming (ScreenWidth,ScreenHeight) renderer

    renderer 
    |> Render.setLogicalSize (ScreenWidth,ScreenHeight) 
    |> ignore

    let digitSprites =
        [0..9]
        |> List.map (fun index->(index |> sprintf "%d",{Sprite.SrcSurface=digitsSurface;SrcRect={X=0 + index * 4;Y=0;Width=4;Height=6};Anchor={X=0;Y=0}}))
        |> Map.ofList

    let context = 
        {Renderer = renderer; 
        Texture = texture; 
        Surface = surface; 
        DigitSprites = digitSprites;
        ScaleX = ScreenWidth / JetLagColumns; 
        ScaleY = ScreenHeight / JetLagRows}

    let state = 
        {Model=createJetLagModel();
        View=Composite 
            [Single (clearScreen context); 
            Single (renderBlocks context); 
            Single (renderWalls context); 
            Single (renderTail context); 
            Single (renderScore context);
            Single (presentScreen context)]}

    let random = Random()

    eventPump DateTimeOffset.Now Event.poll onIdle onEvent (onUpdate random) onDraw state


[<EntryPoint>]
let main argv = 
    runGame()
    0
