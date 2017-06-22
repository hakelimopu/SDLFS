open SDL

let printDisplayMode (prefix:string) (mode:Video.DisplayMode): unit =
    printf "\r\n"
    printf "%s Width: %d\r\n" prefix mode.Width
    printf "%s Height: %d\r\n" prefix mode.Height
    printf "%s Refresh Rate: %d\r\n" prefix mode.RefreshRate
    printf "%s Format: %s\r\n" prefix (mode.Format |> Pixel.getFormatName)
    printf "%s Data: %d\r\n" prefix mode.Data
    

let printDisplayDPI (dpi:Video.DisplayDPI) : unit =
    printf "Display Diagonal DPI: %f\r\n" dpi.Diagonal
    printf "Display Horizontal DPI: %f\r\n" dpi.Horizontal
    printf "Display Vertial DPI: %f\r\n" dpi.Vertical


let printDisplayProperties (properties:Video.DisplayProperties) : unit =
    printf "\r\n"
    printf "Display Index: %d\r\n" properties.Index
    printf "Display Name: %s\r\n" properties.Name
    printf "Display X: %d\r\n" properties.Bounds.X
    printf "Display Y: %d\r\n" properties.Bounds.Y
    printf "Display Width: %d\r\n" properties.Bounds.Width
    printf "Display Height: %d\r\n" properties.Bounds.Height

    properties.DPI
    |> Option.iter (printDisplayDPI)

    properties.DesktopMode
    |> printDisplayMode "Desktop Mode"

    properties.CurrentMode
    |> printDisplayMode "Current Mode"

    properties.AvailableModes
    |> List.iter (printDisplayMode "Available Mode")


[<EntryPoint>]
let main argv = 
    use system = new Init.System(Init.Init.Video)

    let displays = Video.getDisplays()
    printf "Number of displays: %d\r\n" displays.Length
    displays
    |> List.iter printDisplayProperties

    let drivers = Video.getDrivers()
    printf "\r\nNumber of drivers: %d\r\n" drivers.Length
    drivers
    |> List.iter (printf "\t%s\r\n")

    0
