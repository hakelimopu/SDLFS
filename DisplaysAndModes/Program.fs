open SDL

[<EntryPoint>]
let main argv = 
    use system = new Init.System(Init.Init.Video)

    let displayCount = Video.getDisplayCount()

    printf "Number of displays: %d\r\n\r\n" displayCount

    [0..(displayCount-1)]
    |> List.iter 
        (fun index->
            let displayName = Video.getDisplayName index
            let displayBounds = Video.getDisplayBounds index

            printf 
                "For display index %d:\r\n\r\n\tName:%s\r\n\tBounds: (%d,%d,%d,%d)\r\n\r\n"
                index
                displayName
                displayBounds.X
                displayBounds.Y
                displayBounds.Width
                displayBounds.Height)

    0
