open SDL
open Pixel
open Geometry

type EventSource<'TEvent> = unit -> 'TEvent option
type EventHandler<'TEvent,'TState> = 'TEvent -> 'TState -> 'TState option
type PresentationHandler<'TState> = 'TState -> unit

let rec eventOnlyEventPump 
        (eventSource: EventSource<'TEvent>) 
        (eventHandler: EventHandler<'TEvent,'TState>) 
        (presentationHandler: PresentationHandler<'TState>) 
        (state: 'TState) :unit =
    presentationHandler state
    match eventSource() with
    | Some event ->
        (event, state) 
        ||> eventHandler
        |> Option.iter (eventOnlyEventPump eventSource eventHandler presentationHandler)
    | None ->
        state
        |>eventOnlyEventPump eventSource eventHandler presentationHandler

let onEvent (event:Event.Event) (state) =
    if event.isQuitEvent then
        None
    else    
        Some state

let ScreenWidth = 640
let ScreenHeight = 480

type BoardCell = Gold | Block | Fire | Water | Empty

type BoardColumn = 
    {Cells:Map<int, BoardCell>}
    member this.getGoldLeft () : int =
        this.Cells
        |> Map.fold (fun acc _ v -> if v = Gold then acc + 1 else acc ) 0

type Board = 
    {Columns:Map<int, BoardColumn>}
    member this.getGoldLeft() : int =
        this.Columns
        |> Map.fold (fun acc _ v -> v.getGoldLeft()) 0

type PlayerState = 
    {Column:int;
    Row:int;
    Alive:bool;
    Score:int;
    Water:int;
    Level:int;
    Moves:int}
    member this.setColumn (column:int) : PlayerState =
        {this with Column=column}
    member this.setRow (row:int) : PlayerState =
        {this with Row = row}
    member this.setAlive (alive:bool) : PlayerState =
        {this with Alive=alive}
    member this.clearScore () : PlayerState =
        {this with Score = 0}
    member this.addScore (score:int) : PlayerState =
        {this with Score = this.Score + score}
    member this.setWater (water:int) : PlayerState =
        {this with Water = water}
    member this.incrementWater () : PlayerState =
        {this with Water = this.Water + 1}
    member this.decrementWater () : PlayerState =
        {this with Water = this.Water - 1}
    member this.clearLevel () : PlayerState =
        {this with Level=0}
    member this.incrementLevel() : PlayerState =
        {this with Level = this.Level + 1}
    member this.clearMoves () : PlayerState =
        {this with Moves = 0}
    member this.incrementMoves():PlayerState =
        {this with Moves = this.Moves + 1}

type LevelState =
    {Gold:int;Blocks:int;Fire:int;Water:int}

let onDraw (renderer:Render.Renderer) (state:System.Random) : unit =
    renderer
    |>* Render.clear
    
    renderer
    |> Render.present

let runGame () =
    use system = new Init.System(Init.Init.Video ||| Init.Init.Events)

    use window = Window.create ("Wandermaze", Window.Position.Centered, ScreenWidth,ScreenHeight, Window.Flags.None)

    use renderer = Render.create window None Render.Flags.Accelerated

    let state = new System.Random()

    eventOnlyEventPump Event.poll onEvent (onDraw renderer) state

[<EntryPoint>]
let main argv = 
    runGame()
    0
