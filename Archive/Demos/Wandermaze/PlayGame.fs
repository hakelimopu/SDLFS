﻿module PlayGame

open RenderingContext
open SDL

let BoardColumns = 32
let BoardRows = 32

type BoardCell = Gold | Block | Fire | Water | Empty

type BoardColumn = 
    {Cells:Map<int, BoardCell>}
    member this.getGoldLeft () : int =
        this.Cells
        |> Map.filter (fun _ v -> v = Gold)
        |> Map.toList
        |> List.length
    member this.setCell (row:int, cell:BoardCell) : BoardColumn =
        {Cells = this.Cells |> Map.add row cell}
    static member create (height:int) : BoardColumn =
        let cells = 
            [0..(height-1)]
            |> List.map(fun index->index, Empty)
            |> Map.ofList
        {Cells= cells}

type Board = 
    {Columns:Map<int, BoardColumn>}
    member this.getGoldLeft() : int =
        this.Columns
        |> Map.fold (fun acc _ v -> v.getGoldLeft()) 0
    static member create (width:int, height:int) : Board =
        let columns =
            [0..(width-1)]
            |> List.map(fun index->index, (BoardColumn.create height))
            |> Map.ofList
        {Columns = columns}

type PlayerState = 
    {Column:int;
    Row:int;
    Alive:bool;
    Score:int;
    Water:int;
    Level:int;
    Moves:int}
    member this.setColumn (column:int) : PlayerState =
        {this with Column=column}
    member this.setRow (row:int) : PlayerState =
        {this with Row = row}
    member this.setAlive (alive:bool) : PlayerState =
        {this with Alive=alive}
    member this.clearScore () : PlayerState =
        {this with Score = 0}
    member this.addScore (score:int) : PlayerState =
        {this with Score = this.Score + score}
    member this.setWater (water:int) : PlayerState =
        {this with Water = water}
    member this.incrementWater () : PlayerState =
        {this with Water = this.Water + 1}
    member this.decrementWater () : PlayerState =
        {this with Water = this.Water - 1}
    member this.clearLevel () : PlayerState =
        {this with Level=0}
    member this.incrementLevel() : PlayerState =
        {this with Level = this.Level + 1}
    member this.clearMoves () : PlayerState =
        {this with Moves = 0}
    member this.incrementMoves():PlayerState =
        {this with Moves = this.Moves + 1}

type LevelState =
    {Gold:int;Blocks:int;Fire:int;Water:int}

let getInitialLevelState (level:int) =
    match level with
    |  0 -> {Gold= 64; Blocks= 32; Fire= 64; Water=8}
    |  1 -> {Gold= 64; Blocks= 40; Fire=128; Water=8}
    |  2 -> {Gold= 64; Blocks= 48; Fire=192; Water=8}
    |  3 -> {Gold= 64; Blocks= 56; Fire=256; Water=8}
    |  4 -> {Gold= 64; Blocks= 64; Fire=320; Water=7}
    |  5 -> {Gold= 64; Blocks= 72; Fire=384; Water=7}
    |  6 -> {Gold= 64; Blocks= 80; Fire=448; Water=7}
    |  7 -> {Gold= 64; Blocks= 88; Fire=512; Water=7}
    |  8 -> {Gold= 64; Blocks= 96; Fire=512; Water=6}
    |  9 -> {Gold= 64; Blocks=104; Fire=512; Water=6}
    | 10 -> {Gold= 64; Blocks=112; Fire=512; Water=6}
    | 11 -> {Gold= 64; Blocks=120; Fire=512; Water=6}
    | 12 -> {Gold= 64; Blocks=128; Fire=512; Water=5}
    | 13 -> {Gold= 64; Blocks=128; Fire=512; Water=5}
    | 14 -> {Gold= 64; Blocks=128; Fire=512; Water=5}
    | 15 -> {Gold= 64; Blocks=128; Fire=512; Water=5}
    | 16 -> {Gold= 64; Blocks=128; Fire=512; Water=4}
    | 17 -> {Gold= 64; Blocks=128; Fire=512; Water=4}
    | 18 -> {Gold= 64; Blocks=128; Fire=512; Water=4}
    | 19 -> {Gold= 64; Blocks=128; Fire=512; Water=4}
    | 20 -> {Gold= 64; Blocks=128; Fire=512; Water=3}
    | 21 -> {Gold= 64; Blocks=128; Fire=512; Water=3}
    | 22 -> {Gold= 64; Blocks=128; Fire=512; Water=3}
    | 23 -> {Gold= 64; Blocks=128; Fire=512; Water=3}
    | 24 -> {Gold= 64; Blocks=128; Fire=512; Water=2}
    | 25 -> {Gold= 64; Blocks=128; Fire=512; Water=2}
    | 26 -> {Gold= 64; Blocks=128; Fire=512; Water=2}
    | 27 -> {Gold= 64; Blocks=128; Fire=512; Water=2}
    | 28 -> {Gold= 64; Blocks=128; Fire=512; Water=1}
    | 29 -> {Gold= 64; Blocks=128; Fire=512; Water=1}
    | 30 -> {Gold= 64; Blocks=128; Fire=512; Water=1}
    | 31 -> {Gold= 64; Blocks=128; Fire=512; Water=1}
    | _  -> {Gold= 64; Blocks=128; Fire=512; Water=0}

type State = 
    {LevelState:LevelState;
    PlayerState:PlayerState;
    Board:Board;}
    member this.changeLevelState (changeFunc:LevelState->LevelState) : State=
        {this with LevelState=changeFunc(this.LevelState)}
    member this.changePlayerState (changeFunc:PlayerState->PlayerState) : State=
        {this with PlayerState=changeFunc(this.PlayerState)}
    member this.changeBoard (changeFunc:Board->Board) : State=
        {this with Board=changeFunc(this.Board)}
    static member create(width:int, height:int) : Board =
        let random = new System.Random();
        let playerState = {PlayerState.Level = 0; Column = random.Next(width); Row = random.Next(height); Alive = true; Score = 0; Water = 0; Moves = 0}
        let levelState = getInitialLevelState(playerState.Level)
        let board = Board.create(width,height)
        


let rec eventPump (context:RenderingContext) (state:State) : unit =
    context.Renderer
    |> Render.setDrawColor {Pixel.Color.Red = 0uy; Green=0uy; Blue=0uy; Alpha=255uy}  
    |> ignore

    context.Renderer
    |> Render.clear
    |> ignore

    context.Renderer
    |> Render.present
    |> ignore

    match Event.poll() with
    | Some (Event.Quit _)    -> ()
    | _                      -> eventPump context state

let run (context:RenderingContext): unit =
    {Substate = None}
    |> eventPump context

