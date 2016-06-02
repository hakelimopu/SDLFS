namespace SDL

open System
open System.Runtime.InteropServices

#nowarn "9"

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute>]
module RWops = 
    type RWops = IntPtr

    module internal Native =
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)>]
        extern RWops SDL_RWFromFile(IntPtr file, string mode)
    

