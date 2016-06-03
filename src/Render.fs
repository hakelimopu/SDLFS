namespace SDL

#nowarn "9"

open System.Runtime.InteropServices
open System
open SDL
open SDL.Texture
open SDL.Surface

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute>]
module Render = 

    [<Flags>]
    type BlendMode =
        | None  = 0x00000000
        | Blend = 0x00000001
        | Add   = 0x00000002
        | Mod   = 0x00000004

    [<Flags>]
    type Flags = 
        | Software      = 0x00000001
        | Accelerated   = 0x00000002
        | PresentVSync  = 0x00000004
        | TargetTexture = 0x00000008

    [<Flags>]
    type Flip =
        | None = 0x00000000
        | Horizontal = 0x00000001
        | Vertical = 0x00000002

    type Renderer = SDL.Utility.Pointer

    [<StructLayout(LayoutKind.Sequential)>]
    type private SDL_RendererInfo =
        struct
            [<MarshalAs(UnmanagedType.LPStr)>]
            val mutable name:string
            val mutable flags:uint32
            val mutable num_texture_formats:uint32
            [<MarshalAs(UnmanagedType.ByValArray, SizeConst=16, ArraySubType = UnmanagedType.U4 )>]
            val mutable texture_formats:uint32[]
            val mutable max_texture_width:int
            val mutable max_texture_height:int
        end

     type RendererInfo =
         {Name:string;
         Flags:Flags;
         TextureFormats:seq<uint32>;
         MaximumTextureWidth:int<px>;
         MaximumTextureHeight:int<px>}
                                           
    module private Native =
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_CreateRenderer(IntPtr window, int index, uint32 flags)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_DestroyRenderer(IntPtr renderer)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderClear(IntPtr renderer)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetRenderDrawColor(IntPtr renderer, uint8 r, uint8 g, uint8 b, uint8 a)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_RenderPresent(IntPtr rendererr)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderSetLogicalSize(IntPtr renderer, int w, int h)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderCopy(IntPtr renderer, IntPtr texture, IntPtr srcrect, IntPtr dstrect)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetNumRenderDrivers()
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetRenderDriverInfo(int index, IntPtr info)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_CreateWindowAndRenderer(int width, int height, uint32 window_flags, IntPtr window, IntPtr renderer)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_CreateSoftwareRenderer(IntPtr surface)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_GetRenderer(IntPtr  window)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetRendererInfo(IntPtr renderer, IntPtr info)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetRendererOutputSize(IntPtr renderer, IntPtr w, IntPtr h)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderTargetSupported(IntPtr renderer)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetRenderTarget(IntPtr renderer, IntPtr texture)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_GetRenderTarget(IntPtr renderer)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_RenderGetLogicalSize(IntPtr renderer, IntPtr w, IntPtr h)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderSetViewport(IntPtr renderer, IntPtr rect)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_RenderGetViewport(IntPtr renderer, IntPtr rect)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderSetClipRect(IntPtr renderer, IntPtr rect)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_RenderGetClipRect(IntPtr renderer, IntPtr rect)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderIsClipEnabled(IntPtr renderer)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderSetScale(IntPtr  renderer, float scaleX, float scaleY)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_RenderGetScale(IntPtr renderer, IntPtr scaleX, IntPtr scaleY)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetRenderDrawColor(IntPtr renderer, IntPtr  r, IntPtr  g, IntPtr  b, IntPtr  a)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetRenderDrawBlendMode(IntPtr renderer, int blendMode)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetRenderDrawBlendMode(IntPtr renderer, IntPtr blendMode)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderDrawPoint(IntPtr renderer, int x, int y)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderDrawPoints(IntPtr renderer, IntPtr   points, int count)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderDrawLine(IntPtr renderer, int x1, int y1, int x2, int y2)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderDrawLines(IntPtr renderer, IntPtr   points, int count)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderDrawRect(IntPtr renderer, IntPtr rect)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderDrawRects(IntPtr renderer, IntPtr rects, int count)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderFillRect(IntPtr renderer, IntPtr rect)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderFillRects(IntPtr renderer, IntPtr rects, int count)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderCopyEx(IntPtr renderer, IntPtr  texture, IntPtr srcrect, IntPtr dstrect, double angle, IntPtr  center, int flip)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderReadPixels(IntPtr  renderer,IntPtr rect,uint32 format,IntPtr pixels, int pitch)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GL_BindTexture(IntPtr texture, IntPtr texw, IntPtr texh)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GL_UnbindTexture(IntPtr texture)

    let create (window:SDL.Utility.Pointer) (index:int) (flags:Flags) :Renderer =
        let ptr = Native.SDL_CreateRenderer(window.Pointer, index, flags |> uint32)
        new SDL.Utility.Pointer(ptr,Native.SDL_DestroyRenderer)

    let createSoftware (surface:Surface) (index:int) (flags:Flags) :Renderer =
        let ptr = Native.SDL_CreateSoftwareRenderer(surface.Pointer)
        new SDL.Utility.Pointer(ptr,Native.SDL_DestroyRenderer)

    let clear (renderer:Renderer) :bool =
        0 = Native.SDL_RenderClear(renderer.Pointer)

    let present (renderer:Renderer) :unit =
        Native.SDL_RenderPresent(renderer.Pointer)

    let setDrawColor (r, g, b, a) (renderer:Renderer) =
        0 = Native.SDL_SetRenderDrawColor(renderer.Pointer,r,g,b,a)

    let setLogicalSize (w:int<px>,h:int<px>) (renderer:Renderer) =
        0 = Native.SDL_RenderSetLogicalSize(renderer.Pointer,w |> int,h |> int)

    let copy (texture:SDL.Texture.Texture) (srcrect:SDL.Geometry.Rectangle option) (dstrect:SDL.Geometry.Rectangle option) (renderer:Renderer) =
        SDL.Geometry.withSDLRectPointer(fun src -> SDL.Geometry.withSDLRectPointer(fun dst -> 0 = Native.SDL_RenderCopy(renderer.Pointer,texture.Pointer,src,dst)) dstrect) srcrect

    