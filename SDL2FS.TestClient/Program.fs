open SDL

open System

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

[<Measure>]type cell

type RenderingContext =
    {Renderer:Render.Renderer;
    Texture:Texture.Texture;
    Surface:Surface.Surface;
    ScaleX:int<px/cell>;
    ScaleY:int<px/cell>}

let clearScreen (context:RenderingContext) (model:'TModel) : unit =
    context.Renderer
    |> Render.setDrawColor (255uy, 0uy, 255uy, 255uy)
    |> ignore

    context.Renderer
    |> Render.clear
    |> ignore

    context.Surface
    |> Surface.fillRect None {Red=255uy;Green=0uy;Blue=255uy;Alpha=255uy}
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

[<Measure>]type point

type JetLagModel =
    {State:JetLagState;
    FrameCounter:int<ms>;
    HighScore:int<point>;
    Score:int<point>;
    Direction:JetLagDirection;
    LeftWall:int<cell>;
    RightWall:int<cell>;
    Blocks:int<cell> seq;
    Tail:int<cell> seq}

let onEvent (event: Event.Event) (state:'TState) : 'TState option =
    if event.isQuitEvent then
        None
    else
        Some state

[<Measure>]type frame

let FramesPerSecond = 10<frame/second>
let MillisecondsPerFrame = 1000<ms/second> / FramesPerSecond

let addToFrameCounter (delta:int<ms>) (model:JetLagModel) : JetLagModel =
    if model.State = GameOver then
        model
    else
        {model with FrameCounter = model.FrameCounter + delta}

let rec scrollLines (model:JetLagModel) : JetLagModel =
    if model.State = Play && model.FrameCounter >= (MillisecondsPerFrame * 1<frame>) then
        let frameCounter = model.FrameCounter - MillisecondsPerFrame * 1<frame>
        //scroll blocks
        //scroll tail
        //
        {model with FrameCounter = frameCounter}
        |> scrollLines
    else
        model

let onUpdateModel (delta:int<ms>) (model:JetLagModel) : JetLagModel =
    model
    |> addToFrameCounter (delta)
    |> scrollLines

let onUpdate (delta:TimeSpan) (state:State<JetLagModel>) : State<JetLagModel>=
    {state with Model=state.Model |> onUpdateModel (delta.Milliseconds * 1<ms>)}

let onDraw (delta:TimeSpan) (state:State<'TModel>) : unit =
    renderView state.Model state.View

let JetLagColumns    = 40<cell>
let JetLagRows       = 30<cell>
let JetLagTailLength =  6<cell>

let resetJetLagModel (model:JetLagModel) : JetLagModel =
    {model with
        FrameCounter=0<ms>;
        Blocks=[1..(JetLagColumns/1<cell>)] |> Seq.map(fun e-> 0<cell>);
        Tail=[1..(JetLagTailLength/1<cell>)] |> Seq.map(fun e-> JetLagColumns / 2 );
        Direction=Right;
        Score=0<point>}

let createJetLagModel () :JetLagModel =
    {State=GameOver;
    FrameCounter=0<ms>;
    HighScore=0<point>;
    LeftWall=0<cell>;
    RightWall=JetLagColumns - 1<cell>;
    Score=0<point>;
    Direction=Right;
    Blocks=[];
    Tail=[]}
    |> resetJetLagModel
    
let ScreenWidth  = 640<px>
let ScreenHeight = 480<px>

let runGame () =
    use system = new Init.System(Init.Init.Video ||| Init.Init.Events)

    use window = Window.create "Test!" Window.Position.Centered (ScreenWidth,ScreenHeight) Window.Flags.None

    use renderer = Render.create window -1 Render.Flags.Accelerated

    use surface = Surface.createRGB (ScreenWidth,ScreenHeight,32<bit/px>) (0x00FF0000u, 0x0000FF00u, 0x000000FFu, 0x00000000u)

    use texture = Texture.create Pixel.RGB888Format Texture.Access.Streaming (ScreenWidth,ScreenHeight) renderer

    renderer |> Render.setLogicalSize (ScreenWidth,ScreenHeight) |> ignore

    let context = {Renderer = renderer; Texture = texture; Surface = surface; ScaleX = ScreenWidth / JetLagColumns; ScaleY = ScreenHeight / JetLagRows}

    let state = {Model=createJetLagModel();View=Composite [Single (clearScreen context); Single (presentScreen context)]}

    eventPump DateTimeOffset.Now Event.poll onIdle onEvent onUpdate onDraw state


[<EntryPoint>]
let main argv = 
    runGame()
    0
