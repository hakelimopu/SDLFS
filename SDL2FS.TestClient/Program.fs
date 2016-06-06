open SDL

open System

let rec mainMenu () =
    Console.WriteLine()
    Console.WriteLine("Main Menu:")
    Console.WriteLine("[W]indow tests")
    Console.WriteLine("[Q]uit")
    let keyInfo = Console.ReadKey(true)
    if keyInfo.Key = ConsoleKey.Q then
        ()
    else
        match keyInfo.Key with 
        | ConsoleKey.W -> WindowTests.windowMenu()
        | _ -> ()
        mainMenu()


[<EntryPoint>]
let main argv = 
    mainMenu()

    0
