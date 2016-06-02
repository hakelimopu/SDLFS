namespace SDL

#nowarn "9"

open System.Runtime.InteropServices
open System
open SDL
open Microsoft.FSharp.NativeInterop

module MessageBox =

    [<Flags>]
    type Flags = 
        | Error       = 0x10
        | Warning     = 0x20
        | Information = 0x40

    [<Flags>]
    type ButtonFlags = 
        | None             = 0
        | ReturnKeyDefault = 1
        | EscapeKeyDefault = 2

    type ColorType =
        | Background       = 0
        | Text             = 1
        | ButtonBorder     = 2
        | ButtonBackground = 3
        | ButtonSelected   = 4

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_MessageBoxButtonData =
        struct
            val mutable flags    : uint32
            val mutable buttonid : int
            val mutable text     : IntPtr
        end

    type ButtonData =
        {Flags: ButtonFlags;
        Id:int;
        Text:string}

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_MessageBoxColor =
        struct
            val mutable r : uint8
            val mutable g : uint8
            val mutable b : uint8
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_MessageBoxColorScheme =
        struct
            val mutable BackgroundColor       : SDL_MessageBoxColor
            val mutable TextColor             : SDL_MessageBoxColor
            val mutable ButtonBorderColor     : SDL_MessageBoxColor
            val mutable ButtonBackgroundColor : SDL_MessageBoxColor
            val mutable ButtonSelectedColor   : SDL_MessageBoxColor
        end

    type ColorScheme =
        {Background:SDL.Pixel.Color;
        Text:SDL.Pixel.Color;
        ButtonBorder:SDL.Pixel.Color;
        ButtonBackground:SDL.Pixel.Color;
        ButtonSelected:SDL.Pixel.Color}

    [<StructLayout(LayoutKind.Sequential)>]
    type internal SDL_MessageBoxData =
        struct
            val mutable flags       : uint32
            val mutable window      : IntPtr
            val mutable title       : IntPtr
            val mutable message     : IntPtr
            val mutable numbuttons  : int
            val mutable buttons     : IntPtr
            val mutable colorScheme : IntPtr
        end

    type MessageBoxData =
        {Flags:Flags;
        Window:SDL.Window.Window option;
        Title:string;
        Message:string;
        Buttons:seq<ButtonData>;
        ColorScheme:ColorScheme option}

    module private Native =
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_ShowMessageBox(IntPtr messageboxdata, IntPtr buttonid);
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_ShowSimpleMessageBox(uint32 flags, IntPtr title, IntPtr message, IntPtr window);

    let showSimpleMessageBox (flags:Flags) (title:string) (message:string) (window:SDL.Window.Window option) :unit =
        title
        |> SDL.Utility.withUtf8String(fun titlePtr -> 
            message
            |> SDL.Utility.withUtf8String(fun messagePtr ->
                let windowPtr = if window.IsSome then window.Value.Pointer else IntPtr.Zero
                Native.SDL_ShowSimpleMessageBox(flags |> uint32, titlePtr,messagePtr,windowPtr)
                |>ignore))

//    let showMessageBox (data:MessageBoxData) :int option =
//        data.Title
//        |> SDL.Utility.withUtf8String(fun titlePtr -> 
//            data.Message
//            |> SDL.Utility.withUtf8String(fun messagePtr ->
//                let buttons = 
//                    data.Buttons
//                    |> Seq.map(fun button -> 
//                        let alloc = SDL.Utility.allocUtf8String button.Text
//                        let mutable data = new SDL_MessageBoxButtonData()
//                        data.flags <- button.Flags |> uint32
//                        data.buttonid <- button.Id
//                        data.text <- alloc.AddrOfPinnedObject()
//                        (alloc, data))
//                //TODO: finish me
//                None))
