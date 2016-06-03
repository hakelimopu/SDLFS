namespace SDL


open System
open System.Runtime.InteropServices

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute>]
module Cursor = 

    type SDL_Cursor = IntPtr

    type SystemCursor =
        | Arrow = 0
        | Ibeam = 1 
        | Wait = 2 
        | Crosshair=3
        | WaitArrow=4
        | SizeNWSE=5
        | SizeNESW=6
        | SizeWE=7
        | SizeNS=8  
        | SizeAll=9
        | No=10 
        | Hand=11     

    module internal Native = 
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern SDL_Cursor SDL_CreateCursor(uint8 * data,uint8 * mask,int w, int h, int hot_x,int hot_y)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern SDL_Cursor SDL_CreateColorCursor(IntPtr surface,int hot_x,int hot_y)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern SDL_Cursor SDL_CreateSystemCursor(SystemCursor id)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_SetCursor(SDL_Cursor  cursor)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern SDL_Cursor SDL_GetCursor()
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern SDL_Cursor SDL_GetDefaultCursor()
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_FreeCursor(SDL_Cursor  cursor)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_ShowCursor(int toggle)

