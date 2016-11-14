open SDL

[<EntryPoint>]
let main argv = 
    use system = new Init.System(Init.Init.Video)

    let displays = Video.getDisplays()

    printf "Number of displays: %d\r\n\r\n" displays.Length

    displays
    |> List.iter
        (fun display->
            printf 
                "For display index %d:\r\n\r\n\tName:%s\r\n\tBounds: (%d,%d,%d,%d)\r\n\r\n"
                display.Index
                display.Name
                display.Bounds.X
                display.Bounds.Y
                display.Bounds.Width
                display.Bounds.Height)

    0
