namespace SDL

open System.Runtime.InteropServices
open System

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute>]
module Init = 
    type MainFunction = nativeint * IntPtr -> nativeint

    module private Native =
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint="SDL_Init")>]
        extern int SdlInit(uint32 flags)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint="SDL_Quit")>]
        extern void SdlQuit()
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint="SDL_InitSubSystem")>]
        extern int SdlInitSubSystem(uint32 flags)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint="SDL_QuitSubSystem")>]
        extern void SdlQuitSubSystem(uint32 flags)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint="SDL_WasInit")>]
        extern uint32 SdlWasInit(uint32 flags)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint="SDL_SetMainReady")>]
        extern void SdlSetMainReady()
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint="SDL_WinRTRunApp")>]
        extern void SdlWinRTRunApp(MainFunction mainFunction,IntPtr reserved)

    [<Flags>]
    type Init =
        | None           = 0x00000000
        | Timer          = 0x00000001
        | Audio          = 0x00000010
        | Video          = 0x00000020
        | Joystick       = 0x00000200
        | Haptic         = 0x00001000
        | GameController = 0x00002000
        | Events         = 0x00004000
        | Everything     = 0x0000FFFF

    type System(flags:Init) =
        do
            Native.SdlInit(flags |> uint32) |> ignore
        member this.initSubSystem (flags: Init) :bool =
            0 = Native.SdlInitSubSystem(flags |> uint32)
        member this.quitSubSystem (flags: Init) :unit =
            Native.SdlQuitSubSystem(flags |> uint32)
        member this.wasInit (flags:Init) :bool =
            flags = (Native.SdlWasInit(flags |> uint32) |> int |> enum<Init>)
        interface IDisposable with
            member this.Dispose() =
                Native.SdlQuit()

    let setMainReady () =
        Native.SdlSetMainReady()

    let winRTRunApp (mainFunction:MainFunction) =
        Native.SdlWinRTRunApp(mainFunction, IntPtr.Zero)
