﻿//absolute minimum SDL2FS application
//creates a window
//waits for a quit event, which closes the window
//draws a magenta screen
open SDL
open Pixel

let rec eventPump 
        (eventSource:   unit   -> 'TEvent option) 
        (eventHandler: 'TEvent -> 'TState->'TState option) 
        (drawHandler:  'TState -> unit) 
        (state:        'TState) :unit =

    //draw the state
    drawHandler state

    //check for an event
    match eventSource() with

    //an event occurred
    | Some event ->

        //send event to event handler, and then pump again if a state is returned
        (event, state) 
        ||> eventHandler
        |> Option.iter (eventPump eventSource eventHandler drawHandler)

    //no event occurred
    | None ->
        state
        |>eventPump eventSource eventHandler drawHandler

let onEvent (event:Event.Event) (state) =
    if event.isQuitEvent then
        None
    else    
        Some state

let onDraw (surface:Surface.Surface) (texture:Texture.Texture) (renderer:Render.Renderer) (state:Color) : unit =
    surface
    |> Surface.fillRect None state
    |> ignore

    texture
    |> Texture.update None surface
    |> ignore

    renderer
    |> Render.copy texture None None
    |> ignore

    renderer
    |> Render.present


type State = Color

let ScreenWidth = 640
let ScreenHeight = 480

let runGame () =
    use system = new Init.System(Init.Init.Video ||| Init.Init.Events)

    use window = Window.create ("Boilerplate", Window.Position.Centered, ScreenWidth,ScreenHeight, Window.Flags.None)

    use renderer = Render.create window None Render.Flags.Accelerated

    use surface = Surface.createRGB (ScreenWidth,ScreenHeight,32) (0x00FF0000u, 0x0000FF00u, 0x000000FFu, 0x00000000u)

    use texture = Texture.create Pixel.RGB888Format Texture.Access.Streaming (ScreenWidth,ScreenHeight) renderer

    renderer 
    |> Render.setLogicalSize (ScreenWidth,ScreenHeight) 
    |> ignore

    let state = {Color.Red=255uy;Green=0uy;Blue=255uy;Alpha=255uy}

    eventPump Event.poll onEvent (onDraw surface texture renderer) state

[<EntryPoint>]
let main argv = 
    runGame()
    0
