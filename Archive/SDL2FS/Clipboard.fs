namespace SDL

open System.Runtime.InteropServices
open System

module Clipboard =
        
    module private Native =
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetClipboardText(IntPtr text);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_GetClipboardText();
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_HasClipboardText();

    let setText (text:string) :bool =
        text
        |> SDL.Utility.withUtf8String(fun ptr->Native.SDL_SetClipboardText(ptr))
        <> 0

    let getText () :string =
        let ptr = Native.SDL_GetClipboardText()
        let text = 
            ptr
            |> SDL.Utility.intPtrToStringUtf8
        if ptr<>IntPtr.Zero then
            SDL.Utility.Native.SDL_free(ptr)
        else
            ()
        text

    let hasText () :bool =
        Native.SDL_HasClipboardText()
        <> 0