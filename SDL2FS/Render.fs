namespace SDL

#nowarn "9"

open System.Runtime.InteropServices
open System
open SDL
open SDL.Surface
open FSharp.NativeInterop

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
    type internal SDL_RendererInfo =
        struct
            val mutable name:IntPtr
            val mutable flags:uint32
            val mutable num_texture_formats:uint32
            val mutable texture_format0: uint32//this is goofy, but works for now
            val mutable texture_format1: uint32
            val mutable texture_format2: uint32
            val mutable texture_format3: uint32
            val mutable texture_format4: uint32
            val mutable texture_format5: uint32
            val mutable texture_format6: uint32
            val mutable texture_format7: uint32
            val mutable texture_format8: uint32
            val mutable texture_format9: uint32
            val mutable texture_format10: uint32
            val mutable texture_format11: uint32
            val mutable texture_format12: uint32
            val mutable texture_format13: uint32
            val mutable texture_format14: uint32
            val mutable texture_format15: uint32
            val mutable max_texture_width:int
            val mutable max_texture_height:int
        end

     type RendererInfo =
         {Name:string;
         Flags:Flags;
         TextureFormats:seq<uint32>;
         MaximumTextureWidth:int;
         MaximumTextureHeight:int}
                                           
    module private Native =
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_CreateRenderer(IntPtr window, int index, uint32 flags)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_DestroyRenderer(IntPtr renderer)//DONE

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderClear(IntPtr renderer)//DONE
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_RenderPresent(IntPtr renderer)//DONE

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetNumRenderDrivers()//DONE
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetRenderDriverInfo(int index, SDL_RendererInfo* info)//DONE

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_CreateSoftwareRenderer(IntPtr surface)//DONE

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetRendererInfo(IntPtr renderer, SDL_RendererInfo*  info)//DONE
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetRendererOutputSize(IntPtr renderer, int* w, int* h)//DONE

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderTargetSupported(IntPtr renderer)//DONE
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetRenderTarget(IntPtr renderer, IntPtr texture)//DONE
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_GetRenderTarget(IntPtr renderer)

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderSetLogicalSize(IntPtr renderer, int w, int h)//DONE
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_RenderGetLogicalSize(IntPtr renderer, int* w, int* h)//DONE

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderSetViewport(IntPtr renderer, Geometry.SDL_Rect* rect)//DONE
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_RenderGetViewport(IntPtr renderer, Geometry.SDL_Rect* rect)//DONE

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderSetClipRect(IntPtr renderer, Geometry.SDL_Rect* rect)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_RenderGetClipRect(IntPtr renderer, Geometry.SDL_Rect* rect)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderIsClipEnabled(IntPtr renderer)

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderSetScale(IntPtr  renderer, float scaleX, float scaleY)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_RenderGetScale(IntPtr renderer, IntPtr scaleX, IntPtr scaleY)

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetRenderDrawColor(IntPtr renderer, uint8 r, uint8 g, uint8 b, uint8 a)//DONE
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetRenderDrawColor(IntPtr renderer, uint8*  r, uint8*  g, uint8*  b, uint8*  a)//DONE

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetRenderDrawBlendMode(IntPtr renderer, int blendMode)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetRenderDrawBlendMode(IntPtr renderer, IntPtr blendMode)

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderDrawPoint(IntPtr renderer, int x, int y)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderDrawPoints(IntPtr renderer, IntPtr points, int count)

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
        extern int SDL_RenderCopy(IntPtr renderer, IntPtr texture, IntPtr srcrect, IntPtr dstrect)//DONE
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderCopyEx(IntPtr renderer, IntPtr  texture, IntPtr srcrect, IntPtr dstrect, double angle, IntPtr  center, int flip)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_RenderReadPixels(IntPtr  renderer,IntPtr rect,uint32 format,IntPtr pixels, int pitch)
        
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GL_BindTexture(IntPtr texture, IntPtr texw, IntPtr texh)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GL_UnbindTexture(IntPtr texture)

    /////////////////////////////////////////////////////////////////////////////////////////////////////
    // Renderer
    /////////////////////////////////////////////////////////////////////////////////////////////////////

    let create (window:SDL.Utility.Pointer) (index:int option) (flags:Flags) :Renderer =
        let ptr = Native.SDL_CreateRenderer(window.Pointer, (if index.IsSome then index.Value else -1), flags |> uint32)
        new SDL.Utility.Pointer(ptr,Native.SDL_DestroyRenderer)

    let createSoftware (surface:Surface) :Renderer =
        let ptr = Native.SDL_CreateSoftwareRenderer(surface.Pointer)
        new SDL.Utility.Pointer(ptr,Native.SDL_DestroyRenderer)

    let clear (renderer:Renderer) :bool =
        0 = Native.SDL_RenderClear(renderer.Pointer)

    let present (renderer:Renderer) :unit =
        Native.SDL_RenderPresent(renderer.Pointer)

    let setDrawColor (color:Pixel.Color) (renderer:Renderer) :bool =
        0 = Native.SDL_SetRenderDrawColor(renderer.Pointer,color.Red,color.Green,color.Blue,color.Alpha)

    let getDrawColor (renderer:Renderer) :Pixel.Color option = 
        let mutable r = 0uy
        let mutable g = 0uy
        let mutable b = 0uy
        let mutable a = 0uy

        let result = 
            Native.SDL_GetRenderDrawColor(renderer.Pointer, &&r, &&g, &&b, &&a)

        if result=0 then
            Some {Pixel.Color.Red = r; Green = g; Blue=b; Alpha=a}
        else
            None

    let setLogicalSize (w:int,h:int) (renderer:Renderer) :bool  =
        0 = Native.SDL_RenderSetLogicalSize(renderer.Pointer,w |> int,h |> int)

    let getLogicalSize (renderer:Renderer) : int * int =
        let mutable w = 0
        let mutable h = 0
        Native.SDL_RenderGetLogicalSize(renderer.Pointer, &&w, &&h)
        (w,h)

    let copy (texture:SDL.Texture.Texture) (srcrect:SDL.Geometry.Rectangle option) (dstrect:SDL.Geometry.Rectangle option) (renderer:Renderer) :bool =
        SDL.Geometry.withSDLRectPointer(fun src -> SDL.Geometry.withSDLRectPointer(fun dst -> 0 = Native.SDL_RenderCopy(renderer.Pointer,texture.Pointer,src,dst)) dstrect) srcrect

    //copyEx
    //readPixels

    let private SDL_RendererInfoToRendererInfo (from:SDL_RendererInfo) : RendererInfo =
        {Name=Utility.intPtrToStringAscii from.name;
        Flags = from.flags |> int32 |> enum<Flags>; 
        TextureFormats=[from.texture_format0;from.texture_format1;from.texture_format2;from.texture_format3;from.texture_format4;from.texture_format5;from.texture_format6;from.texture_format7;from.texture_format8;from.texture_format9;from.texture_format10;from.texture_format11;from.texture_format12;from.texture_format13;from.texture_format14;from.texture_format15] |> List.take (from.num_texture_formats |> int); 
        MaximumTextureWidth=from.max_texture_width;//this is always returning as 0
        MaximumTextureHeight=from.max_texture_height}//this is always returning as 0

    let internal getDriverInfo (index:int) : RendererInfo =
        let mutable result = new SDL_RendererInfo()

        Native.SDL_GetRenderDriverInfo (index, &&result)
        |> ignore

        result
        |> SDL_RendererInfoToRendererInfo

    let getDrivers () : RendererInfo list=
        [0..(Native.SDL_GetNumRenderDrivers()-1)]
        |> List.map getDriverInfo

    let getInfo (renderer:Renderer) : RendererInfo option =
        let mutable info = new SDL_RendererInfo()
        
        let result = 
            Native.SDL_GetRendererInfo (renderer.Pointer, &&info)

        if result=0 then
            Some (info |> SDL_RendererInfoToRendererInfo)
        else
            None

    let getOutputSize (renderer:Renderer) : (int * int) option =
        let mutable w = 0
        let mutable h = 0

        let result = 
            Native.SDL_GetRendererOutputSize(renderer.Pointer, &&w, &&h)

        if result = 0 then
            Some (w,h)
        else
            None

    let supportsRenderTarget (renderer:Renderer) : bool = 
        Native.SDL_RenderTargetSupported(renderer.Pointer) <> 0

    let setRenderTarget (texture:Texture.Texture option) (renderer:Renderer) : bool =
        Native.SDL_SetRenderTarget(renderer.Pointer,if texture.IsSome then texture.Value.Pointer else IntPtr.Zero) <> 0

    let setViewPort (rect:Geometry.Rectangle option) (renderer:Renderer) : bool =
        if rect.IsSome then
            let mutable r = Geometry.rectangleToSDL_Rect rect.Value
            Native.SDL_RenderSetViewport(renderer.Pointer,&&r) <> 0
        else
            Native.SDL_RenderSetViewport(renderer.Pointer,IntPtr.Zero |> NativePtr.ofNativeInt<Geometry.SDL_Rect>) <> 0

    let getViewPort (renderer:Renderer) : Geometry.Rectangle =
        let mutable r = new Geometry.SDL_Rect()

        Native.SDL_RenderGetViewport(renderer.Pointer, &&r)

        r
        |> Geometry.sdl_RectToRectangle

//    let setClip (rect:Geometry.Rectangle option) (renderer:Renderer) : bool =
//        false
//
//    let getClip (renderer:Renderer) : Geometry.Rectangle option =
//        None

    let isClipping (renderer:Renderer) : bool =
        Native.SDL_RenderIsClipEnabled(renderer.Pointer) = 0

