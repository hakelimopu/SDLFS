﻿namespace SDL

#nowarn "9"

open System.Runtime.InteropServices
open System
open Microsoft.FSharp.NativeInterop

module Video =
    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_DisplayMode =
        struct
            val mutable format       : uint32
            val mutable w            : int
            val mutable h            : int 
            val mutable refresh_rate : int
            val mutable driverdata   : IntPtr
        end

    module private Native =
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetNumVideoDrivers();
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_GetVideoDriver(int index);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_VideoInit(IntPtr driver_name);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_VideoQuit();
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_GetCurrentVideoDriver();
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetNumVideoDisplays();
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr  SDL_GetDisplayName(int displayIndex);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetDisplayBounds(int displayIndex, SDL.Geometry.SDL_Rect* rect);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetDisplayDPI(int displayIndex, float* ddpi, float* hdpi, float* vdpi);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetNumDisplayModes(int displayIndex);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetDisplayMode(int displayIndex, int modeIndex, IntPtr mode);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetCurrentDisplayMode(int displayIndex, IntPtr mode);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_GetClosestDisplayMode(int displayIndex, IntPtr mode, IntPtr closest);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetDesktopDisplayMode(int displayIndex, IntPtr mode);

    type DisplayMode =
        {Format: uint32;
        Width:int;
        Height:int;
        RefreshRate:int;
        Data:IntPtr}

    let internal SDL_DisplayModeToDisplayMode (mode:SDL_DisplayMode) :DisplayMode =
        {Format = mode.format;
        Width = mode.w * 1;
        Height= mode.h * 1;
        RefreshRate = mode.refresh_rate * 1;
        Data = mode.driverdata}

    let internal DisplayModeToSDL_DisplayMode (mode:DisplayMode) :SDL_DisplayMode =
        SDL_DisplayMode(
            format = mode.Format,
            w = mode.Width / 1,
            h= mode.Height / 1,
            refresh_rate = mode.RefreshRate / 1,
            driverdata = mode.Data)

    let getDrivers () :string list =
        Native.SDL_GetNumVideoDrivers()
        |> List.unfold (fun index ->
            if index=0 then
                None
            else
                (Native.SDL_GetVideoDriver(index-1) |> SDL.Utility.intPtrToStringAscii, index-1) |> Some)
        |> List.rev

    let init (driverName:string) =
        driverName
        |> SDL.Utility.withAsciiString(fun ptr-> Native.SDL_VideoInit(ptr) = 0)

    let quit () =
        Native.SDL_VideoQuit()

    let getCurrentDriver () =
        Native.SDL_GetCurrentVideoDriver()
        |> SDL.Utility.intPtrToStringAscii

    let getDisplayCount () : int =
        Native.SDL_GetNumVideoDisplays()

    let getDisplayName (index:int) :string = 
        Native.SDL_GetDisplayName(index)
        |> SDL.Utility.intPtrToStringAscii

    let getDisplayBounds (index:int) : Geometry.Rectangle =
        let mutable rect:SDL.Geometry.SDL_Rect = new SDL.Geometry.SDL_Rect()

        Native.SDL_GetDisplayBounds(index,&&rect)
        |> ignore

        rect
        |> SDL.Geometry.sdl_RectToRectangle

    type DisplayDPI = 
        {Diagonal: float; Horizontal: float; Vertical:float}

    let getDisplayDPI (index:int) : DisplayDPI option =
        let mutable ddpi:float = 0.0
        let mutable hdpi:float = 0.0
        let mutable vdpi:float = 0.0

        let result = 
            Native.SDL_GetDisplayDPI(index, &&ddpi, &&hdpi, &&vdpi)

        if result = 0 then
            {Diagonal = ddpi; Horizontal = hdpi; Vertical = vdpi} |> Some
        else
            None

    type DisplayProperties = 
        {Index:int; 
        Name:string; 
        Bounds: Geometry.Rectangle; 
        DPI: DisplayDPI option}

//    let getDisplays() : DisplayProperties list =
//        let displayCount = getDisplayCount()
//
//        [0..(displayCount-1)]
//        |> List.map 
//            (fun displayIndex->
//                let displayName = 
//                )

    let getDisplayModes (index:int) : List<DisplayMode> =
        Native.SDL_GetNumDisplayModes(index)
        |> List.unfold (fun modeIndex ->
            if modeIndex=0 then
                None
            else
                let mutable mode:SDL_DisplayMode = new SDL_DisplayMode()
                let ptr = NativePtr.toNativeInt &&mode
                
                Native.SDL_GetDisplayMode(index,modeIndex,ptr)
                |> ignore

                (SDL_DisplayModeToDisplayMode mode, index-1) |> Some)
        |> List.rev
    
    let getCurrentDisplayMode (index:int) :DisplayMode =
        let mutable mode:SDL_DisplayMode = new SDL_DisplayMode()
        let ptr = NativePtr.toNativeInt &&mode
                
        Native.SDL_GetCurrentDisplayMode(index, ptr)
        |> ignore

        SDL_DisplayModeToDisplayMode mode
    
    let getDesktopDisplayMode (index:int) :DisplayMode =
        let mutable mode:SDL_DisplayMode = new SDL_DisplayMode()
        let ptr = NativePtr.toNativeInt &&mode
                
        Native.SDL_GetDesktopDisplayMode(index, ptr)
        |> ignore

        SDL_DisplayModeToDisplayMode mode

    let getClosestDisplayMode (index:int) (mode:DisplayMode) :DisplayMode =
        let mutable displayMode' = DisplayModeToSDL_DisplayMode mode
        let ptr = NativePtr.toNativeInt  &&displayMode'

        let mutable resultMode:SDL_DisplayMode = new SDL_DisplayMode()
        let resultPtr = NativePtr.toNativeInt &&resultMode
                
        Native.SDL_GetClosestDisplayMode(index, ptr, resultPtr)
        |> ignore

        SDL_DisplayModeToDisplayMode resultMode
