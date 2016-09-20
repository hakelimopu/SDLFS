namespace SDL

#nowarn "9"

open System.Runtime.InteropServices
open System
open SDL
open Microsoft.FSharp.NativeInterop

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute>]
module Window = 

    type GLattr =
        | RedSize                 = 0
        | GreenSize               = 1
        | BlueSize                = 2
        | AlphaSize               = 3
        | BufferSize              = 4
        | DoubleBuffer            = 5
        | DepthSize               = 6
        | StencilSize             = 7
        | AccumRedSize            = 8
        | AccumGreenSize          = 9
        | AccumBlueSize           = 10
        | AccumAlphaSize          = 11
        | Stereo                  = 12
        | MultiSampleBuffers      = 13
        | MultiSampleSamples      = 14
        | AcceleratedVisual       = 15
        | RetainedBacking         = 16
        | ContextMajorVersion     = 17
        | ContextMinorVersion     = 18
        | ContextEgl              = 19
        | ContextFlags            = 20
        | ContextProfileMask      = 21
        | ShareWithCurrentContext = 22
        | FrameBufferSRGBCapable  = 23
        | ContextReleaseBehavior  = 24

    [<Flags>]
    type GLProfile = 
        | Core           = 0x0001
        | Compatibility  = 0x0002
        | ES             = 0x0004

    [<Flags>]
    type GLFlag = 
        | Debug             = 0x0001
        | ForwardCompatible = 0x0002
        | RobustAccess      = 0x0004
        | ResetIsolation    = 0x0008

    [<Flags>]
    type GLReleaseFlag = 
        | None   = 0x0000
        | Flush  = 0x0001

    [<RequireQualifiedAccess>]
    type Flags = 
        | None               = 0x00000000
        | FullScreen         = 0x00000001
        | OpenGL             = 0x00000002
        | Shown              = 0x00000004
        | Hidden             = 0x00000008
        | Borderless         = 0x00000010
        | Resizable          = 0x00000020
        | Minimized          = 0x00000040
        | Maximized          = 0x00000080
        | InputGrabbed       = 0x00000100
        | InputFocus         = 0x00000200
        | MouseFocus         = 0x00000400
        | FullScreenDesktop  = 0x00001001
        | Foreign            = 0x00000800
        | AllowHighDPI       = 0x00002000
        | MouseCapture       = 0x00004000

    type WindowEvent =
        | None        = 0
        | Shown       = 1
        | Hidden      = 2
        | Exposed     = 3
        | Moved       = 4
        | Resized     = 5
        | SizeChanged = 6
        | Minimized   = 7
        | Maximized   = 8
        | Restored    = 9
        | Enter       = 10
        | Leave       = 11
        | FocusGained = 12
        | FocusLost   = 13
        | Close       = 14

    type Window = SDL.Utility.Pointer

    module private Native =
        //create and destroy
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_CreateWindow(IntPtr title, int x, int y, int w, int h, uint32 flags)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_CreateWindowFrom(IntPtr data)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_DestroyWindow(IntPtr window)

        //display modes
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetWindowDisplayIndex(IntPtr window)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetWindowDisplayMode(IntPtr window, IntPtr mode)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetWindowDisplayMode(IntPtr window,IntPtr mode)

        //window properties
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern uint32 SDL_GetWindowPixelFormat(IntPtr window)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern uint32 SDL_GetWindowID(IntPtr window)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern uint32 SDL_GetWindowFlags(IntPtr window)

        //title
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_SetWindowTitle(IntPtr window, IntPtr title)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_GetWindowTitle(IntPtr window)

        //icon
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_SetWindowIcon(IntPtr window,IntPtr icon)

        //data
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_SetWindowData(IntPtr window, IntPtr name,IntPtr userdata)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_GetWindowData(IntPtr window, IntPtr name)

        //position
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_SetWindowPosition(IntPtr window,int x, int y)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_GetWindowPosition(IntPtr window,int* x, int* y)

        //size
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_SetWindowSize(IntPtr window, int w,int h)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_GetWindowSize(IntPtr window, int* w,int* h)

        //minimum size
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_SetWindowMinimumSize(IntPtr window,int min_w, int min_h)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_GetWindowMinimumSize(IntPtr window,int* w, int* h)

        //maximum size
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_SetWindowMaximumSize(IntPtr window,int max_w, int max_h)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_GetWindowMaximumSize(IntPtr window,int* w, int* h)

        //bordered
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_SetWindowBordered(IntPtr window,int bordered)

        //show/hide
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_ShowWindow(IntPtr window)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_HideWindow(IntPtr window)

        //window state
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_RaiseWindow(IntPtr window)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_MaximizeWindow(IntPtr window)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_MinimizeWindow(IntPtr window)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_RestoreWindow(IntPtr window)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetWindowFullscreen(IntPtr window,uint32 flags)

        //window surface
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_GetWindowSurface(IntPtr window)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_UpdateWindowSurface(IntPtr window)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_UpdateWindowSurfaceRects(IntPtr window, IntPtr rects,int numrects)

        //grab
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_SetWindowGrab(IntPtr window,int grabbed)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetWindowGrab(IntPtr window)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_GetGrabbedWindow()

        //brightness
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetWindowBrightness(IntPtr window, float brightness)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern float SDL_GetWindowBrightness(IntPtr window)

        //gamma ramp
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetWindowGammaRamp(IntPtr window, IntPtr red, IntPtr green, IntPtr blue)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetWindowGammaRamp(IntPtr window,IntPtr red,IntPtr green,IntPtr blue)

        //hit test
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetWindowHitTest(IntPtr window,IntPtr callback,IntPtr callback_data)

        //screen saver
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_IsScreenSaverEnabled()
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_EnableScreenSaver()
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_DisableScreenSaver()

        //window from id
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_GetWindowFromID(uint32 id)

    [<RequireQualifiedAccess>]
    type Position =
        | Undefined
        | Centered
        | Absolute of int<px> * int<px>

    let create (title:string) (position:Position) (w:int<px>,h:int<px>) (flags:Flags) :Window =
        let windowX, windowY = 
            match position with
            | Position.Undefined -> (0x1FFF0000,0x1FFF0000)
            | Position.Centered -> (0x2FFF0000,0x2FFF0000)
            | Position.Absolute (x,y) -> (x/1<px>, y/1<px>)
        let ptr = 
            title
            |> SDL.Utility.withUtf8String (fun ptr -> Native.SDL_CreateWindow(ptr, windowX, windowY, w /1<px>, h /1<px>, flags |> uint32))
        new SDL.Utility.Pointer(ptr, Native.SDL_DestroyWindow)

    let createFrom (data:IntPtr) :Window =
        new SDL.Utility.Pointer(Native.SDL_CreateWindowFrom(data),Native.SDL_DestroyWindow)

    let hide (window:Window) =
        Native.SDL_HideWindow window.Pointer

    let show (window:Window) =
        Native.SDL_ShowWindow window.Pointer

    let minimize (window:Window) =
        Native.SDL_MinimizeWindow window.Pointer

    let maximize (window:Window) =
        Native.SDL_MaximizeWindow window.Pointer

    let raise (window:Window) =
        Native.SDL_RaiseWindow window.Pointer

    let restore (window:Window) =
        Native.SDL_RestoreWindow window.Pointer

    let setTitle (text:string) (window:Window) =
        text
        |> SDL.Utility.withUtf8String (fun ptr -> Native.SDL_SetWindowTitle(window.Pointer, ptr))

    let getTitle (window:Window) :string =
        Native.SDL_GetWindowTitle window.Pointer
        |> SDL.Utility.intPtrToStringUtf8

    let setSize (w:int<px>,h:int<px>) (window:Window) =
        Native.SDL_SetWindowSize(window.Pointer, w/1<px>, h/1<px>)

    let getSize (window:Window) :int<px> * int<px> =
        let mutable x = 0
        let mutable y = 0
        Native.SDL_GetWindowSize (window.Pointer, &&x, &&y)
        (x * 1<px>, y * 1<px>)

    let setPosition (x:int<px>,y:int<px>) (window:Window) =
        Native.SDL_SetWindowPosition(window.Pointer, x/1<px>, y/1<px>)

    let getPosition (window:Window) :int<px> * int<px> =
        let mutable x = 0
        let mutable y = 0
        Native.SDL_GetWindowPosition (window.Pointer, &&x, &&y)
        (x * 1<px>, y * 1<px>)

    let setBrightness (brightness:float) (window:Window) =
        Native.SDL_SetWindowBrightness(window.Pointer, brightness)

    let getBrightness (window:Window) :float =
        Native.SDL_GetWindowBrightness(window.Pointer)

    let setBordered (bordered:bool) (window:Window) =
        Native.SDL_SetWindowBordered(window.Pointer, if bordered then 1 else 0)

    let setMaximumSize (w:int<px>,h:int<px>) (window:Window) =
        Native.SDL_SetWindowMaximumSize(window.Pointer, w/1<px>, h/1<px>)

    let getMaximumSize (window:Window) :int<px> * int<px> =
        let mutable x = 0
        let mutable y = 0
        Native.SDL_GetWindowMaximumSize (window.Pointer, &&x, &&y)
        (x * 1<px>, y * 1<px>)

    let setMinimumSize (w:int<px>,h:int<px>) (window:Window) =
        Native.SDL_SetWindowMinimumSize(window.Pointer, w/1<px>, h/1<px>)

    let getMinimumSize (window:Window) :int<px> * int<px> =
        let mutable x = 0
        let mutable y = 0
        Native.SDL_GetWindowMinimumSize (window.Pointer, &&x, &&y)
        (x * 1<px>, y * 1<px>)

    let getGrab (window:Window) :bool =
        Native.SDL_GetWindowGrab(window.Pointer)
        <> 0

    let setGrab (grab:bool) (window:Window) =
        Native.SDL_SetWindowGrab(window.Pointer,if grab then 1 else 0)

    let getGrabbedWindow () : Window =
        new SDL.Utility.Pointer(Native.SDL_GetGrabbedWindow(), fun x->())

    let setData (name:string) (data:IntPtr) (window:Window) : IntPtr =
        name
        |> SDL.Utility.withAsciiString(fun ptr->Native.SDL_SetWindowData(window.Pointer,ptr,data))

    let getData (name:string) (window:Window) :IntPtr =
        name
        |> SDL.Utility.withAsciiString(fun ptr->Native.SDL_GetWindowData(window.Pointer,ptr))

    let getPixelFormat (window:Window) :uint32 =
        Native.SDL_GetWindowPixelFormat(window.Pointer)

    let getId (window:Window) :uint32 =
        Native.SDL_GetWindowID(window.Pointer)

    let getFlags (window:Window) :uint32 =
        Native.SDL_GetWindowFlags(window.Pointer)

    let updateSurface (window:Window) :unit =
        Native.SDL_UpdateWindowSurface(window.Pointer)
        |> ignore

    let getSurface (window:Window) :SDL.Utility.Pointer =
        new SDL.Utility.Pointer(Native.SDL_GetWindowSurface(window.Pointer), fun e->())

    let fromId (id:uint32) :Window =
        new SDL.Utility.Pointer(Native.SDL_GetWindowFromID(id), fun e->())

    let getDisplayIndex (window:Window) :int =
        Native.SDL_GetWindowDisplayIndex(window.Pointer)

    let getDisplayMode (window:Window) :SDL.Video.DisplayMode =
        let mutable mode = new SDL.Video.SDL_DisplayMode()

        Native.SDL_GetWindowDisplayMode(window.Pointer, ((&&mode) |> NativePtr.toNativeInt ))
        |> ignore

        mode
        |> SDL.Video.SDL_DisplayModeToDisplayMode

    let setDisplayMode (window:Window) (mode:SDL.Video.DisplayMode) :unit =
        let mutable mode' = mode |> Video.DisplayModeToSDL_DisplayMode
        let ptr = 
            &&mode'
            |> NativePtr.toNativeInt

        Native.SDL_SetWindowDisplayMode(window.Pointer, ptr)
        |> ignore

    let setFullscreen (flags:uint32) (window:Window) =
        Native.SDL_SetWindowFullscreen(window.Pointer,flags)
        |> ignore

    let setWindowIcon (icon:SDL.Utility.Pointer) (window:Window) =
        Native.SDL_SetWindowIcon(window.Pointer,icon.Pointer)