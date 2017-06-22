#nowarn "9"

open System.Runtime.InteropServices
open System

[<StructLayout(LayoutKind.Explicit, Size=56)>]
type SDL_Event =
    struct
        [<FieldOffset(0)>]
        val Type: uint32
    end

module Native =
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint="SDL_Init")>]
    extern int SdlInit(uint32 flags)
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint="SDL_Quit")>]
    extern void SdlQuit()
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern int SDL_CreateWindowAndRenderer(int width, int height, uint32 window_flags, IntPtr* window, IntPtr* renderer)
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern void SDL_DestroyWindow(IntPtr window)
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern void SDL_DestroyRenderer(IntPtr renderer)
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern int SDL_RenderClear(IntPtr renderer)
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern void SDL_RenderPresent(IntPtr renderer)
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern int SDL_RenderDrawLine(IntPtr renderer, int x1, int y1, int x2, int y2)
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern int SDL_SetRenderDrawColor(IntPtr renderer, uint8 r, uint8 g, uint8 b, uint8 a)
    [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern int SDL_WaitEvent(SDL_Event* event)


let screenWidth = 640
let screenHeight = 360

let rec runLoop (random:Random) (renderer:IntPtr) : unit =
    Native.SDL_SetRenderDrawColor(renderer, random.Next(256) |> byte, random.Next(256) |> byte, random.Next(256) |> byte,255uy)
    |> ignore

    Native.SDL_RenderDrawLine(renderer,random.Next(screenWidth),random.Next(screenHeight),random.Next(screenWidth),random.Next(screenHeight))
    |> ignore

    Native.SDL_RenderPresent(renderer)

    let mutable event = SDL_Event()
    match Native.SDL_WaitEvent(&&event) with
    | 0 -> ()
    | _ ->
        if event.Type=0x100u then
            ()
        else
            runLoop random renderer

[<EntryPoint>]
let main argv = 
    match Native.SdlInit 0xFFFFu with
    | 0 -> 
        let mutable window:IntPtr = IntPtr.Zero
        let mutable renderer:IntPtr = IntPtr.Zero

        match Native.SDL_CreateWindowAndRenderer(screenWidth, screenHeight, 0u, &&window, &&renderer) with
        | 0 ->
            Native.SDL_SetRenderDrawColor(renderer,0uy,0uy,0uy,255uy)
            |> ignore

            Native.SDL_RenderClear(renderer)
            |> ignore

            runLoop (new Random()) renderer
                
            Native.SDL_DestroyRenderer(renderer)
            Native.SDL_DestroyWindow(window)
        | _ -> ()

        Native.SdlQuit()
    | _ -> 
        ()
    0
