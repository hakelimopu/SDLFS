namespace SDL

#nowarn "9" 

open System.Runtime.InteropServices
open System

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute>]
module Version =

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_version =
        struct
            val major: uint8
            val minor: uint8
            val patch: uint8
        end

    module private Native =
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_GetVersion(SDL_version& ver)    
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)>]
        extern IntPtr SDL_GetRevision()
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetRevisionNumber()

    type Version = {Major: uint8; Minor: uint8; Patch: uint8}

    let get () :Version =
        let mutable version = new SDL_version()
        Native.SDL_GetVersion(&version)
        {Major = version.major; Minor=version.minor; Patch = version.patch}

    let getRevision () :string =
        Native.SDL_GetRevision()
        |> SDL.Utility.intPtrToStringUtf8

    let getRevisionNumber() :int =
        Native.SDL_GetRevisionNumber()