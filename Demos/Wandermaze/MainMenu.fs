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
    context.Renderer
    |> Render.clear
    |> ignore

    context.Renderer
    |> Render.present
    |> ignore

    match Event.poll() with
    | Some (Event.Quit _)    -> None
    | _                      -> eventPump context state

let run (context:RenderingContext): Result option =
    {Selection = None}
    |> eventPump context

