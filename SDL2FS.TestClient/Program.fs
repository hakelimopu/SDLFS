open SDL

open System

type ViewFunc<'TModel> = 'TModel -> unit 

type View<'TModel> =
    | Single of ViewFunc<'TModel>
    | Composite of View<'TModel> list

type Controller<'TEvent, 'TModel, 'TIdentifier> when 'TIdentifier: comparison = 
    'TEvent -> ('TModel * View<'TModel>) -> ('TModel * View<'TModel> * ('TIdentifier option))

type State<'TEvent,'TModel,'TIdentifier> when 'TIdentifier: comparison =
    {Model:'TModel;
    View:View<'TModel>;
    Controllers:Map<'TIdentifier, Controller<'TEvent, 'TModel,'TIdentifier>>}

//let rec eventPump (eventSource:unit->'TEvent) (identifierFetcher:State<'TEvent,'TModel,'TIdentifier>->'TIdentifier) (state: State<'TEvent,'TModel,'TIdentifier>) =
//    let event = eventSource()
//    let identifier = state |> identifierFetcher
//    let (m,v,i) =
//        state.Controllers
//        |> Map.f
//         event (state.Model, state.View)
//    if i.


let rec mainMenu () =
    Console.WriteLine()
    Console.WriteLine("Main Menu:")
    Console.WriteLine("[W]indow tests")
    Console.WriteLine("[Q]uit")
    let keyInfo = Console.ReadKey(true)
    if keyInfo.Key = ConsoleKey.Q then
        ()
    else
        match keyInfo.Key with 
        | ConsoleKey.W -> WindowTests.windowMenu()
        | _ -> ()
        mainMenu()


[<EntryPoint>]
let main argv = 
    mainMenu()

    0
