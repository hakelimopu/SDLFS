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
            |> SDL.Event.wait

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
            |> SDL.Event.wait

        if event.IsSome then
            if event.Value.isKeyDownEvent then
                if event.Value.toKeyboardEvent.Value.Keysym.Scancode = Keyboard.ScanCode.Escape then
                    ()
                else
                    eventPump()
            else
                eventPump()
        else 
            ()

    eventPump()

let createDesktopFullscreenWindowTest () = 
    use system = new SDL.Init.System(SDL.Init.Init.Everything)
    use window = SDL.Window.create "SDL2FS" (100<SDL.px>, 200<SDL.px>) (640<SDL.px>,480<SDL.px>) SDL.Window.Flags.FullScreenDesktop

    Console.WriteLine()
    Console.WriteLine("Press escape to stop test.")

    let rec eventPump () : unit =
        let event = 
            None
            |> SDL.Event.wait

        if event.IsSome then
            if event.Value.isKeyDownEvent then
                if event.Value.toKeyboardEvent.Value.Keysym.Scancode = Keyboard.ScanCode.Escape then
                    ()
                else
                    eventPump()
            else
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
        | ConsoleKey.F -> createFullscreenWindowTest()
        | ConsoleKey.D -> createDesktopFullscreenWindowTest()
        | _ -> ()
        windowMenu()



    