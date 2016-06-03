namespace SDL

#nowarn "9"

open System.Runtime.InteropServices
open System
open SDL
open Microsoft.FSharp.NativeInterop

module Joystick =

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_JoystickGUID =
        struct
            [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.U1)>]
            val Data: byte array
        end

    type PowerLevel = 
        | Unknown = -1
        | Empty   =  0
        | Low     =  1
        | Medium  =  2
        | Full    =  3
        | Wired   =  4

    type Joystick = Utility.Pointer

    type EventState =
        | Ignore = 0
        | Enable = 1

    module private Native =
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_NumJoysticks();
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_JoystickNameForIndex(int device_index);

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_JoystickOpen(int device_index);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_JoystickClose(IntPtr  joystick);

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_JoystickFromInstanceID(int joyid);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_JoystickInstanceID(IntPtr  joystick);

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_JoystickName(IntPtr joystick);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_JoystickGetAttached(IntPtr  joystick);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_JoystickCurrentPowerLevel(IntPtr  joystick);

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern SDL_JoystickGUID SDL_JoystickGetDeviceGUID(int device_index);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern SDL_JoystickGUID SDL_JoystickGetGUID(IntPtr  joystick);
        //[<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        //extern void SDL_JoystickGetGUIDString(SDL_JoystickGUID guid, IntPtr pszGUID, int cbGUID);//don't need it
        //[<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        //extern SDL_JoystickGUID SDL_JoystickGetGUIDFromString(IntPtr pchGUID);//don't need it

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_JoystickEventState(int state);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_JoystickUpdate();

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_JoystickNumAxes(IntPtr  joystick);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int16 SDL_JoystickGetAxis(IntPtr  joystick, int axis)

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_JoystickNumHats(IntPtr  joystick);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern byte SDL_JoystickGetHat(IntPtr  joystick, int hat);

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_JoystickNumBalls(IntPtr  joystick);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_JoystickGetBall(IntPtr  joystick, int ball, int *dx, int *dy);

        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_JoystickNumButtons(IntPtr  joystick);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern byte SDL_JoystickGetButton(IntPtr  joystick, int button);


    let getCount = Native.SDL_NumJoysticks

    let getNameForIndex (index:int) : string =
        Native.SDL_JoystickNameForIndex(index)
        |> Utility.intPtrToStringAscii

    let create (index:int) : Joystick =
        new Joystick(Native.SDL_JoystickOpen(index),fun p->Native.SDL_JoystickClose(p))

    let createFromInstanceId (instanceId: int) : Joystick =
        new Joystick(Native.SDL_JoystickFromInstanceID(instanceId),fun p->Native.SDL_JoystickClose(p))

    let getInstanceId (joystick:Joystick) : int =
        Native.SDL_JoystickInstanceID(joystick.Pointer)

    let getName (joystick:Joystick) : string =
        Native.SDL_JoystickName(joystick.Pointer)
        |> Utility.intPtrToStringAscii

    let isAttached (joystick:Joystick): bool =
        Native.SDL_JoystickGetAttached(joystick.Pointer) <> 0

    let getGuidForIndex (index:int) : Guid =
        let result = Native.SDL_JoystickGetDeviceGUID(index)
        Guid(result.Data)

    let getGuid (joystick:Joystick) : Guid =
        let result = Native.SDL_JoystickGetGUID(joystick.Pointer)
        Guid(result.Data)

    let setEventState (eventState: EventState) : bool =
        if eventState = EventState.Enable then
            Native.SDL_JoystickEventState(1) = 1
        else
            Native.SDL_JoystickEventState(0) = 0

    let getEventState () : EventState =
        match Native.SDL_JoystickEventState(-1) with
        | 1 -> EventState.Enable
        | _ -> EventState.Ignore

    let update = Native.SDL_JoystickUpdate

    let getPowerLevel (joystick:Joystick) : PowerLevel =
        Native.SDL_JoystickCurrentPowerLevel(joystick.Pointer)
        |> LanguagePrimitives.EnumOfValue

    let getAxes (joystick:Joystick) : int =
        Native.SDL_JoystickNumAxes(joystick.Pointer)

    let getBalls (joystick:Joystick) : int =
        Native.SDL_JoystickNumBalls(joystick.Pointer)

    let getButtons (joystick:Joystick) : int =
        Native.SDL_JoystickNumButtons(joystick.Pointer)

    let getHats (joystick:Joystick) : int =
        Native.SDL_JoystickNumHats(joystick.Pointer)

    let getAxis (joystick:Joystick) (axis:int) : int16 =
        Native.SDL_JoystickGetAxis(joystick.Pointer, axis)

