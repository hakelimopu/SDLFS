module WindowTests

open SDL
open System

let createWindowTest () = 
    use system = new SDL.Init.System(SDL.Init.Init.Everything)
    use window = SDL.Window.create "SDL2FS" (100<SDL.px>, 200<SDL.px>) (640<SDL.px>,480<SDL.px>) SDL.Window.Flags.None

    Console.WriteLine()
    Console.WriteLine("Close window to stop test.")

    let rec eventPump () : unit =
        let event = 
            None
            |> SDL.Event.waitEvent

        if event.IsSome && not event.Value.isQuitEvent then 
            eventPump()
        else 
            ()

    eventPump()

let createFullscreenWindowTest () = 
    use system = new SDL.Init.System(SDL.Init.Init.Everything)
    use window = SDL.Window.create "SDL2FS" (100<SDL.px>, 200<SDL.px>) (640<SDL.px>,480<SDL.px>) SDL.Window.Flags.FullScreen

    Console.WriteLine()
    Console.WriteLine("Press escape to stop test.")

    let rec eventPump () : unit =
        let event = 
            None
            |> SDL.Event.waitEvent

        if event.IsSome && not event.Value.isQuitEvent then 
            eventPump()
        else 
            ()

    eventPump()



let rec windowMenu () =
    Console.WriteLine()
    Console.WriteLine("Window Test Menu:")
    Console.WriteLine("[C]reate Window")
    Console.WriteLine("[F]ullscreen Window")
    Console.WriteLine("[D]esktop Fullscreen Window")
    Console.WriteLine("[Esc] - Go Back")
    let keyInfo = Console.ReadKey(true)
    if keyInfo.Key = ConsoleKey.Escape then
        ()
    else
        match keyInfo.Key with 
        | ConsoleKey.C -> createWindowTest()



    