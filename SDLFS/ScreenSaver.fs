namespace SDL

open System.Runtime.InteropServices
open System

module ScreenSaver =
    module private Native =
        //screen saver
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_IsScreenSaverEnabled()
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_EnableScreenSaver()
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_DisableScreenSaver()

    let enable () = Native.SDL_EnableScreenSaver()

    let disable () = Native.SDL_DisableScreenSaver()

    let isEnabled () = Native.SDL_IsScreenSaverEnabled() <> 0

