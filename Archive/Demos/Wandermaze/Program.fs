open SDL
open Geometry
open RenderingContext

let ScreenRectangle = {SDL.Geometry.Rectangle.X = 0; Y = 0; Width = 1020; Height=768}
let BoardViewRectangle  = {SDL.Geometry.Rectangle.X = 0; Y = 0; Width = 768; Height=768}

let BoardCellRectangle = {SDL.Geometry.Rectangle.X = 0; Y = 0; Width = BoardViewRectangle.Width / BoardColumns; Height = BoardViewRectangle.Height / BoardRows}


let runGame () =
    use system = new Init.System(Init.Init.Video ||| Init.Init.Events)

    use window = Window.create ("Wandermaze", Window.Position.Centered, ScreenRectangle.Width,ScreenRectangle.Height, Window.Flags.None)

    use renderer = Render.create window None Render.Flags.Accelerated

    Render.setLogicalSize (340,256) renderer |> ignore

    let context = 
        {Renderer=renderer}

    context
    |> SplashScreen.run 
    |> Option.map (fun _ -> MainMenu.run context)
    |> ignore

[<EntryPoint>]
let main argv = 
    runGame()
    0
