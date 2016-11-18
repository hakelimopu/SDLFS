namespace SDL

#nowarn "9"

open System.Runtime.InteropServices
open System
open SDL
open Microsoft.FSharp.NativeInterop

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute>]
module Texture = 

    type Access =
        | Static    = 0
        | Streaming = 1
        | Target    = 2

    [<Flags>]
    type Modulate = 
        | None  = 0x00000000
        | Color = 0x00000001
        | Alpha = 0x00000002

    type Texture = SDL.Utility.Pointer

    module private Native =
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_CreateTexture(IntPtr renderer, uint32 format, int access, int w, int h)//DONE
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_DestroyTexture(IntPtr texture)//DONE
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_CreateTextureFromSurface(IntPtr renderer, IntPtr surface)//DONE
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_QueryTexture(IntPtr texture, IntPtr format, IntPtr access, IntPtr w, IntPtr h)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetTextureColorMod(IntPtr texture, uint8 r, uint8 g, uint8 b)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetTextureColorMod(IntPtr texture, IntPtr  r, IntPtr  g, IntPtr  b)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetTextureAlphaMod(IntPtr texture, uint8 alpha)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetTextureAlphaMod(IntPtr texture, IntPtr  alpha)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetTextureBlendMode(IntPtr texture, int blendMode)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetTextureBlendMode(IntPtr texture, IntPtr blendMode)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_UpdateTexture(IntPtr texture, IntPtr  rect, IntPtr pixels, int pitch)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_UpdateYUVTexture(IntPtr texture, IntPtr  rect, IntPtr Yplane, int Ypitch, IntPtr Uplane, int Upitch, IntPtr Vplane, int Vpitch)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_LockTexture(IntPtr texture, IntPtr  rect, IntPtr pixels, IntPtr pitch)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_UnlockTexture(IntPtr texture)


    let create format (access: Access) (w: int,h: int) (renderer:SDL.Utility.Pointer) =
        let ptr = Native.SDL_CreateTexture(renderer.Pointer,format,access |> int,w,h)
        new SDL.Utility.Pointer(ptr, Native.SDL_DestroyTexture)

    let fromSurface (renderer:SDL.Utility.Pointer) surface =
        let ptr = Native.SDL_CreateTextureFromSurface(renderer.Pointer,surface)
        new SDL.Utility.Pointer(ptr, Native.SDL_DestroyTexture)

    let update (dstrect:SDL.Geometry.Rectangle option) (src:SDL.Surface.Surface) (texture:Texture) : bool =
        dstrect
        |> SDL.Geometry.withSDLRectPointer (fun rectptr->
            let surf =
                src.Pointer
                |> NativePtr.ofNativeInt<SDL.Surface.SDL_Surface>
                |> NativePtr.read
            0 = Native.SDL_UpdateTexture(texture.Pointer,rectptr,surf.pixels,surf.pitch)) 
