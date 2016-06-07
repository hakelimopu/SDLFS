namespace SDL

#nowarn "9"

open System.Runtime.InteropServices
open System

module Log =

    let CategoryApplication =  0
    let CategoryError       =  1
    let CategoryAssert      =  2
    let CategorySystem      =  3
    let CategoryAudio       =  4
    let CategoryVideo       =  5
    let CategoryRender      =  6
    let CategoryInput       =  7
    let CategoryTest        =  8
    let CategoryReserved1   =  9
    let CategoryReserved2   = 10
    let CategoryReserved3   = 11
    let CategoryReserved4   = 12
    let CategoryReserved5   = 13
    let CategoryReserved6   = 14
    let CategoryReserved7   = 15
    let CategoryReserved8   = 16
    let CategoryReserved9   = 17
    let CategoryReserved10  = 18
    let CategoryCustom      = 19

    type Priority = 
        | Verbose  = 1
        | Debug    = 2
        | Info     = 3
        | Warn     = 4
        | Error    = 5
        | Critical = 6

    type OutputFunction = delegate of nativeint * nativeint * nativeint * nativeint -> unit

    module private Native =
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_Log([<MarshalAs(UnmanagedType.LPStr)>]string fmt)    
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern Priority SDL_LogGetPriority(int category);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_LogSetAllPriority(Priority priority);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_LogSetPriority(int category, Priority priority);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_LogResetPriorities();
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_LogVerbose(int category,  [<MarshalAs(UnmanagedType.LPStr)>]string fmt);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_LogDebug(int category,  [<MarshalAs(UnmanagedType.LPStr)>]string fmt);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_LogInfo(int category,  [<MarshalAs(UnmanagedType.LPStr)>]string fmt);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_LogWarn(int category,  [<MarshalAs(UnmanagedType.LPStr)>]string fmt);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_LogError(int category,  [<MarshalAs(UnmanagedType.LPStr)>]string fmt);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_LogCritical(int category,  [<MarshalAs(UnmanagedType.LPStr)>]string fmt);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_LogMessage(int category, Priority priority,  [<MarshalAs(UnmanagedType.LPStr)>]string fmt);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_LogGetOutputFunction(OutputFunction& callback, IntPtr* userdata);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_LogSetOutputFunction(OutputFunction callback, IntPtr userdata);

    let setOutputFunction (callback:OutputFunction) :unit =
        Native.SDL_LogSetOutputFunction(callback, IntPtr.Zero)

    let getOutputFunction () :OutputFunction =
        let mutable callback:OutputFunction = OutputFunction(fun a b c d -> ())
        let mutable ptr:IntPtr = IntPtr.Zero
        Native.SDL_LogGetOutputFunction(&callback,&&ptr)
        callback

    let log (message:string) :unit =
        Native.SDL_Log message

    let getPriority (category:int) :Priority =
        Native.SDL_LogGetPriority (category)

    let setAllPriority (priority:Priority) :unit =
        Native.SDL_LogSetAllPriority(priority)

    let setPriority (priority:Priority) (category:int) :unit =
        Native.SDL_LogSetPriority (category, priority)

    let resetPriorities () =
        Native.SDL_LogResetPriorities()

    //keep log functions from exploding!
    let private escapeLogString (message:string) : string =
        message.Replace("%","%%")

    let verbose (category:int) (message:string) :unit =
        Native.SDL_LogVerbose (category, message |> escapeLogString)

    let debug (category:int) (message:string) :unit =
        Native.SDL_LogDebug (category, message |> escapeLogString)

    let error (category:int) (message:string) :unit =
        Native.SDL_LogError (category, message |> escapeLogString)

    let info (category:int) (message:string) :unit =
        Native.SDL_LogInfo (category, message |> escapeLogString)

    let warn (category:int) (message:string) :unit =
        Native.SDL_LogWarn (category, message |> escapeLogString)

    let critical (category:int) (message:string) :unit =
        Native.SDL_LogCritical (category, message |> escapeLogString)

    let message (priority:Priority) (category:int) (message:string) :unit =
        Native.SDL_LogMessage (category, priority, message |> escapeLogString)

