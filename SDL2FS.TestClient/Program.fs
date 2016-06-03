open SDL

let createWindowTest () = 
    use system = new SDL.Init.System(SDL.Init.Init.Everything)
    use window = SDL.Window.create "Test" (100<SDL.px>, 50<SDL.px>) (150<SDL.px>,200<SDL.px>) SDL.Window.Flags.None

    let rec eventPump () : unit =
        let event = 
            None
            |> SDL.Event.waitEvent
        if event.IsSome && not event.Value.isQuitEvent then 
            eventPump()
        else 
            ()

    eventPump()

[<EntryPoint>]
let main argv = 
    createWindowTest()

    0
