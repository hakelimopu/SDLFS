namespace SDL

open System
open System.Runtime.InteropServices
open SDL
open Microsoft.FSharp.NativeInterop
open SDL.Pixel

#nowarn "9"

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute>]
module Surface = 
    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_Surface =
        struct
            val flags :uint32
            val format: IntPtr//SDL_PixelFormat*
            val w:int
            val h:int
            val pitch:int
            val pixels: IntPtr
            val userdate: IntPtr
            val locked: int
            val lock_data: IntPtr
            val clip_rect: SDL.Geometry.SDL_Rect
            val map: IntPtr
            val refcount: int
        end

    type Surface = SDL.Utility.Pointer

    module private Native =
        //Creating RGB surfaces
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_CreateRGBSurface(uint32 flags, int width, int height, int depth, uint32 Rmask, uint32 Gmask, uint32 Bmask, uint32 Amask)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_CreateRGBSurfaceFrom(IntPtr pixels, int width, int height, int depth, int pitch, uint32 Rmask, uint32 Gmask, uint32 Bmask, uint32 Amask)

        //Clean up surface
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_FreeSurface(IntPtr surface)    

        //Palette
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern  int SDL_SetSurfacePalette(IntPtr surface, IntPtr palette)//TODO

        //Locking
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern  int SDL_LockSurface(IntPtr surface)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern  void SDL_UnlockSurface(IntPtr surface)

        //Bitmaps
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_LoadBMP_RW(IntPtr src, int freesrc)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SaveBMP_RW(IntPtr surface, IntPtr dst, int freedst)

        //RLE
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetSurfaceRLE(IntPtr surface, int flag)

        //Color Key
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetColorKey(IntPtr surface, int flag, uint32 key)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetColorKey(IntPtr surface, uint32 * key)

        //Color mod
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetSurfaceColorMod(IntPtr surface, uint8 r, uint8 g, uint8 b)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetSurfaceColorMod(IntPtr surface, uint8 * r, uint8 * g, uint8 * b)

        //IntPtr Alpha
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetSurfaceAlphaMod(IntPtr surface, uint8 alpha)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetSurfaceAlphaMod(IntPtr surface, uint8 * alpha)

        //Blend Mode
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetSurfaceBlendMode(IntPtr surface, int blendMode)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetSurfaceBlendMode(IntPtr surface, int* blendMode)

        //Clip Rect
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int  SDL_SetClipRect(IntPtr surface, SDL.Geometry.SDL_Rect* rect)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void  SDL_GetClipRect(IntPtr surface, SDL.Geometry.SDL_Rect* rect)

        //Conversions
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_ConvertSurface(IntPtr src, IntPtr fmt, uint32 flags)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_ConvertSurfaceFormat(IntPtr src, uint32 pixel_format, uint32 flags)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_ConvertPixels(int width, int height, uint32 src_format, IntPtr src, int src_pitch, uint32 dst_format, IntPtr dst, int dst_pitch)

        //filling rectangles
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_FillRect(IntPtr dst, IntPtr rect, uint32 color)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_FillRects(IntPtr dst, IntPtr rects, int count, uint32 color)//TODO

        //blitting
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_UpperBlit(IntPtr src, IntPtr srcrect, IntPtr dst, IntPtr dstrect)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_LowerBlit(IntPtr src, IntPtr srcrect, IntPtr dst, IntPtr dstrect)

        //stretching
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SoftStretch(IntPtr src, IntPtr srcrect, IntPtr dst, IntPtr dstrect)

        //scaled blitting
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_UpperBlitScaled(IntPtr src, IntPtr srcrect, IntPtr dst, IntPtr dstrect)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_LowerBlitScaled(IntPtr src, IntPtr srcrect, IntPtr dst, IntPtr dstrect)


    let createRGB (width:int,height:int,depth:int) (rmask:uint32,gmask:uint32,bmask:uint32,amask:uint32) :Surface=
        let ptr = Native.SDL_CreateRGBSurface(0u,width/1,height/1,depth/1,rmask,gmask,bmask,amask)
        new SDL.Utility.Pointer(ptr, Native.SDL_FreeSurface)

    let private getFormat (surface:Surface) :IntPtr =
        let sdlSurface = 
            surface.Pointer
            |> NativePtr.ofNativeInt<SDL_Surface>
            |> NativePtr.read
        sdlSurface.format

    let fillRect (rect:SDL.Geometry.Rectangle option) (color:SDL.Pixel.Color) (surface:Surface) :bool =
        let format = surface |> getFormat
        SDL.Geometry.withSDLRectPointer (fun r->0 = Native.SDL_FillRect(surface.Pointer, r, color |> SDL.Pixel.mapColor format)) rect

    let loadBmp (pixelFormat: uint32) (fileName:string) : Surface =
        let bitmapSurface = Native.SDL_LoadBMP_RW(SDL.Utility.withUtf8String (fun ptr->SDL.RWops.Native.SDL_RWFromFile(ptr,"rb")) fileName, 1)
        let convertedSurface = Native.SDL_ConvertSurfaceFormat(bitmapSurface,pixelFormat,0u)
        Native.SDL_FreeSurface bitmapSurface
        new SDL.Utility.Pointer(convertedSurface, Native.SDL_FreeSurface)

    let saveBmp (fileName:string) (surface:Surface) :bool =
        0 = Native.SDL_SaveBMP_RW(surface.Pointer, SDL.Utility.withUtf8String (fun ptr->SDL.RWops.Native.SDL_RWFromFile(ptr,"wb")) fileName, 1)

    let upperBlit (srcrect:SDL.Geometry.Rectangle option) (src:Surface) (dstrect:SDL.Geometry.Rectangle option) (dst:Surface) =
        SDL.Geometry.withSDLRectPointer (fun srcptr -> SDL.Geometry.withSDLRectPointer (fun dstptr -> 0 = Native.SDL_UpperBlit(src.Pointer,srcptr,dst.Pointer,dstptr)) dstrect) srcrect

    let blit = upperBlit

    let lowerBlit (srcrect:SDL.Geometry.Rectangle option) (src:Surface) (dstrect:SDL.Geometry.Rectangle option) (dst:Surface) =
        SDL.Geometry.withSDLRectPointer (fun srcptr -> SDL.Geometry.withSDLRectPointer (fun dstptr -> 0 = Native.SDL_LowerBlit(src.Pointer,srcptr,dst.Pointer,dstptr)) dstrect) srcrect

    let upperBlitScaled (srcrect:SDL.Geometry.Rectangle option) (src:Surface) (dstrect:SDL.Geometry.Rectangle option) (dst:Surface) =
        SDL.Geometry.withSDLRectPointer (fun srcptr -> SDL.Geometry.withSDLRectPointer (fun dstptr -> 0 = Native.SDL_UpperBlitScaled(src.Pointer,srcptr,dst.Pointer,dstptr)) dstrect) srcrect

    let lowerBlitScaled (srcrect:SDL.Geometry.Rectangle option) (src:Surface) (dstrect:SDL.Geometry.Rectangle option) (dst:Surface) =
        SDL.Geometry.withSDLRectPointer (fun srcptr -> SDL.Geometry.withSDLRectPointer (fun dstptr -> 0 = Native.SDL_LowerBlitScaled(src.Pointer,srcptr,dst.Pointer,dstptr)) dstrect) srcrect

    let softStretch (srcrect:SDL.Geometry.Rectangle option) (src:Surface) (dstrect:SDL.Geometry.Rectangle option) (dst:Surface) =
        SDL.Geometry.withSDLRectPointer (fun srcptr -> SDL.Geometry.withSDLRectPointer (fun dstptr -> 0 = Native.SDL_SoftStretch(src.Pointer,srcptr,dst.Pointer,dstptr)) dstrect) srcrect

    let setColorKey (color:SDL.Pixel.Color option) (surface:Surface) =
        let fmt = 
            (surface |> getFormat)
        let key = 
            if color.IsSome then SDL.Pixel.mapColor fmt color.Value else 0u
        let flag = 
            if color.IsSome then 1 else 0
        0 = Native.SDL_SetColorKey(surface.Pointer, flag, key)

    let getColorKey (surface:Surface) :SDL.Pixel.Color option =
        let mutable key: uint32 = 0u
        match Native.SDL_GetColorKey(surface.Pointer,&&key) with
        | 0 ->
            let fmt = 
                (surface |> getFormat)
            key |> SDL.Pixel.getColor fmt |> Some
        | _ -> None

    let lockBind (surface:Surface) (func: unit -> unit) :bool =
        if 0 = Native.SDL_LockSurface surface.Pointer then
            func()
            Native.SDL_UnlockSurface surface.Pointer
            true
        else
            false

    let setRLE (surface:Surface) (flag:bool) :bool =
        0 = Native.SDL_SetSurfaceRLE(surface.Pointer,(if flag then 1 else 0))


    let setModulation (color:SDL.Pixel.Color) (surface:Surface) :bool = 
        (0 = Native.SDL_SetSurfaceColorMod(surface.Pointer, color.Red, color.Green, color.Blue)) && (0 = Native.SDL_SetSurfaceAlphaMod(surface.Pointer, color.Alpha))
    
    let getModulation (surface:Surface) :SDL.Pixel.Color option =
        let mutable r : uint8 = 0uy
        let mutable g : uint8 = 0uy
        let mutable b : uint8 = 0uy
        let mutable a : uint8 = 0uy
        let result = Native.SDL_GetSurfaceColorMod(surface.Pointer,&&r,&&g,&&b), Native.SDL_GetSurfaceAlphaMod(surface.Pointer,&&a)
        match result with
        | (0,0) -> {Red=r;Green=g;Blue=b;Alpha=a} |> Some
        | _ -> None
