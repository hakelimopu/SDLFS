namespace SDL

open System
open System.Runtime.InteropServices
open SDL
open Microsoft.FSharp.NativeInterop

#nowarn "9"

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute>]
module Hints = 

    let SDL_HINT_FRAMEBUFFER_ACCELERATION                 = "SDL_FRAMEBUFFER_ACCELERATION"
    let SDL_HINT_RENDER_DRIVER                            = "SDL_RENDER_DRIVER"
    let SDL_HINT_RENDER_OPENGL_SHADERS                    = "SDL_RENDER_OPENGL_SHADERS"
    let SDL_HINT_RENDER_DIRECT3D_THREADSAFE               = "SDL_RENDER_DIRECT3D_THREADSAFE"
    let SDL_HINT_RENDER_DIRECT3D11_DEBUG                  = "SDL_RENDER_DIRECT3D11_DEBUG"
    let SDL_HINT_RENDER_SCALE_QUALITY                     = "SDL_RENDER_SCALE_QUALITY"
    let SDL_HINT_RENDER_VSYNC                             = "SDL_RENDER_VSYNC"
    let SDL_HINT_VIDEO_ALLOW_SCREENSAVER                  = "SDL_VIDEO_ALLOW_SCREENSAVER"
    let SDL_HINT_VIDEO_X11_XVIDMODE                       = "SDL_VIDEO_X11_XVIDMODE"
    let SDL_HINT_VIDEO_X11_XINERAMA                       = "SDL_VIDEO_X11_XINERAMA"
    let SDL_HINT_VIDEO_X11_XRANDR                         = "SDL_VIDEO_X11_XRANDR"
    let SDL_HINT_VIDEO_X11_NET_WM_PING                    = "SDL_VIDEO_X11_NET_WM_PING"
    let SDL_HINT_WINDOW_FRAME_USABLE_WHILE_CURSOR_HIDDEN  = "SDL_WINDOW_FRAME_USABLE_WHILE_CURSOR_HIDDEN"
    let SDL_HINT_WINDOWS_ENABLE_MESSAGELOOP               = "SDL_WINDOWS_ENABLE_MESSAGELOOP"
    let SDL_HINT_GRAB_KEYBOARD                            = "SDL_GRAB_KEYBOARD"
    let SDL_HINT_MOUSE_RELATIVE_MODE_WARP                 = "SDL_MOUSE_RELATIVE_MODE_WARP"
    let SDL_HINT_VIDEO_MINIMIZE_ON_FOCUS_LOSS             = "SDL_VIDEO_MINIMIZE_ON_FOCUS_LOSS"
    let SDL_HINT_IDLE_TIMER_DISABLED                      = "SDL_IOS_IDLE_TIMER_DISABLED"
    let SDL_HINT_ORIENTATIONS                             = "SDL_IOS_ORIENTATIONS"
    let SDL_HINT_ACCELEROMETER_AS_JOYSTICK                = "SDL_ACCELEROMETER_AS_JOYSTICK"
    let SDL_HINT_XINPUT_ENABLED                           = "SDL_XINPUT_ENABLED"
    let SDL_HINT_XINPUT_USE_OLD_JOYSTICK_MAPPING          = "SDL_XINPUT_USE_OLD_JOYSTICK_MAPPING"
    let SDL_HINT_GAMECONTROLLERCONFIG                     = "SDL_GAMECONTROLLERCONFIG"
    let SDL_HINT_JOYSTICK_ALLOW_BACKGROUND_EVENTS         = "SDL_JOYSTICK_ALLOW_BACKGROUND_EVENTS"
    let SDL_HINT_ALLOW_TOPMOST                            = "SDL_ALLOW_TOPMOST"
    let SDL_HINT_TIMER_RESOLUTION                         = "SDL_TIMER_RESOLUTION"
    let SDL_HINT_THREAD_STACK_SIZE                        = "SDL_THREAD_STACK_SIZE"
    let SDL_HINT_VIDEO_HIGHDPI_DISABLED                   = "SDL_VIDEO_HIGHDPI_DISABLED"
    let SDL_HINT_MAC_CTRL_CLICK_EMULATE_RIGHT_CLICK       = "SDL_MAC_CTRL_CLICK_EMULATE_RIGHT_CLICK"
    let SDL_HINT_VIDEO_WIN_D3DCOMPILER                    = "SDL_VIDEO_WIN_D3DCOMPILER"
    let SDL_HINT_VIDEO_WINDOW_SHARE_PIXEL_FORMAT          = "SDL_VIDEO_WINDOW_SHARE_PIXEL_FORMAT"
    let SDL_HINT_WINRT_PRIVACY_POLICY_URL                 = "SDL_WINRT_PRIVACY_POLICY_URL"
    let SDL_HINT_WINRT_PRIVACY_POLICY_LABEL               = "SDL_WINRT_PRIVACY_POLICY_LABEL"
    let SDL_HINT_WINRT_HANDLE_BACK_BUTTON                 = "SDL_WINRT_HANDLE_BACK_BUTTON"
    let SDL_HINT_VIDEO_MAC_FULLSCREEN_SPACES              = "SDL_VIDEO_MAC_FULLSCREEN_SPACES"
    let SDL_HINT_MAC_BACKGROUND_APP                       = "SDL_MAC_BACKGROUND_APP"
    let SDL_HINT_ANDROID_APK_EXPANSION_MAIN_FILE_VERSION  = "SDL_ANDROID_APK_EXPANSION_MAIN_FILE_VERSION"
    let SDL_HINT_ANDROID_APK_EXPANSION_PATCH_FILE_VERSION = "SDL_ANDROID_APK_EXPANSION_PATCH_FILE_VERSION"
    let SDL_HINT_IME_INTERNAL_EDITING                     = "SDL_IME_INTERNAL_EDITING"
    let SDL_HINT_ANDROID_SEPARATE_MOUSE_AND_TOUCH         = "SDL_ANDROID_SEPARATE_MOUSE_AND_TOUCH"
    let SDL_HINT_EMSCRIPTEN_KEYBOARD_ELEMENT              = "SDL_EMSCRIPTEN_KEYBOARD_ELEMENT"
    let SDL_HINT_NO_SIGNAL_HANDLERS                       = "SDL_NO_SIGNAL_HANDLERS"
    let SDL_HINT_WINDOWS_NO_CLOSE_ON_ALT_F4               = "SDL_WINDOWS_NO_CLOSE_ON_ALT_F4"

    type Priority =
        | Default  = 0
        | Normal   = 1
        | Override = 2

    type private SDL_HintCallback = IntPtr * IntPtr * IntPtr * IntPtr -> unit

    module private Native =
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetHintWithPriority(IntPtr name, IntPtr value, Priority priority);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_SetHint(IntPtr name, IntPtr value);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_GetHint(IntPtr name);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_AddHintCallback(IntPtr name, SDL_HintCallback callback, IntPtr userdata);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_DelHintCallback(IntPtr name, SDL_HintCallback callback, IntPtr userdata);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_ClearHints()

    let set (name:string) (value:string) :bool =
        let valuePtrHandler (namePtr:IntPtr) (valuePtr:IntPtr) =
            Native.SDL_SetHint(namePtr,valuePtr)
            <> 0

        let namePtrHandler (value:string) (namePtr:IntPtr) =
            value
            |> SDL.Utility.withAsciiString (valuePtrHandler namePtr)

        name 
        |> SDL.Utility.withAsciiString (namePtrHandler value)


    let setWithPriority (name:string) (value:string) (priority:Priority):bool =
        let valuePtrHandler (namePtr:IntPtr) (valuePtr:IntPtr) =
            Native.SDL_SetHintWithPriority(namePtr, valuePtr, priority)
            <> 0

        let namePtrHandler (value:string) (namePtr:IntPtr) =
            value
            |> SDL.Utility.withAsciiString (valuePtrHandler namePtr)

        name 
        |> SDL.Utility.withAsciiString (namePtrHandler value)

    let get (name:string) :string =
        let namePtrHandler (namePtr:IntPtr) =
            Native.SDL_GetHint(namePtr)
            |> SDL.Utility.intPtrToStringAscii

        name 
        |> SDL.Utility.withAsciiString namePtrHandler

    let clear () =
        Native.SDL_ClearHints()

    let addCallback (name:string) (callback:string->string->string->unit) : IntPtr * IntPtr * IntPtr * IntPtr -> unit=
        let wrappedCallback (callback:string->string->string->unit) (userDataPtr:IntPtr, namePtr:IntPtr, oldValuePtr:IntPtr, newValuePtr:IntPtr) : unit =
            let name =
                namePtr
                |> SDL.Utility.intPtrToStringAscii

            let oldValue = 
                oldValuePtr
                |> SDL.Utility.intPtrToStringAscii

            let newValue = 
                newValuePtr
                |> SDL.Utility.intPtrToStringAscii
        
            callback name oldValue newValue

        let namePtrHandler (namePtr:IntPtr) =
            let result = wrappedCallback callback
            Native.SDL_AddHintCallback(namePtr, result, IntPtr.Zero)
            result

        name 
        |> SDL.Utility.withAsciiString namePtrHandler

    let removeCallback (name:string) (callback:IntPtr * IntPtr * IntPtr * IntPtr -> unit) :unit =
        let namePtrHandler (namePtr:IntPtr) =
            Native.SDL_DelHintCallback(namePtr,callback,IntPtr.Zero)

        name 
        |> SDL.Utility.withAsciiString namePtrHandler
    
