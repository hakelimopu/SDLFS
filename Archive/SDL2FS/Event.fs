namespace SDL

#nowarn "9"

open System.Runtime.InteropServices
open System
open SDL

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute>]
module Event =

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_QuitEvent =
        struct
            val Type: uint32
            val Timestamp: uint32
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_Keysym = 
        struct
            val Scancode: int32
            val Sym: int32
            val Mod: uint16
            val Unused: uint32
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_CommonEvent =
        struct
            val Type: uint32
            val Timestamp: uint32
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_WindowEvent =
        struct
            val Type: uint32
            val Timestamp: uint32
            val WindowID: uint32
            val Event: uint8     
            val Padding1: uint8
            val Padding2: uint8
            val Padding3: uint8
            val Data1: int32  
            val Data2: int32   
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_KeyboardEvent =
        struct
            val Type: uint32
            val Timestamp: uint32
            val WindowID: uint32
            val State: uint8
            val Repeat: uint8
            val Padding2: uint8
            val Padding3: uint8
            val Keysym: SDL_Keysym
        end

    [<StructLayout(LayoutKind.Explicit, Size=52)>]
    type internal SDL_TextEditingEvent =
        struct
            [<FieldOffset(0)>]
            val Type: uint32
            [<FieldOffset(4)>]
            val Timestamp: uint32
            [<FieldOffset(8)>]
            val WindowID: uint32                            
            [<FieldOffset(12)>]
            val Text: byte//really a byte[32]
            [<FieldOffset(44)>]
            val Start: int32                               
            [<FieldOffset(48)>]
            val Length: int32                              
        end

    [<StructLayout(LayoutKind.Explicit, Size=44)>]
    type internal SDL_TextInputEvent =
        struct
            [<FieldOffset(0)>]
            val Type: uint32
            [<FieldOffset(4)>]
            val Timestamp: uint32
            [<FieldOffset(8)>]
            val WindowID: uint32                            
            [<FieldOffset(12)>]
            val Text: byte//really a byte[32]
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_MouseMotionEvent =
        struct
            val Type: uint32
            val Timestamp: uint32
            val WindowID: uint32
            val Which: uint32
            val State: uint32
            val X: int32
            val Y: int32
            val Xrel: int32
            val Yrel: int32
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_MouseButtonEvent =
        struct
            val Type: uint32
            val Timestamp: uint32
            val WindowID: uint32
            val Which: uint32
            val Button: uint8
            val State: uint8
            val Clicks: uint8
            val Padding1: uint8
            val X: int32
            val Y: int32
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_MouseWheelEvent=
        struct
            val Type:uint32
            val Timestamp:uint32
            val WindowID:uint32
            val Which:uint32
            val X:int32
            val Y:int32
            val Direction:uint32
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_JoyAxisEvent=
        struct
            val Type:uint32
            val Timestamp:uint32
            val Which:int32
            val Axis:uint8
            val Padding1:uint8
            val Padding2:uint8
            val Padding3:uint8
            val Value:int16
            val Padding4:uint16
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_JoyBallEvent=
        struct
            val Type:uint32
            val Timestamp:uint32
            val Which:int32
            val Ball:uint8
            val Padding1:uint8
            val Padding2:uint8
            val Padding3:uint8
            val Xrel:int16
            val Yrel:int16
        end 

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_JoyHatEvent=
        struct
            val Type:uint32
            val Timestamp:uint32
            val Which:int32
            val Hat:uint8
            val Value:uint8
            val Padding1:uint8
            val Padding2:uint8
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_JoyButtonEvent=
        struct
            val Type:uint32
            val Timestamp:uint32
            val Which:int32
            val Button:uint8
            val State:uint8
            val Padding1:uint8
            val Padding2:uint8
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_JoyDeviceEvent=
        struct
            val Type:uint32
            val Timestamp:uint32
            val Which:int32
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_ControllerAxisEvent=
        struct
            val Type:uint32
            val Timestamp:uint32
            val Which:int32
            val Axis:uint8
            val Padding1:uint8
            val Padding2:uint8
            val Padding3:uint8
            val Value:int16
            val Padding4:uint16
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_ControllerButtonEvent=
        struct
            val Type:uint32
            val Timestamp:uint32
            val Which:int32
            val Button:uint8
            val State:uint8
            val Padding1:uint8
            val Padding2:uint8
        end 

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_ControllerDeviceEvent=
        struct
            val Type:uint32
            val Timestamp:uint32
            val Which:int32
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_AudioDeviceEvent=
        struct
            val Type:uint32
            val Timestamp:uint32
            val Which:uint32
            val Iscapture:uint8
            val Padding1:uint8
            val Padding2:uint8
            val Padding3:uint8
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_TouchFingerEvent=
        struct
            val Type:uint32
            val Timestamp:uint32
            val TouchId:int64
            val FingerId:int64
            val X:float
            val Y:float
            val Dx:float
            val Dy:float
            val Pressure:float
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_MultiGestureEvent=
        struct
            val Type:uint32
            val Timestamp:uint32
            val TouchId:int64
            val DTheta:float
            val DDist:float
            val X:float
            val Y:float
            val NumFingers:uint16
            val Padding:uint16
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_DollarGestureEvent=
        struct
            val Type:uint32
            val Timestamp:uint32
            val TouchId:int64
            val GestureId:int64
            val NumFingers:uint32
            val Error:float
            val X:float
            val Y:float
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_DropEvent=
        struct
            val Type:uint32
            val Timestamp:uint32
            val File:IntPtr
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_OSEvent=
        struct
            val Type:uint32
            val Timestamp:uint32
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_UserEvent=
        struct
            val Type:uint32
            val Timestamp:uint32
            val WindowID:uint32
            val Code:int32
            val Data1:IntPtr
            val Data2:IntPtr
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_SysWMEvent=
        struct
            val Type:uint32
            val Timestamp:uint32
            val Msg:IntPtr
        end
    
    [<StructLayout(LayoutKind.Explicit, Size=56)>]
    type internal SDL_Event =
        struct
            [<FieldOffset(0)>]
            val Type: uint32
            [<FieldOffset(0)>]
            val Common:SDL_CommonEvent
            [<FieldOffset(0)>]
            val Window:SDL_WindowEvent
            [<FieldOffset(0)>]
            val Key:SDL_KeyboardEvent
            [<FieldOffset(0)>]
            val Edit:SDL_TextEditingEvent
            [<FieldOffset(0)>]
            val Text:SDL_TextInputEvent
            [<FieldOffset(0)>]
            val Motion:SDL_MouseMotionEvent
            [<FieldOffset(0)>]
            val Button:SDL_MouseButtonEvent
            [<FieldOffset(0)>]
            val Wheel:SDL_MouseWheelEvent
            [<FieldOffset(0)>]
            val Jaxis:SDL_JoyAxisEvent
            [<FieldOffset(0)>]
            val Jball:SDL_JoyBallEvent
            [<FieldOffset(0)>]
            val Jhat:SDL_JoyHatEvent
            [<FieldOffset(0)>]
            val Jbutton:SDL_JoyButtonEvent
            [<FieldOffset(0)>]
            val Jdevice:SDL_JoyDeviceEvent
            [<FieldOffset(0)>]
            val Caxis:SDL_ControllerAxisEvent
            [<FieldOffset(0)>]
            val Cbutton:SDL_ControllerButtonEvent
            [<FieldOffset(0)>]
            val Cdevice:SDL_ControllerDeviceEvent
            [<FieldOffset(0)>]
            val Adevice:SDL_AudioDeviceEvent
            [<FieldOffset(0)>]
            val Quit:SDL_QuitEvent
            [<FieldOffset(0)>]
            val User:SDL_UserEvent
            [<FieldOffset(0)>]
            val Syswm:SDL_SysWMEvent
            [<FieldOffset(0)>]
            val Tfinger:SDL_TouchFingerEvent
            [<FieldOffset(0)>]
            val Mgesture:SDL_MultiGestureEvent
            [<FieldOffset(0)>]
            val Dgesture:SDL_DollarGestureEvent
            [<FieldOffset(0)>]
            val Drop:SDL_DropEvent
        end

    type EventType =
        | Quit                     = 0x100
        | AppTerminating           = 0x101
        | AppLowmemory             = 0x102
        | AppWillEnterBackground   = 0x103
        | AppDidEnterBackground    = 0x104
        | AppWillEnterForeground   = 0x105
        | AppDidEnterForeground    = 0x106
        | WindowEvent              = 0x200
        | SysWMEvent               = 0x201
        | KeyDown                  = 0x300
        | KeyUp                    = 0x301
        | TextEditing              = 0x302
        | TextInput                = 0x303
        | KeyMapChanged            = 0x304
        | MouseMotion              = 0x400
        | MouseButtonDown          = 0x401
        | MouseButtonUp            = 0x402
        | MouseWheel               = 0x403
        | JoyAxisMotion            = 0x600
        | JoyBallMotion            = 0x601
        | JoyHatMotion             = 0x602
        | JoyButtonDown            = 0x603
        | JoyButtonUp              = 0x604
        | JoyDeviceAdded           = 0x605
        | JoyDeviceRemoved         = 0x606
        | ControllerAxisMotion     = 0x650 
        | ControllerButtonDown     = 0x651   
        | ControllerButtonUp       = 0x652
        | ControllerDeviceAdded    = 0x653
        | ControllerDeviceRemoved  = 0x654
        | ControllerDeviceRemapped = 0x655
        | FingerDown               = 0x700
        | FingerUp                 = 0x701
        | FingerMotion             = 0x702
        | DollarGesture            = 0x800
        | DollarRecord             = 0x801
        | MultiGesture             = 0x802
        | ClipboardUpdate          = 0x900 
        | DropFile                 = 0x1000
        | AudioDeviceAdded         = 0x1100
        | AudioDeviceRemoved       = 0x1101      
        | RenderTargetsReset       = 0x2000
        | RenderDeviceReset        = 0x2001
        | UserEvent                = 0x8000
        | LastEvent                = 0xFFFF

    type EventAction =
        | AddEvent=0
        | PeekEvent=1
        | GetEvent=2

    type EventState =
        | Query  = -1
        | Ignore = 0
        | Disable= 0
        | Enable = 1

    type internal SDL_EventFilter = delegate of nativeint * nativeptr<SDL_Event> -> int

    module private Native =
        //pump
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_PumpEvents()

        //wait
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_WaitEvent(SDL_Event* event);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_WaitEventTimeout(SDL_Event* event,int timeout)

        //poll
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_PollEvent(SDL_Event* event);

        //peep
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_PeepEvents(SDL_Event* events, int numevents,int action,uint32 minType, uint32 maxType)

        //pushing
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_PushEvent(SDL_Event* event)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern uint32 SDL_RegisterEvents(int numevents)

        //query
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_HasEvent(uint32 eventType)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_HasEvents(uint32 minType, uint32 maxType)

        //flush
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_FlushEvent(uint32 eventType)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_FlushEvents(uint32 minType, uint32 maxType)

        //filter - dunno how I'll be able to do these in F#.
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_SetEventFilter(SDL_EventFilter filter,nativeint userdata)//how to pass NULL?
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetEventFilter(SDL_EventFilter& filter,nativeint* userdata)//cant use SDL_EventFilter*, which may make this unusable...
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_FilterEvents(SDL_EventFilter filter,nativeint userdata)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern uint8 SDL_EventState(uint32 eventType, int state)

        //watch - dunno how I'll be able to do these in F#.
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_AddEventWatch(SDL_EventFilter filter,nativeint userdata)
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_DelEventWatch(SDL_EventFilter filter,nativeint userdata)


    type QuitEvent =
        {Timestamp:uint32}

    type Keysym =
        {Scancode: SDL.Keyboard.ScanCode;
        Sym: int32;
        Mod: uint16}

    type KeyboardEvent =
        {Timestamp: uint32;
        WindowID: uint32;
        State: uint8;
        Repeat: uint8;
        Keysym: Keysym}

    type MouseMotionEvent =
        {Timestamp: uint32;
        WindowID: uint32;
        Which: uint32;
        State: uint32;
        X: int32;
        Y: int32;
        Xrel: int32;
        Yrel: int32}

    type MouseButtonEvent =
        {Timestamp: uint32;
        WindowID: uint32;
        Which: uint32;
        Button: uint8;
        State: uint8;
        Clicks: uint8;
        X: int32;
        Y: int32}

    type WindowEvent =
        {Type: uint32;
        Timestamp: uint32;
        WindowID: uint32;
        Event: uint8;
        Data1: int32; 
        Data2: int32}

    type TextEditingEvent =
        {Timestamp: uint32;
        WindowID: uint32;                          
        Text: string;
        Start: int32;                              
        Length: int32}

    type TextInputEvent =
        {Timestamp: uint32;
        WindowID: uint32;                            
        Text:string}

    type MouseWheelEvent=
        {Timestamp:uint32;
        WindowID:uint32;
        Which:uint32;
        X:int32;
        Y:int32;
        Direction:SDL.Mouse.WheelDirection}

    type JoyAxisEvent=
        {Timestamp:uint32;
        Which:int32;
        Axis:uint8;
        Value:int16}

    type JoyBallEvent=
        {Timestamp:uint32;
        Which:int32;
        Ball:uint8;
        Xrel:int16;
        Yrel:int16}

    type JoyHatEvent=
        {
        Timestamp:uint32;
        Which:int32     ;
        Hat:uint8       ;
        Value:uint8     ;
        }

    type JoyButtonEvent=
        {
        Timestamp:uint32;
        Which:int32     ;
        Button:uint8    ;
        State:uint8     ;
        }

    type JoyDeviceEvent=
        {
        Timestamp:uint32;
        Which:int32     ;
        }

    type ControllerAxisEvent=
        {Timestamp:uint32;
        Which:int32;
        Axis:uint8;
        Value:int16}

    type ControllerButtonEvent=
        {
        Timestamp:uint32;
        Which:int32     ;
        Button:uint8    ;
        State:uint8     ;
        } 

    type ControllerDeviceEvent=
        {
        Timestamp:uint32;
        Which:int32     ;
        }

    type AudioDeviceEvent=
        {Timestamp:uint32;
        Which:uint32;
        Iscapture:uint8}

    type TouchFingerEvent=
        {
        Timestamp:uint32;
        TouchId:int64   ;
        FingerId:int64  ;
        X:float         ;
        Y:float         ;
        Dx:float        ;
        Dy:float        ;
        Pressure:float  ;
        }

    type MultiGestureEvent=
        {
        Timestamp:uint32 ;
        TouchId:int64    ;
        DTheta:float     ;
        DDist:float      ;
        X:float          ;
        Y:float          ;
        NumFingers:uint16;
        Padding:uint16
        }

    type DollarGestureEvent=
        {
        Timestamp:uint32 ;
        TouchId:int64    ;
        GestureId:int64  ;
        NumFingers:uint32;
        Error:float      ;
        X:float          ;
        Y:float          ;
        }

    type DropEvent=
        {
        Timestamp:uint32;
        File:IntPtr     ;
        }

    type OSEvent=
        {
        Timestamp:uint32;
        }

    type UserEvent=
        {
        Type:uint32;
        Timestamp:uint32;
        WindowID:uint32;
        Code:int32;
        Data1:IntPtr;
        Data2:IntPtr;
        }

    type SysWMEvent=
        {
        Type:uint32;
        Timestamp:uint32;
        Msg:IntPtr;
        }


    type Event = 
        | Quit of QuitEvent

        | KeyDown of KeyboardEvent
        | KeyUp of KeyboardEvent

        | MouseMotion of MouseMotionEvent
        | MouseButtonDown of MouseButtonEvent
        | MouseButtonUp of MouseButtonEvent
        | MouseWheel of MouseWheelEvent              

        | AppTerminating//TODO
        | AppLowmemory//TODO
        | AppWillEnterBackground//TODO
        | AppDidEnterBackground//TODO
        | AppWillEnterForeground//TODO
        | AppDidEnterForeground//TODO

        | WindowEvent of WindowEvent       
        | SysWMEvent of SysWMEvent           
        
        | TextEditing of TextEditingEvent 
        | TextInput of TextInputEvent
        | KeyMapChanged  //TODO          

        | JoyAxisMotion of JoyAxisEvent            
        | JoyBallMotion of JoyBallEvent           
        | JoyHatMotion of JoyHatEvent            
        | JoyButtonDown of JoyButtonEvent           
        | JoyButtonUp of JoyButtonEvent
        | JoyDeviceAdded of JoyDeviceEvent           
        | JoyDeviceRemoved of JoyDeviceEvent      
       
        //toXXXX/isXXXX for Event done through here
        | ControllerAxisMotion of ControllerAxisEvent    
        | ControllerButtonDown of ControllerButtonEvent     
        | ControllerButtonUp of ControllerButtonEvent       
        | ControllerDeviceAdded of ControllerDeviceEvent
        | ControllerDeviceRemoved of ControllerDeviceEvent
        | ControllerDeviceRemapped of ControllerDeviceEvent

        | FingerDown of TouchFingerEvent      
        | FingerUp of TouchFingerEvent
        | FingerMotion of TouchFingerEvent

        | DollarGesture of DollarGestureEvent      
        | DollarRecord of DollarGestureEvent

        | MultiGesture of MultiGestureEvent

        | ClipboardUpdate   //TODO       
       
        | DropFile of DropEvent                 

        | AudioDeviceAdded of AudioDeviceEvent    
        | AudioDeviceRemoved  of AudioDeviceEvent            

        | RenderTargetsReset //TODO
        | RenderDeviceReset //TODO

        | User of UserEvent

        | Other of uint32

        member this.isQuitEvent : bool =
            match this with
            | Quit _ -> true
            | _      -> false
        member this.isKeyDownEvent : bool =
            match this with
            | KeyDown _ -> true
            | _         -> false
        member this.isKeyUpEvent : bool =
            match this with
            | KeyUp _ -> true
            | _       -> false
        member this.isKeyEvent : bool =
            this.isKeyDownEvent || this.isKeyUpEvent
        member this.isMouseMotionEvent : bool =
            match this with
            | MouseMotion _ -> true
            | _         -> false
        member this.isMouseButtonDownEvent : bool =
            match this with
            | MouseButtonDown _ -> true
            | _         -> false
        member this.isMouseButtonUpEvent : bool =
            match this with
            | MouseButtonUp _ -> true
            | _         -> false
        member this.isMouseButtonEvent : bool =
            this.isMouseButtonDownEvent || this.isMouseButtonUpEvent
        member this.isMouseWheelEvent : bool =
            match this with
            | MouseWheel _ -> true
            | _         -> false
        member this.isWindowEvent : bool =
            match this with
            | WindowEvent _ -> true
            | _         -> false
        member this.isSysWMEvent : bool =
            match this with
            | SysWMEvent _ -> true
            | _         -> false
        member this.isTextEditingEvent : bool =
            match this with
            | TextEditing _ -> true
            | _         -> false
        member this.isTextInputEvent : bool =
            match this with
            | TextInput _ -> true
            | _         -> false
        member this.isJoyAxisEvent : bool =
            match this with
            | JoyAxisMotion _ -> true
            | _         -> false
        member this.isJoyBallEvent : bool =
            match this with
            | JoyBallMotion _ -> true
            | _         -> false
        member this.isJoyHatEvent : bool =
            match this with
            | JoyHatMotion _ -> true
            | _         -> false
        member this.isJoyButtonDownEvent : bool =
            match this with
            | JoyButtonDown _ -> true
            | _         -> false
        member this.isJoyButtonUpEvent : bool =
            match this with
            | JoyButtonUp _ -> true
            | _         -> false
        member this.isJoyButtonEvent : bool =
            this.isJoyButtonDownEvent || this.isJoyButtonUpEvent
        member this.isJoyDeviceAddedEvent : bool =
            match this with
            | JoyDeviceAdded _ -> true
            | _         -> false
        member this.isJoyDeviceRemovedEvent : bool =
            match this with
            | JoyDeviceRemoved _ -> true
            | _         -> false
        member this.isJoyDeviceEvent : bool =
            this.isJoyDeviceAddedEvent || this.isJoyDeviceRemovedEvent

        member this.toQuitEvent : QuitEvent option =
            match this with
            | Quit q -> Some q
            | _      -> None
        member this.toKeyboardEvent : KeyboardEvent option =
            match this with
            | KeyUp k   -> Some k
            | KeyDown k -> Some k
            | _         -> None
        member this.toMouseMotionEvent : MouseMotionEvent option =
            match this with
            | MouseMotion m -> Some m
            | _             -> None
        member this.toMouseButtonEvent : MouseButtonEvent option =
            match this with
            | MouseButtonDown m -> Some m
            | MouseButtonUp m   -> Some m
            | _                 -> None
        member this.toMouseWheelEvent : MouseWheelEvent option =
            match this with
            | MouseWheel m -> Some m
            | _            -> None
        member this.toWindowEvent : WindowEvent option =
            match this with
            | WindowEvent w -> Some w
            | _             -> None
        member this.toSysWMEvent : SysWMEvent option =
            match this with
            | SysWMEvent s -> Some s
            | _            -> None
        member this.toTextEditingEvent : TextEditingEvent option =
            match this with
            | TextEditing e -> Some e
            | _             -> None
        member this.toTextInputEvent : TextInputEvent option =
            match this with
            | TextInput i -> Some i
            | _           -> None
        member this.toJoyAxisEvent : JoyAxisEvent option =
            match this with
            | JoyAxisMotion j -> Some j
            | _               -> None
        member this.toJoyBallEvent : JoyBallEvent option =
            match this with 
            | JoyBallMotion j -> Some j
            | _               -> None
        member this.toJoyHatEvent : JoyHatEvent option =
            match this with
            | JoyHatMotion j -> Some j
            | _              -> None
        member this.toJoyButtonEvent : JoyButtonEvent option =
            match this with
            | JoyButtonDown j -> Some j
            | JoyButtonUp j   -> Some j
            | _ -> None
        member this.toJoyDeviceEvent : JoyDeviceEvent option =
            match this with
            | JoyDeviceAdded j   -> Some j
            | JoyDeviceRemoved j -> Some j
            | _                  -> None


    let private toQuitEvent (event:SDL_QuitEvent) :QuitEvent =
        {Timestamp = event.Timestamp}

    let private toKeyboardEvent (event:SDL_KeyboardEvent) : KeyboardEvent =
        {Timestamp = event.Timestamp;
        WindowID = event.WindowID;
        State = event.State;
        Repeat = event.Repeat;
        Keysym = {Scancode = event.Keysym.Scancode |> enum<SDL.Keyboard.ScanCode>; Sym=event.Keysym.Sym;Mod=event.Keysym.Mod}}

    let private toMouseMotionEvent (event: SDL_MouseMotionEvent) :MouseMotionEvent =
        {Timestamp=event.Timestamp;
        WindowID=event.WindowID;
        Which=event.Which;
        State=event.State;
        X=event.X;
        Y=event.Y;
        Xrel=event.Xrel;
        Yrel=event.Yrel}

    let private toMouseButtonEvent (event:SDL_MouseButtonEvent) :MouseButtonEvent =
        {Timestamp = event.Timestamp;
        WindowID = event.WindowID;
        Which = event.Which;
        Button = event.Button;
        State = event.State;
        Clicks = event.Clicks;
        X = event.X;
        Y = event.Y}

    let private toMouseWheelEvent (event:SDL_MouseWheelEvent) :MouseWheelEvent =
        {Timestamp=event.Timestamp;
        WindowID=event.WindowID;
        Which=event.Which;
        X=event.X;
        Y=event.Y;
        Direction=event.Direction |> int32 |> enum<SDL.Mouse.WheelDirection>}

    let private toAudioDeviceEvent (event:SDL_AudioDeviceEvent) :AudioDeviceEvent =
        {Timestamp=event.Timestamp;
        Which=event.Which;
        Iscapture=event.Iscapture}    

    let private toControllerAxisEvent (event:SDL_ControllerAxisEvent) :ControllerAxisEvent =
        {Timestamp=event.Timestamp;
        Which=event.Which;
        Axis=event.Axis;
        Value=event.Value}
    
    let private toControllerButtonEvent (event:SDL_ControllerButtonEvent) :ControllerButtonEvent =
        {Timestamp=event.Timestamp;
        Which=event.Which;
        Button=event.Button;
        State=event.State} 
    
    let private toControllerDeviceEvent (event:SDL_ControllerDeviceEvent) :ControllerDeviceEvent=
        {Timestamp=event.Timestamp;
        Which=event.Which}

    let private convertEvent (result: bool, event:SDL_Event) =
        match result, (event.Type |> int |> enum<EventType>) with
        | true, EventType.Quit                     -> event.Quit |> toQuitEvent |> Quit |> Some

        | true, EventType.KeyDown                  -> event.Key |> toKeyboardEvent |> KeyDown |> Some
        | true, EventType.KeyUp                    -> event.Key |> toKeyboardEvent |> KeyUp |> Some

        | true, EventType.MouseMotion              -> event.Motion |> toMouseMotionEvent |> MouseMotion |> Some

        | true, EventType.MouseButtonDown          -> event.Button |> toMouseButtonEvent |> MouseButtonDown |> Some
        | true, EventType.MouseButtonUp            -> event.Button |> toMouseButtonEvent |> MouseButtonUp |> Some

        | true, EventType.MouseWheel               -> event.Wheel |> toMouseWheelEvent |> MouseWheel |> Some

        | true, EventType.AudioDeviceAdded         -> event.Adevice |> toAudioDeviceEvent |> AudioDeviceAdded |> Some
        | true, EventType.AudioDeviceRemoved       -> event.Adevice |> toAudioDeviceEvent |> AudioDeviceRemoved |> Some

        | true, EventType.ControllerAxisMotion     -> event.Caxis |> toControllerAxisEvent |> ControllerAxisMotion |> Some

        | true, EventType.ControllerButtonDown     -> event.Cbutton |> toControllerButtonEvent |> ControllerButtonDown |> Some
        | true, EventType.ControllerButtonUp       -> event.Cbutton |> toControllerButtonEvent |> ControllerButtonUp |> Some

        | true, EventType.ControllerDeviceAdded    -> event.Cdevice |> toControllerDeviceEvent |> ControllerDeviceAdded |> Some
        | true, EventType.ControllerDeviceRemoved  -> event.Cdevice |> toControllerDeviceEvent |> ControllerDeviceRemoved |> Some
        | true, EventType.ControllerDeviceRemapped -> event.Cdevice |> toControllerDeviceEvent |> ControllerDeviceRemapped |> Some

        | true, _                                  -> event.Type |> Other |> Some
        | _, _                                     -> None
    
    let wait (timeout:int option) =
        let mutable event = new SDL_Event()
        let result = 
            match timeout with
            | None -> Native.SDL_WaitEvent(&&event) = 1 
            | Some x -> Native.SDL_WaitEventTimeout(&&event,x/1) = 1
        convertEvent (result,event)
    
    let poll () =
        let mutable event = new SDL_Event()
        let result = Native.SDL_PollEvent(&&event) = 1
        convertEvent (result,event)

    let pump = Native.SDL_PumpEvents

    let register = Native.SDL_RegisterEvents

    let has (eventType:uint32) :bool =
        Native.SDL_HasEvent(eventType) <> 0

    let hasAny (minType:uint32) (maxType:uint32) : bool =
        Native.SDL_HasEvents(minType,maxType) <> 0

    let flush (eventType:uint32) =
        Native.SDL_FlushEvent(eventType)

    let flushAny (minType:uint32) (maxType:uint32) =
        Native.SDL_FlushEvents(minType,maxType)