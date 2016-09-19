
open SDL

let rec eventPump (eventPoller:unit->'TEvent option) (modelFetcher:'TState->'TModel) (idleHandler:'TState->('TModel->unit) list) (eventHandler:'TState -> ('TEvent->'TModel->'TModel option)) (state:'TState) =
    match(eventPoller()) with
    | Some event ->

        match state |> (state |> eventHandler) event with
        | Some newState -> 
            newState 
            |> eventPump eventPoller modelFetcher idleHandler eventHandler
        | None -> 
            ()

    | None ->
        let model = state |> modelFetcher

        state 
        |> idleHandler
        |> List.iter (fun f -> f model)
        
        eventPump eventPoller modelFetcher idleHandler eventHandler state

type Location = {X:int; Y:int}

let createLocation (x:int, y:int) : Location =
    {X=x;Y=y}

type Player = {Location:Location}

let createPlayer (location:Location) : Player =
    {Location = location}

type World = {Player:Player}

let createWorld (player:Player) : World =
    {Player=player}

type GameView = string * (World->unit)

type EventHandler = Event.Event -> World -> World option

type GameState = {Model:World; Views: GameView list; EventHandlers: EventHandler list}

let idleFunc (renderer:SDL.Render.Renderer) (state:World) : unit = 
    renderer
    |> Render.clear 
    |> ignore

    renderer
    |> Render.present

let createFunc(renderer:Render.Renderer):GameState =
    let world = 
        (0,0)
        |> createLocation
        |> createPlayer
        |> createWorld
    {Model=world;Views=[("",idleFunc renderer)];EventHandlers=[]}


let eventHandler (event:Event.Event) (state:GameState) : GameState option =
    if event.isQuitEvent then
        None
    else
        state |> Some

type InitializationOptions = 
    {
        SystemInitializationFlags: Init.Init;
        WindowTitle: string;
        WindowPosition:int<px> * int<px>;
        WindowSize:int<px> * int<px>;
        WindowFlags:Window.Flags;
        RenderSize:int<px> * int<px>
    }

let runGame (initOpts:InitializationOptions) : unit =
    use system = new Init.System(initOpts.SystemInitializationFlags)

    use mainWindow = Window.create initOpts.WindowTitle initOpts.WindowPosition initOpts.WindowSize initOpts.WindowFlags

    use mainRenderer = Render.create mainWindow -1 Render.Flags.Accelerated

    use surface = Surface.createRGB (initOpts.RenderSize |> fst,initOpts.RenderSize |> snd, 32<bit/px>) (0x00FF0000u,0x0000FF00u,0x000000FFu,0x00000000u)

    use mainTexture = mainRenderer |> SDL.Texture.create SDL.Pixel.RGB888Format SDL.Texture.Access.Streaming initOpts.RenderSize

    mainRenderer 
    |> SDL.Render.setLogicalSize initOpts.RenderSize 
    |> ignore

    eventPump
        Event.poll
        (fun s -> s.Model)
        (fun s -> s.Views |> List.map snd)
        (fun s -> s.EventHandlers |> List.head)
        (createFunc())
    

[<EntryPoint>]
let main argv = 
    {SystemInitializationFlags = Init.Init.Video ||| Init.Init.Events;
    WindowTitle="Snake!";
    WindowPosition = (100<px>,100<px>);
    WindowSize = (640<px>,480<px>);
    WindowFlags = Window.Flags.None;
    RenderSize = (320<px>,240<px>)}
    |> runGame

    0
