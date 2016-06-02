namespace SDL

#nowarn "9"

open System.Runtime.InteropServices
open System
open Microsoft.FSharp.NativeInterop

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute>]
module Keyboard = 

    type ScanCode =
        | Unknown = 0
        | A = 4
        | B = 5
        | C = 6
        | D = 7
        | E = 8
        | F = 9
        | G = 10
        | H = 11
        | I = 12
        | J = 13
        | K = 14
        | L = 15
        | M = 16
        | N = 17
        | O = 18
        | P = 19
        | Q = 20
        | R = 21
        | S = 22
        | T = 23
        | U = 24
        | V = 25
        | W = 26
        | X = 27
        | Y = 28
        | Z = 29
        | _1 = 30
        | _2 = 31
        | _3 = 32
        | _4 = 33
        | _5 = 34
        | _6 = 35
        | _7 = 36
        | _8 = 37
        | _9 = 38
        | _0 = 39
        | Return = 40
        | Escape = 41
        | Backspace = 42
        | Tab = 43
        | Space = 44
        | Minus = 45
        | Equals = 46
        | LeftBracket = 47
        | RightBracket = 48
        | Backslash = 49
        | NonUSHash = 50
        | Semicolon = 51
        | Apostrophe = 52
        | Grave = 53
        | Comma = 54
        | Period = 55
        | Slash = 56
        | CapsLock = 57
        | F1 = 58
        | F2 = 59
        | F3 = 60
        | F4 = 61
        | F5 = 62
        | F6 = 63
        | F7 = 64
        | F8 = 65
        | F9 = 66
        | F10 = 67
        | F11 = 68
        | F12 = 69
        | PrintScreen = 70
        | ScrollLock = 71
        | Pause = 72
        | Insert = 73
        | Home = 74
        | PageUp = 75
        | Delete = 76
        | End = 77
        | PageDown = 78
        | Right = 79
        | Left = 80
        | Down = 81
        | Up = 82
        | NumLockClear = 83
        | KeyPadDivide = 84
        | KeyPadMultiply = 85
        | KeyPadMinus = 86
        | KeyPadPlus = 87
        | KeyPadEnter = 88
        | KeyPad1 = 89
        | KeyPad2 = 90
        | KeyPad3 = 91
        | KeyPad4 = 92
        | KeyPad5 = 93
        | KeyPad6 = 94
        | KeyPad7 = 95
        | KeyPad8 = 96
        | KeyPad9 = 97
        | KeyPad0 = 98
        | KeyPadPeriod = 99
        | NonUSBackslash = 100
        | Application = 101
        | Power = 102
        | KeyPadEquals = 103
        | F13 = 104
        | F14 = 105
        | F15 = 106
        | F16 = 107
        | F17 = 108
        | F18 = 109
        | F19 = 110
        | F20 = 111
        | F21 = 112
        | F22 = 113
        | F23 = 114
        | F24 = 115
        | Execute = 116
        | Help = 117
        | Menu = 118
        | Select = 119
        | Stop = 120
        | Again = 121
        | Undo = 122
        | Cut = 123
        | Copy = 124
        | Paste = 125
        | Find = 126
        | Mute = 127
        | VolumeUp = 128
        | VolumeDown = 129
        | KeyPadComma = 133
        | KeyPadEqualsAS400 = 134
        | International1 = 135 
        | International2 = 136
        | International3 = 137 
        | International4 = 138
        | International5 = 139
        | International6 = 140
        | International7 = 141
        | International8 = 142
        | International9 = 143
        | Lang1 = 144 
        | Lang2 = 145 
        | Lang3 = 146 
        | Lang4 = 147 
        | Lang5 = 148 
        | Lang6 = 149 
        | Lang7 = 150 
        | Lang8 = 151 
        | Lang9 = 152 
        | AltErase = 153 
        | SysReq = 154
        | Cancel = 155
        | Clear = 156
        | Prior = 157
        | Return2 = 158
        | Separator = 159
        | Out = 160
        | Oper = 161
        | ClearAgain = 162
        | CRSEL = 163
        | EXSEL = 164
        | KeyPad00 = 176
        | KeyPad000 = 177
        | ThousandsSeparator = 178
        | DecimalSeparator = 179
        | CurrencyUnit = 180
        | CurrencySubunit = 181
        | KeyPadLeftParen = 182
        | KeyPadRightParen = 183
        | KeyPadLeftBrace = 184
        | KeyPadRightBrace = 185
        | KeyPadTab = 186
        | KeyPadBackspace = 187
        | KeyPadA = 188
        | KeyPadB = 189
        | KeyPadC = 190
        | KeyPadD = 191
        | KeyPadE = 192
        | KeyPadF = 193
        | KeyPadXor = 194
        | KeyPadPower = 195
        | KeyPadPercent = 196
        | KeyPadLess = 197
        | KeyPadGreater = 198
        | KeyPadAmpersand = 199
        | KeyPadDblAmpersand = 200
        | KeyPadVerticalBar = 201
        | KeyPadDblVerticalBar = 202
        | KeyPadColon = 203
        | KeyPadHash = 204
        | KeyPadSpace = 205
        | KeyPadAt = 206
        | KeyPadExclam = 207
        | KeyPadMemStore = 208
        | KeyPadMemRecall = 209
        | KeyPadMemClear = 210
        | KeyPadMemAdd = 211
        | KeyPadMemSubtract = 212
        | KeyPadMemMultiply = 213
        | KeyPadMemDivide = 214
        | KeyPadPlusMinus = 215
        | KeyPadClear = 216
        | KeyPadClearEntry = 217
        | KeyPadBinary = 218
        | KeyPadOctal = 219
        | KeyPadDecimal = 220
        | KeyPadHexadecimal = 221
        | LCtrl = 224
        | LShift = 225
        | LAlt = 226
        | LGui = 227
        | RCtrl = 228
        | RShift = 229
        | RAlt = 230
        | RGui = 231
        | Mode = 257
        | AudioNext = 258
        | AudioPrev = 259
        | AudioStop = 260
        | AudioPlay = 261
        | AudioMute = 262
        | MediaSelect = 263
        | WWW = 264
        | Mail = 265
        | Calculator = 266
        | Computer = 267
        | AcSearch = 268
        | AcHome = 269
        | AcBack = 270
        | AcForward = 271
        | AcStop = 272
        | AcRefresh = 273
        | AcBookmarks = 274
        | BrightnessDown = 275
        | BrightnessUp = 276
        | DisplaySwitch = 277
        | KbDillumToggle = 278
        | KbDillumDown = 279
        | KbDillumUp = 280
        | Eject = 281
        | Sleep = 282
        | App1 = 283
        | App2 = 284

    [<Flags>]
    type KeyModifier =
        | None = 0x0000
        | LShift = 0x0001
        | RShift = 0x0002
        | LCtrl = 0x0040
        | RCtrl = 0x0080
        | LAlt = 0x0100
        | RAlt = 0x0200
        | LGui = 0x0400
        | RGui = 0x0800
        | Num = 0x1000
        | Caps = 0x2000
        | Mode = 0x4000
        | Reserved = 0x8000

    module private Native =
        //keyboard focus
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_GetKeyboardFocus()

        //key state
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_GetKeyboardState(IntPtr numkeys)

        //modifier state
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetModState()
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_SetModState(int modstate)

        //conversions
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetKeyFromScancode(int scancode)//TODO
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetScancodeFromKey(int key)//TODO
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_GetScancodeName(int scancode)//TODO
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetScancodeFromName(IntPtr name)//TODO
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern IntPtr SDL_GetKeyName(int key)//TODO
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_GetKeyFromName(IntPtr name)//TODO

        //text input
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_StartTextInput()
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_IsTextInputActive()
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_StopTextInput()
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern void SDL_SetTextInputRect(IntPtr rect)

        //screen keyboard
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_HasScreenKeyboardSupport()
        [<DllImport(@"SDL2.dll", CallingConvention = CallingConvention.Cdecl)>]
        extern int SDL_IsScreenKeyboardShown(IntPtr window)

    let getFocus () :IntPtr =
        Native.SDL_GetKeyboardFocus()

    let private scanCodes = 
        [ScanCode.A;
        ScanCode.B;
        ScanCode.C;
        ScanCode.D;
        ScanCode.E;
        ScanCode.F;
        ScanCode.G;
        ScanCode.H;
        ScanCode.I;
        ScanCode.J;
        ScanCode.K;
        ScanCode.L;
        ScanCode.M;
        ScanCode.N;
        ScanCode.O;
        ScanCode.P;
        ScanCode.Q;
        ScanCode.R;
        ScanCode.S;
        ScanCode.T;
        ScanCode.U;
        ScanCode.V;
        ScanCode.W;
        ScanCode.X;
        ScanCode.Y;
        ScanCode.Z;
        ScanCode._1;
        ScanCode._2;
        ScanCode._3;
        ScanCode._4;
        ScanCode._5;
        ScanCode._6;
        ScanCode._7;
        ScanCode._8;
        ScanCode._9;
        ScanCode._0;
        ScanCode.Return;
        ScanCode.Escape;
        ScanCode.Backspace;
        ScanCode.Tab;
        ScanCode.Space;
        ScanCode.Minus;
        ScanCode.Equals;
        ScanCode.LeftBracket;
        ScanCode.RightBracket;
        ScanCode.Backslash;
        ScanCode.NonUSHash;
        ScanCode.Semicolon;
        ScanCode.Apostrophe;
        ScanCode.Grave;
        ScanCode.Comma;
        ScanCode.Period;
        ScanCode.Slash;
        ScanCode.CapsLock;
        ScanCode.F1;
        ScanCode.F2;
        ScanCode.F3;
        ScanCode.F4;
        ScanCode.F5;
        ScanCode.F6;
        ScanCode.F7;
        ScanCode.F8;
        ScanCode.F9;
        ScanCode.F10;
        ScanCode.F11;
        ScanCode.F12;
        ScanCode.PrintScreen;
        ScanCode.ScrollLock;
        ScanCode.Pause;
        ScanCode.Insert;
        ScanCode.Home;
        ScanCode.PageUp;
        ScanCode.Delete;
        ScanCode.End;
        ScanCode.PageDown;
        ScanCode.Right;
        ScanCode.Left;
        ScanCode.Down;
        ScanCode.Up;
        ScanCode.NumLockClear;
        ScanCode.KeyPadDivide;
        ScanCode.KeyPadMultiply;
        ScanCode.KeyPadMinus;
        ScanCode.KeyPadPlus;
        ScanCode.KeyPadEnter;
        ScanCode.KeyPad1;
        ScanCode.KeyPad2;
        ScanCode.KeyPad3;
        ScanCode.KeyPad4;
        ScanCode.KeyPad5;
        ScanCode.KeyPad6;
        ScanCode.KeyPad7;
        ScanCode.KeyPad8;
        ScanCode.KeyPad9;
        ScanCode.KeyPad0;
        ScanCode.KeyPadPeriod;
        ScanCode.NonUSBackslash;
        ScanCode.Application;
        ScanCode.Power;
        ScanCode.KeyPadEquals;
        ScanCode.F13;
        ScanCode.F14;
        ScanCode.F15;
        ScanCode.F16;
        ScanCode.F17;
        ScanCode.F18;
        ScanCode.F19;
        ScanCode.F20;
        ScanCode.F21;
        ScanCode.F22;
        ScanCode.F23;
        ScanCode.F24;
        ScanCode.Execute;
        ScanCode.Help;
        ScanCode.Menu;
        ScanCode.Select;
        ScanCode.Stop;
        ScanCode.Again;
        ScanCode.Undo;
        ScanCode.Cut;
        ScanCode.Copy;
        ScanCode.Paste;
        ScanCode.Find;
        ScanCode.Mute;
        ScanCode.VolumeUp;
        ScanCode.VolumeDown;
        ScanCode.KeyPadComma;
        ScanCode.KeyPadEqualsAS400;
        ScanCode.International1;
        ScanCode.International2;
        ScanCode.International3;
        ScanCode.International4;
        ScanCode.International5;
        ScanCode.International6;
        ScanCode.International7;
        ScanCode.International8;
        ScanCode.International9;
        ScanCode.Lang1;
        ScanCode.Lang2;
        ScanCode.Lang3;
        ScanCode.Lang4;
        ScanCode.Lang5;
        ScanCode.Lang6;
        ScanCode.Lang7;
        ScanCode.Lang8;
        ScanCode.Lang9;
        ScanCode.AltErase;
        ScanCode.SysReq;
        ScanCode.Cancel;
        ScanCode.Clear;
        ScanCode.Prior;
        ScanCode.Return2;
        ScanCode.Separator;
        ScanCode.Out;
        ScanCode.Oper;
        ScanCode.ClearAgain;
        ScanCode.CRSEL;
        ScanCode.EXSEL;
        ScanCode.KeyPad00;
        ScanCode.KeyPad000;
        ScanCode.ThousandsSeparator;
        ScanCode.DecimalSeparator;
        ScanCode.CurrencyUnit;
        ScanCode.CurrencySubunit;
        ScanCode.KeyPadLeftParen;
        ScanCode.KeyPadRightParen;
        ScanCode.KeyPadLeftBrace;
        ScanCode.KeyPadRightBrace;
        ScanCode.KeyPadTab;
        ScanCode.KeyPadBackspace;
        ScanCode.KeyPadA;
        ScanCode.KeyPadB;
        ScanCode.KeyPadC;
        ScanCode.KeyPadD;
        ScanCode.KeyPadE;
        ScanCode.KeyPadF;
        ScanCode.KeyPadXor;
        ScanCode.KeyPadPower;
        ScanCode.KeyPadPercent;
        ScanCode.KeyPadLess;
        ScanCode.KeyPadGreater;
        ScanCode.KeyPadAmpersand;
        ScanCode.KeyPadDblAmpersand;
        ScanCode.KeyPadVerticalBar;
        ScanCode.KeyPadDblVerticalBar;
        ScanCode.KeyPadColon;
        ScanCode.KeyPadHash;
        ScanCode.KeyPadSpace;
        ScanCode.KeyPadAt;
        ScanCode.KeyPadExclam;
        ScanCode.KeyPadMemStore;
        ScanCode.KeyPadMemRecall;
        ScanCode.KeyPadMemClear;
        ScanCode.KeyPadMemAdd;
        ScanCode.KeyPadMemSubtract;
        ScanCode.KeyPadMemMultiply;
        ScanCode.KeyPadMemDivide;
        ScanCode.KeyPadPlusMinus;
        ScanCode.KeyPadClear;
        ScanCode.KeyPadClearEntry;
        ScanCode.KeyPadBinary;
        ScanCode.KeyPadOctal;
        ScanCode.KeyPadDecimal;
        ScanCode.KeyPadHexadecimal;
        ScanCode.LCtrl;
        ScanCode.LShift;
        ScanCode.LAlt;
        ScanCode.LGui;
        ScanCode.RCtrl;
        ScanCode.RShift;
        ScanCode.RAlt;
        ScanCode.RGui;
        ScanCode.Mode;
        ScanCode.AudioNext;
        ScanCode.AudioPrev;
        ScanCode.AudioStop;
        ScanCode.AudioPlay;
        ScanCode.AudioMute;
        ScanCode.MediaSelect;
        ScanCode.WWW;
        ScanCode.Mail;
        ScanCode.Calculator;
        ScanCode.Computer;
        ScanCode.AcSearch;
        ScanCode.AcHome;
        ScanCode.AcBack;
        ScanCode.AcForward;
        ScanCode.AcStop;
        ScanCode.AcRefresh;
        ScanCode.AcBookmarks;
        ScanCode.BrightnessDown;
        ScanCode.BrightnessUp;
        ScanCode.DisplaySwitch;
        ScanCode.KbDillumToggle;
        ScanCode.KbDillumDown;
        ScanCode.KbDillumUp;
        ScanCode.Eject;
        ScanCode.Sleep;
        ScanCode.App1;
        ScanCode.App2]

    let getState () : Map<ScanCode,bool> =
        let kbState = 
            Native.SDL_GetKeyboardState(IntPtr.Zero)
            |> NativePtr.ofNativeInt<uint8>
        let getStatus (code:ScanCode) :bool=
            NativePtr.add kbState (code |> int)
            |> NativePtr.read
            |> (=) 1uy
        scanCodes
        |> Seq.map (fun code->(code,code|>getStatus))
        |> Map.ofSeq

    let getModifierState () : KeyModifier =
        Native.SDL_GetModState()
        |> enum<KeyModifier>

    let setModifierState (state:KeyModifier) :unit =
        Native.SDL_SetModState(state|>int)

    let startTextInput () :unit =
        Native.SDL_StartTextInput()

    let isTextInputActive (): bool = 
        0 <> Native.SDL_IsTextInputActive()

    let stopTextInput () :unit =
        Native.SDL_StopTextInput()

    let setTextInputRectangle (rectangle: SDL.Geometry.Rectangle option) :unit = 
        rectangle
        |> SDL.Geometry.withSDLRectPointer (fun rect->Native.SDL_SetTextInputRect(rect))

    let hasScreenKeyboardSupport () :bool =    
        0 <> Native.SDL_HasScreenKeyboardSupport()

    let isScreenKeyboardShown (window:IntPtr) :bool =
        0 <> Native.SDL_IsScreenKeyboardShown(window)
