namespace SDL

open System.Runtime.InteropServices
open System
open SDL
open Microsoft.FSharp.NativeInterop

#nowarn "9"

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute>]
module Pixel = 

    type PixelType =
        | Unknown=0
        | Index1=1
        | Index4=2
        | Index8=3
        | Packed8=4
        | Packed16=5
        | Packed32=6
        | ArrayU8=7
        | ArrayU16=8
        | ArrayU32=9
        | ArrayF16=10
        | ArrayF32=11

    type BitmapOrder =
        | None=0
        | _4321=1
        | _1234=2

    type PackedOrder =
        | None=0
        | XRGB=1
        | RGBX=2
        | ARGB=3
        | RGBA=4
        | XBGR=5
        | BGRX=6
        | ABGR=7
        | BGRA=8

    type ArrayOrder =
        | None=0
        | RGB=1
        | RGBA=2
        | ARGB=3
        | BGR=4
        | BGRA=5
        | ABGR=6

    type PackedLayout =
        | None=0
        | _332=1
        | _4444=2
        | _1555=3
        | _5551=4
        | _565=5
        | _8888=6
        | _2101010=7
        | _1010102=9

    let private DefinePixelFormat (typ, order, layout, bits, bytes) =
        ((1 <<< 28) ||| ((typ) <<< 24) ||| ((order) <<< 20) ||| ((layout) <<< 16) ||| ((bits) <<< 8) ||| ((bytes) <<< 0)) |> uint32


    let UnknownFormat     = 0
    let Index1LSBFormat   = DefinePixelFormat(PixelType.Index1   |> int, BitmapOrder._4321  |> int, 0                     |> int,  1, 0)
    let Index1MSBFormat   = DefinePixelFormat(PixelType.Index1   |> int, BitmapOrder._1234  |> int, 0                     |> int,  1, 0)
    let Index4LSBFormat   = DefinePixelFormat(PixelType.Index4   |> int, BitmapOrder._4321  |> int, 0                     |> int,  4, 0)
    let Index4MSBFormat   = DefinePixelFormat(PixelType.Index4   |> int, BitmapOrder._1234  |> int, 0                     |> int,  4, 0)
    let Index8Format      = DefinePixelFormat(PixelType.Index8   |> int, 0                  |> int, 0                     |> int,  8, 1)
    let RGB332Format      = DefinePixelFormat(PixelType.Packed8  |> int, PackedOrder.XRGB   |> int, PackedLayout._332     |> int,  8, 1)
    let RGB444Format      = DefinePixelFormat(PixelType.Packed16 |> int, PackedOrder.XRGB   |> int, PackedLayout._4444    |> int, 12, 2)
    let RGB555Format      = DefinePixelFormat(PixelType.Packed16 |> int, PackedOrder.XRGB   |> int, PackedLayout._1555    |> int, 15, 2)
    let BGR555Format      = DefinePixelFormat(PixelType.Packed16 |> int, PackedOrder.XBGR   |> int, PackedLayout._1555    |> int, 15, 2)
    let ARGB4444Format    = DefinePixelFormat(PixelType.Packed16 |> int, PackedOrder.ARGB   |> int, PackedLayout._4444    |> int, 16, 2)
    let RGBA4444Format    = DefinePixelFormat(PixelType.Packed16 |> int, PackedOrder.RGBA   |> int, PackedLayout._4444    |> int, 16, 2)
    let ABGR4444Format    = DefinePixelFormat(PixelType.Packed16 |> int, PackedOrder.ABGR   |> int, PackedLayout._4444    |> int, 16, 2)
    let BGRA4444Format    = DefinePixelFormat(PixelType.Packed16 |> int, PackedOrder.BGRA   |> int, PackedLayout._4444    |> int, 16, 2)
    let ARGB1555Format    = DefinePixelFormat(PixelType.Packed16 |> int, PackedOrder.ARGB   |> int, PackedLayout._1555    |> int, 16, 2)
    let RGBA5551Format    = DefinePixelFormat(PixelType.Packed16 |> int, PackedOrder.RGBA   |> int, PackedLayout._5551    |> int, 16, 2)
    let ABGR1555Format    = DefinePixelFormat(PixelType.Packed16 |> int, PackedOrder.ABGR   |> int, PackedLayout._1555    |> int, 16, 2)
    let BGRA5551Format    = DefinePixelFormat(PixelType.Packed16 |> int, PackedOrder.BGRA   |> int, PackedLayout._5551    |> int, 16, 2)
    let RGB565Format      = DefinePixelFormat(PixelType.Packed16 |> int, PackedOrder.XRGB   |> int, PackedLayout._565     |> int, 16, 2)
    let BGR565Format      = DefinePixelFormat(PixelType.Packed16 |> int, PackedOrder.XBGR   |> int, PackedLayout._565     |> int, 16, 2)
    let RGB24Format       = DefinePixelFormat(PixelType.ArrayU8  |> int, ArrayOrder.RGB     |> int, 0                     |> int, 24, 3)
    let BGR24Format       = DefinePixelFormat(PixelType.ArrayU8  |> int, ArrayOrder.BGR     |> int, 0                     |> int, 24, 3)
    let RGB888Format      = DefinePixelFormat(PixelType.Packed32 |> int, PackedOrder.XRGB   |> int, PackedLayout._8888    |> int, 24, 4)
    let RGBX8888Format    = DefinePixelFormat(PixelType.Packed32 |> int, PackedOrder.RGBX   |> int, PackedLayout._8888    |> int, 24, 4)
    let BGR888Format      = DefinePixelFormat(PixelType.Packed32 |> int, PackedOrder.XBGR   |> int, PackedLayout._8888    |> int, 24, 4)
    let BGRX8888Format    = DefinePixelFormat(PixelType.Packed32 |> int, PackedOrder.BGRX   |> int, PackedLayout._8888    |> int, 24, 4)
    let ARGB8888Format    = DefinePixelFormat(PixelType.Packed32 |> int, PackedOrder.ARGB   |> int, PackedLayout._8888    |> int, 32, 4)
    let RGBA8888Format    = DefinePixelFormat(PixelType.Packed32 |> int, PackedOrder.RGBA   |> int, PackedLayout._8888    |> int, 32, 4)
    let ABGR8888Format    = DefinePixelFormat(PixelType.Packed32 |> int, PackedOrder.ABGR   |> int, PackedLayout._8888    |> int, 32, 4)
    let BGRA8888Format    = DefinePixelFormat(PixelType.Packed32 |> int, PackedOrder.BGRA   |> int, PackedLayout._8888    |> int, 32, 4)
    let ARGB2101010Format = DefinePixelFormat(PixelType.Packed32 |> int, PackedOrder.ARGB   |> int, PackedLayout._2101010 |> int, 32, 4)

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_Color =
        struct
            val mutable r: uint8
            val mutable g: uint8
            val mutable b: uint8
            val mutable a: uint8
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_Palette =
        struct
            val ncolors: int
            val colors: IntPtr
            val version: uint32
            val refcount: int
        end


    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_PixelFormat =
        struct
            val format: uint32
            val palette: IntPtr//SDL_Palette*
            val BitsPerPixel: uint8
            val BytesPerPixel: uint8
            val padding: uint16
            val Rmask: uint32
            val Gmask: uint32
            val Bmask: uint32
            val Amask: uint32
            val Rloss: uint8
            val Gloss: uint8
            val Bloss: uint8
            val Aloss: uint8
            val Rshift: uint8
            val Gshift: uint8
            val Bshift: uint8
            val Ashift: uint8
            val refcount: int
            val next: IntPtr;//SDL_PixelFormat*
        end

    module private Native =
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_GetPixelFormatName(uint32 formatEnum)

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_PixelFormatEnumToMasks(uint32 formatEnum,int* bpp,uint32* Rmask,uint32* Gmask,uint32* Bmask,uint32* Amask)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern uint32 SDL_MasksToPixelFormatEnum(int bpp,uint32 Rmask,uint32 Gmask,uint32 Bmask,uint32 Amask)

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_AllocFormat(uint32 formatEnum)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_FreeFormat(IntPtr format)

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_AllocPalette(int ncolors)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_FreePalette(IntPtr palette)

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetPixelFormatPalette(IntPtr format,IntPtr palette)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetPaletteColors(IntPtr palette,SDL_Color* colors,int firstcolor, int ncolors)

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern uint32 SDL_MapRGB(IntPtr format,uint8 r, uint8 g, uint8 b)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern uint32 SDL_MapRGBA(IntPtr format,uint8 r, uint8 g, uint8 b,uint8 a)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]

        extern void SDL_GetRGB(uint32 pixel,IntPtr format,uint8* r, uint8* g, uint8* b)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_GetRGBA(uint32 pixel,IntPtr format,uint8* r, uint8* g, uint8* b,uint8* a)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_CalculateGammaRamp(float gamma, IntPtr ramp)

    type Color =
        {Red: uint8;Green: uint8;Blue :uint8; Alpha: uint8}

    type Palette = IntPtr

    type PixelFormat = IntPtr

    type PixelFormatInfo =
        {Format: uint32;
        Palette: Palette;
        BitsPerPixel: int;
        BytesPerPixel: int;
        RMask: uint32;
        GMask: uint32;
        BMask: uint32;
        AMask: uint32}

    let formatEnumName (format:uint32) :string = 
        Native.SDL_GetPixelFormatName(format)
        |> SDL.Utility.intPtrToStringUtf8

    let formatEnumToMasks (formatEnum: uint32) : (int*uint32*uint32*uint32*uint32) option =
        let mutable bpp=0
        let mutable rmask=0u
        let mutable gmask=0u
        let mutable bmask=0u
        let mutable amask=0u
        if Native.SDL_PixelFormatEnumToMasks(formatEnum,&&bpp,&&rmask,&&gmask,&&bmask,&&amask) <> 0 then
            Some (bpp*1,rmask,gmask,bmask,amask)
        else
            None

    let masksToFormatEnum (bpp:int,rmask:uint32,gmask:uint32,bmask:uint32,amask:uint32) :uint32 =
        Native.SDL_MasksToPixelFormatEnum(bpp/1,rmask,gmask,bmask,amask)

    let alloc (formatEnum: uint32) :PixelFormat =
        Native.SDL_AllocFormat formatEnum

    let free format =
        Native.SDL_FreeFormat format

    let allocPalette colorCount =
        Native.SDL_AllocPalette(colorCount)

    let freePalette palette =
        Native.SDL_FreePalette(palette)

    let setPalette palette format =
        0 = Native.SDL_SetPixelFormatPalette(format,palette)

    let setPaletteColor index (color:Color) palette =
        let mutable c = new SDL_Color()
        c.r <- color.Red
        c.g <- color.Green
        c.b <- color.Blue
        c.a <- color.Alpha
        0 = Native.SDL_SetPaletteColors(palette,&&c,index,1)

    let getPaletteColorCount (palette:IntPtr) : int=
        if palette=IntPtr.Zero then
            0
        else
            let pal = 
                palette
                |> NativePtr.ofNativeInt<SDL_Palette>
                |> NativePtr.read
            pal.ncolors

    let getPaletteColor (index:int) palette :Color option=
        if palette = IntPtr.Zero || index<0 || index>= (palette |> getPaletteColorCount) then
            None
        else
            let pal = 
                palette
                |> NativePtr.ofNativeInt<SDL_Palette>
                |> NativePtr.read
            let colors :nativeptr<SDL_Color> =
                pal.colors
                |> NativePtr.ofNativeInt<SDL_Color>
            let color = 
                NativePtr.add colors index
                |> NativePtr.read
            Some {Red=color.r;Green=color.g;Blue=color.b;Alpha=color.a}
    
    let mapColor (format:PixelFormat) (color: Color) :uint32 = 
        Native.SDL_MapRGBA(format,color.Red,color.Green,color.Blue,color.Alpha)

    let getColor (format:PixelFormat) (value:uint32) :Color =
        let mutable r=0uy
        let mutable g=0uy
        let mutable b=0uy
        let mutable a=0uy
        Native.SDL_GetRGBA(value,format,&&r,&&g,&&b,&&a)
        {Red=r;Green=g;Blue=b;Alpha=a}