module SplashScreen

open RenderingContext
open SDL

type Result = Proceed

type State = 
    {Start:System.DateTimeOffset;
    Until:System.DateTimeOffset}


let rec eventPump (context:RenderingContext) (state:State) : Result option =
    Render.setDrawColor {Pixel.Color.Red = 0uy; Green=0uy; Blue=0uy; Alpha=255uy} context.Renderer |> ignore
    
    context.Renderer
    |> Render.clear
    |> ignore

    let patterns = [Patterns.patterns.["P"];Patterns.patterns.["D"];Patterns.patterns.["G"]]

    let nextOffset (start:Geometry.Point) : Geometry.Point =
        {start with X = start.X + 8}

    Patterns.renderPatterns context.Renderer {Geometry.Point.X = 1; Y = 1} (Some {Pixel.Color.Red = 0uy; Green=0uy; Blue=0uy; Alpha=255uy}) (Some {Pixel.Color.Red = 0uy; Green=255uy; Blue=0uy; Alpha=255uy}) nextOffset {Geometry.Point.X = 158; Y = 124} patterns

    context.Renderer
    |> Render.present
    |> ignore

    let current = System.DateTimeOffset.Now
    if current >= state.Until then //if time is up
        Some Proceed
    else
        match Event.poll() with
        | Some (Event.KeyDown _) -> Some Proceed
        | Some (Event.Quit _)    -> None
        | _                      -> eventPump context state

let run (context:RenderingContext): Result option =
    {Start = System.DateTimeOffset.Now; Until = System.DateTimeOffset.Now.AddSeconds(5.0)}
    |> eventPump context
