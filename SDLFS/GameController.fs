namespace SDL

#nowarn "9"

open System.Runtime.InteropServices
open System
open SDL
open Microsoft.FSharp.NativeInterop

module GameController =

    type BindType =
        | None   = 0
        | Button = 1
        | Axis   = 2
        | Hat    = 3

    [<StructLayout(LayoutKind.Explicit, Size=12)>]
    type internal SDL_GameControllerButtonBind =
        struct
            [<FieldOffset(0)>]
            val BindType: int
            [<FieldOffset(4)>]
            val Axis: int
            [<FieldOffset(4)>]
            val Hat: int
            [<FieldOffset(8)>]
            val HatMask: int
        end

    type Axis =
        | Invalid      = -1
        | LeftX        =  0
        | LeftY        =  1
        | RightX       =  2
        | RightY       =  3
        | TriggerLeft  =  4
        | TriggerRight =  5

    type Button =
        | Invalid       = -1
        | A             =  0
        | B             =  1
        | X             =  2
        | Y             =  3
        | Back          =  4
        | Guide         =  5
        | Start         =  6
        | LeftStick     =  7
        | RightStick    =  8
        | LeftShoulder  =  9
        | RightShoulder = 10 
        | DPadUp        = 11
        | DPadDown      = 12
        | DPadLeft      = 13
        | DPadRight     = 14

    type Controller = SDL.Utility.Pointer

    module private Native =
        //belongs elsewhere... in joystick
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_NumJoysticks()//TODO: does this belong here?

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_GameControllerOpen(int joystick_index)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_GameControllerClose(IntPtr gamecontroller)

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GameControllerAddMappingsFromRW(IntPtr rw, int freerw )
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GameControllerAddMapping(IntPtr mappingString )
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_GameControllerMapping( IntPtr gamecontroller )

        //extern IntPtr SDL_GameControllerMappingForGUID( SDL_JoystickGUID guid )

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_IsGameController(int joystick_index)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_GameControllerNameForIndex(int joystick_index)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_GameControllerName(IntPtr gamecontroller)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GameControllerGetAttached(IntPtr gamecontroller)

        //extern IntPtr SDL_GameControllerFromInstanceID(SDL_JoystickID joyid)
        //extern SDL_Joystick *SDL_GameControllerGetJoystick(SDL_GameController *gamecontroller)

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GameControllerEventState(int state)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_GameControllerUpdate()

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GameControllerGetAxisFromString(IntPtr pchString)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_GameControllerGetStringForAxis(int axis)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int16 SDL_GameControllerGetAxis(IntPtr gamecontroller, int axis)

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GameControllerGetButtonFromString(IntPtr pchString)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_GameControllerGetStringForButton(int button)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern uint8 SDL_GameControllerGetButton(IntPtr gamecontroller,int button)

        //extern SDL_GameControllerButtonBind SDL_GameControllerGetBindForAxis(SDL_GameController *gamecontroller, SDL_GameControllerAxis axis)
        //extern SDL_GameControllerButtonBind SDL_GameControllerGetBindForButton(SDL_GameController *gamecontroller, SDL_GameControllerButton button)

    let create (index: int)  : Controller =
        let n = Native.SDL_NumJoysticks()//TODO: why is this here?
        let ptr = Native.SDL_GameControllerOpen(index) 
        
        new Controller(ptr, fun p -> Native.SDL_GameControllerClose(p))
