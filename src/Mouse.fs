namespace SDL

open System
open System.Runtime.InteropServices
open SDL

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute>]
module Mouse =

    type Button =
        | Left   = 1
        | Middle = 2
        | Right  = 3
        | X1     = 4
        | X2     = 5


    type SystemCursor =
        | Arrow     = 0
        | IBeam     = 1
        | Wait      = 2
        | Crosshair = 3
        | Waitarrow = 4
        | SizeNWSE  = 5
        | SizeNESW  = 6
        | SizeWE    = 7
        | SizeNS    = 8
        | SizeAll   = 9
        | No        = 10
        | Hand      = 11

    type WheelDirection = 
        | Normal  = 0
        | Flipped = 1

    module internal Native = 
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_GetMouseFocus()//TODO
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern uint32 SDL_GetMouseState(int *x, int *y)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern uint32 SDL_GetGlobalMouseState(int *x, int *y)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern uint32 SDL_GetRelativeMouseState(int *x, int *y)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_WarpMouseInWindow(IntPtr window,int x, int y)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_WarpMouseGlobal(int x, int y)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetRelativeMouseMode(int enabled)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_CaptureMouse(int enabled)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetRelativeMouseMode()

    type MouseState =
        {Position:SDL.Geometry.Point;Buttons:Set<Button>}

    let private toButtonSet (buttons:uint32) : Set<Button> =
        (Set.empty, [(Button.Left,0x1u);(Button.Middle,0x2u);(Button.Right,0x4u);(Button.X1,0x8u);(Button.X2,0x10u)])
        ||> Seq.fold (fun buttonSet (button,flag) ->
            if buttons ||| flag = flag then
                buttonSet
                |> Set.add button
            else
                buttonSet)

    let getMouseState () :MouseState =
        let mutable x:int = 0
        let mutable y:int = 0
        let buttons = Native.SDL_GetMouseState(&&x,&&y)
        {Position = {X = x * 1<px>;Y = y * 1<px>};Buttons = buttons |> toButtonSet}

    let getGlobalMouseState () :MouseState =
        let mutable x:int = 0
        let mutable y:int = 0
        let buttons = Native.SDL_GetGlobalMouseState(&&x,&&y)
        {Position = {X = x * 1<px>;Y = y * 1<px>};Buttons = buttons |> toButtonSet}

    let getRelativeMouseState () :MouseState =
        let mutable x:int = 0
        let mutable y:int = 0
        let buttons = Native.SDL_GetRelativeMouseState(&&x,&&y)
        {Position = {X = x * 1<px>;Y = y * 1<px>};Buttons = buttons |> toButtonSet}

    let warpMouseInWindow (window:SDL.Utility.Pointer) (xy:SDL.Geometry.Point) :unit =
        Native.SDL_WarpMouseInWindow(window.Pointer,xy.X / 1<px>,xy.Y / 1<px>)

    let warpMouseInCurrentWindow (xy:SDL.Geometry.Point) :unit =
        Native.SDL_WarpMouseInWindow(IntPtr.Zero,xy.X / 1<px>,xy.Y / 1<px>)

    let warpMouseGlobal (xy:SDL.Geometry.Point) :bool =
        0 = Native.SDL_WarpMouseGlobal(xy.X / 1<px>,xy.Y / 1<px>)

    let setRelativeMouseMode (flag:bool) :bool = 
        0 = Native.SDL_SetRelativeMouseMode(if flag then 1 else 0)

    let getRelativeMouseMove () :bool =
        0 <> Native.SDL_GetRelativeMouseMode()

    let captureMouse (flag:bool) : bool =
        0 = Native.SDL_CaptureMouse(if flag then 1 else 0)