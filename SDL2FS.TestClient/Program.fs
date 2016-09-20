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
    |> Render.clear
    |> ignore

let presentScreen (context:RenderingContext) (model:'TModel) : unit =
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

let onUpdate (delta:TimeSpan) (state:State<JetLagModel>) : State<JetLagModel>=
    state

let onDraw (delta:TimeSpan) (state:State<'TModel>) : unit =
    renderView state.Model state.View

let JetLagColumns    = 40<cell>
let JetLagRows       = 30<cell>
let JetLagTailLength =  6<cell>

let resetJetLagModel (model:JetLagModel) : JetLagModel =
    {model with
        Blocks=[1..(JetLagColumns/1<cell>)] |> Seq.map(fun e-> 0<cell>);
        Tail=[1..(JetLagTailLength/1<cell>)] |> Seq.map(fun e-> JetLagColumns / 2 );
        Direction=Right;
        Score=0<point>}

let createJetLagModel () :JetLagModel =
    {State=GameOver;
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
