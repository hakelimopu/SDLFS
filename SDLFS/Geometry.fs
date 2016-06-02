namespace SDL

#nowarn "9"

open System.Runtime.InteropServices
open SDL
open System
open Microsoft.FSharp.NativeInterop

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute>]
module Geometry = 
    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_Point =
        struct
            val mutable x: int
            val mutable y: int
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_Rect = 
        struct
            val mutable x :int
            val mutable y :int
            val mutable w :int
            val mutable h :int
        end

    module private Native =
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_HasIntersection(SDL_Rect* A, SDL_Rect* B)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_IntersectRect(SDL_Rect* A, SDL_Rect* B, SDL_Rect* result)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_UnionRect(SDL_Rect* A, SDL_Rect* B, SDL_Rect* result)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_EnclosePoints(SDL_Point* points, int count, SDL_Rect* clip, SDL_Rect* result)//TODO
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_IntersectRectAndLine(SDL_Rect* rect, int* X1, int* Y1, int* X2, int* Y2)//TODO

    type Point = {X: int<px>; Y: int<px>}

    type Rectangle = {X: int<px>; Y: int<px>; Width: int<px>; Height: int<px>}

    let internal rectangleToSDL_Rect (r:Rectangle) :SDL_Rect =
        let mutable result = new SDL_Rect()
        result.x <- r.X |> int
        result.y <- r.Y |> int
        result.w <- r.Width |> int
        result.h <- r.Height |> int
        result

    let internal withSDLRectPointer (func:IntPtr->'T) (rectangle:Rectangle option)=
        let mutable sdlrect = new SDL_Rect()
        let rectptr =
            if rectangle.IsNone then
                IntPtr.Zero
            else
                sdlrect <- (rectangle.Value |> rectangleToSDL_Rect)
                NativePtr.toNativeInt &&sdlrect
        func rectptr


    let internal pointToSDL_Point (p:Point) :SDL_Point =
        let mutable result = new SDL_Point()
        result.x <- p.X |> int
        result.y <- p.Y |> int
        result

    let internal sdl_RectToRectangle (r:SDL_Rect) :Rectangle =
        {X = r.x * 1<px>; Y=r.y * 1<px>; Width=r.w * 1<px>; Height=r.h * 1<px>}

    let internal sdl_PointToPoint (p:SDL_Point) :Point =
        {X = p.x * 1<px>; Y=p.y * 1<px>}

    let pointInRect (point: Point) (rectangle:Rectangle) :bool =
        point.X >= rectangle.X && point.Y >=rectangle.Y && point.X < (rectangle.X + rectangle.Width) && point.Y < (rectangle.Y + rectangle.Height)

    let isEmpty (rectangle:Rectangle option) :bool =
        match rectangle with
        | None -> true
        | Some r -> r.Width <= 0<px> || r.Height <=0<px>


    let equals (first:Rectangle option) (second:Rectangle option) : bool =
        match (first,second) with
        | (None, None) -> true
        | (Some r1, Some r2) -> r1.X=r2.X && r1.Y=r2.Y && r1.Width = r2.Width && r1.Height = r2.Height
        | _ -> false

    let hasIntersection (a:Rectangle) (b:Rectangle): bool =
        let mutable r1 = a |> rectangleToSDL_Rect
        let mutable r2 = b |> rectangleToSDL_Rect
        let mutable r3 = new SDL_Rect()
        0 <> Native.SDL_HasIntersection(&&r1,&&r2)

    let intersect (a:Rectangle) (b:Rectangle): Rectangle =
        let mutable r1 = a |> rectangleToSDL_Rect
        let mutable r2 = b |> rectangleToSDL_Rect
        let mutable r3 = new SDL_Rect()
        Native.SDL_IntersectRect(&&r1,&&r2,&&r3) |> ignore
        sdl_RectToRectangle(r3)

    let union (a:Rectangle) (b:Rectangle): Rectangle =
        let mutable r1 = a |> rectangleToSDL_Rect
        let mutable r2 = b |> rectangleToSDL_Rect
        let mutable r3 = new SDL_Rect()
        Native.SDL_UnionRect(&&r1,&&r2,&&r3) |> ignore
        sdl_RectToRectangle(r3)