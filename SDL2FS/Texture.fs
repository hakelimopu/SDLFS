namespace SDL

#nowarn "9"

open System.Runtime.InteropServices
open System
open SDL
open Microsoft.FSharp.NativeInterop

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute>]
module Texture = 

    [<Flags>]
    type BlendMode =
        | None  = 0x00000000
        | Blend = 0x00000001
        | Add   = 0x00000002
        | Mod   = 0x00000004

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
        extern IntPtr SDL_CreateTexture(IntPtr renderer, uint32 format, int access, int w, int h)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_DestroyTexture(IntPtr texture)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_CreateTextureFromSurface(IntPtr renderer, IntPtr surface)

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_QueryTexture(IntPtr texture, uint32* format, int* access, int* w, int* h)

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetTextureColorMod(IntPtr texture, uint8 r, uint8 g, uint8 b)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetTextureColorMod(IntPtr texture, uint8*  r, uint8*  g, uint8*  b)

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetTextureAlphaMod(IntPtr texture, uint8 alpha)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetTextureAlphaMod(IntPtr texture, uint8*  alpha)

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetTextureBlendMode(IntPtr texture, int blendMode)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetTextureBlendMode(IntPtr texture, int* blendMode)

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_UpdateTexture(IntPtr texture, IntPtr  rect, IntPtr pixels, int pitch)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_UpdateYUVTexture(IntPtr texture, IntPtr  rect, IntPtr Yplane, int Ypitch, IntPtr Uplane, int Upitch, IntPtr Vplane, int Vpitch)////TODO

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_LockTexture(IntPtr texture, IntPtr  rect, IntPtr pixels, IntPtr pitch)//TODO
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_UnlockTexture(IntPtr texture)//TODO


    let create (format:uint32) (access: Access) (w: int,h: int) (renderer:SDL.Utility.Pointer) =
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

    let query (texture:Texture) : (uint32 * Access * int * int) option =
        let mutable format = 0u
        let mutable access = 0
        let mutable width = 0
        let mutable height = 0
        let result = Native.SDL_QueryTexture(texture.Pointer, &&format, &&access, &&width, &&height)
        if result = 0 then
            Some (format, access |> enum<Access>, width, height)
        else
            None

    let setModulation (color:Pixel.Color) (texture:Texture): bool =
        let colorResult = Native.SDL_SetTextureColorMod(texture.Pointer, color.Red, color.Green, color.Blue)
        if colorResult = 0 then
            Native.SDL_SetTextureAlphaMod(texture.Pointer, color.Alpha) = 0
        else
            false

    let getModulation (texture:Texture) : Pixel.Color option =
        let mutable r = 0uy
        let mutable g = 0uy
        let mutable b = 0uy

        let colorResult = 
            Native.SDL_GetTextureColorMod(texture.Pointer, &&r, &&g, &&b)

        if colorResult = 0 then
            let mutable a = 0uy
            let alphaResult = 
                Native.SDL_GetTextureAlphaMod(texture.Pointer, &&a)
            if alphaResult = 0 then
                Some {Pixel.Color.Red = r; Green = g; Blue = b; Alpha = a}
            else
                None
        else
            None

    let setBlendMode (blendMode:BlendMode) (texture:Texture) :bool = 
        Native.SDL_SetTextureBlendMode(texture.Pointer, blendMode |> int) = 0

    let getBlendMode (texture:Texture) : BlendMode option =
        let mutable blendMode = 0
        let result = 
            Native.SDL_GetTextureBlendMode(texture.Pointer, &&blendMode)
        if result = 0 then
            Some (blendMode |> enum<BlendMode>)
        else
            None