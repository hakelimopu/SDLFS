namespace SDL

open System.Runtime.InteropServices
open System

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute>]
module Platform = 

    module private Native =
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_GetPlatform ();


    let getPlatform () :string =
        Native.SDL_GetPlatform()
        |> Utility.intPtrToStringAscii

module CpuInfo =
    module private Native =
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetCPUCount();
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetCPUCacheLineSize();
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_HasRDTSC();
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_HasAltiVec();
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_HasMMX();
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_Has3DNow();
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_HasSSE();
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_HasSSE2();
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_HasSSE3();
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_HasSSE41();
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_HasSSE42();
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_HasAVX();
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_HasAVX2();
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetSystemRAM();

    let getCPUCount()         = Native.SDL_GetCPUCount()
    let getCPUCacheLineSize() = Native.SDL_GetCPUCacheLineSize()
    let hasRDTSC()            = Native.SDL_HasRDTSC() <> 0
    let hasAltiVec()          = Native.SDL_HasAltiVec() <> 0
    let hasMMX()              = Native.SDL_HasMMX() <> 0
    let has3DNow()            = Native.SDL_Has3DNow() <> 0
    let hasSSE()              = Native.SDL_HasSSE() <> 0
    let hasSSE2()             = Native.SDL_HasSSE2() <> 0
    let hasSSE3()             = Native.SDL_HasSSE3() <> 0
    let hasSSE41()            = Native.SDL_HasSSE41() <> 0
    let hasSSE42()            = Native.SDL_HasSSE42() <> 0
    let hasAVX()              = Native.SDL_HasAVX() <> 0
    let hasAVX2()             = Native.SDL_HasAVX2() <> 0
    let getSystemRAM()        = Native.SDL_GetSystemRAM()
