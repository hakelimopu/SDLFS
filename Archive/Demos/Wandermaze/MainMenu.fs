module MainMenu

open RenderingContext
open SDL

type Result =  
    | New 
    | Load 
    | Exit
    member this.next () =
        match this with
        | New -> Load
        | _ -> Exit
    member this.previous () =
        match this with
        | Exit -> Load
        | _ -> New

type State = 
    {Selection: Result option}


let rec eventPump (context:RenderingContext) (state:State) : Result option =
    Render.setDrawColor {Pixel.Color.Red = 0uy; Green=0uy; Blue=0uy; Alpha=255uy} context.Renderer |> ignore

    context.Renderer
    |> Render.clear
    |> ignore

    let nextOffset (start:Geometry.Point) : Geometry.Point =
        {start with X = start.X + 8}

    let black = 
        {Pixel.Color.Red = 0uy; Pixel.Color.Green=0uy; Pixel.Color.Blue=0uy; Pixel.Color.Alpha=255uy}
    let gray = 
        {Pixel.Color.Red = 192uy; Pixel.Color.Green=192uy; Pixel.Color.Blue=192uy; Pixel.Color.Alpha=255uy}
    let white = 
        {Pixel.Color.Red = 255uy; Pixel.Color.Green=255uy; Pixel.Color.Blue=255uy; Pixel.Color.Alpha=255uy}

    let newColor =
        match state.Selection with
        | Some New -> Some white
        | _ -> Some gray
    let loadColor =
        match state.Selection with
        | Some Load -> Some white
        | _ -> Some gray
    let exitColor =
        match state.Selection with
        | Some Exit -> Some white
        | _ -> Some gray

    Patterns.renderPatterns context.Renderer  {Geometry.Point.X = 1; Y = 1} (Some black) newColor nextOffset {Geometry.Point.X = 0; Y = 0} (Patterns.fromText "New" Patterns.patterns)
    Patterns.renderPatterns context.Renderer  {Geometry.Point.X = 1; Y = 1} (Some black) loadColor nextOffset {Geometry.Point.X = 0; Y = 8} (Patterns.fromText "Load" Patterns.patterns)
    Patterns.renderPatterns context.Renderer  {Geometry.Point.X = 1; Y = 1} (Some black) exitColor nextOffset {Geometry.Point.X = 0; Y = 16} (Patterns.fromText "Exit" Patterns.patterns)

    context.Renderer
    |> Render.present
    |> ignore

    let nextSelection (state:State) : State =
        {state with Selection = if state.Selection.IsSome then Some (state.Selection.Value.next()) else Some New}

    let previousSelection (state:State) : State =
        {state with Selection = if state.Selection.IsSome then Some (state.Selection.Value.previous()) else Some Exit}

    match Event.poll() with
    | Some (Event.Quit _)    -> None
    | Some (Event.KeyDown x) when x.Keysym.Scancode = Keyboard.ScanCode.Up -> eventPump context (state|>previousSelection)
    | Some (Event.KeyDown x) when x.Keysym.Scancode = Keyboard.ScanCode.Down -> eventPump context (state|>nextSelection)
    | _                      -> eventPump context state

let run (context:RenderingContext): Result option =
    {Selection = None}
    |> eventPump context

