#load "./ch13_core.fsx"
open Ch13_core.Core

module Solitaire =
  open System

  type Game = {
    deck: Card list
    table: Card list
    stacks: Card list list
  }

  let deal shuffledDeck = 
    let emptyGame = {
      deck = shuffledDeck
      table = []
      stacks = []
    }
    [6..-1..1]
    |>  List.fold (fun game i -> 
          {
            stacks = game.stacks @ [ game.deck |> List.take i ]
            deck = game.deck |> List.skip i
            table = []
          }
        
        ) emptyGame

  let clearLine = "\x1B[L"

  let printHeader game =
    printfn "%s============ Solitaire =============" clearLine
    game

  let printStacks game = 
    let maxCardInAnyStack = 
      game.stacks 
      |> List.map (fun stack -> stack.Length )
      |> List.max
    printfn "%s| 1  |  2  |  3  |  4  |  5  |  6  |" clearLine
    [0..maxCardInAnyStack - 1]
      |> List.iter (fun cardNum ->
        [0..5]
        |> List.map (fun stackNum ->
            match game.stacks[stackNum].Length - cardNum with 
            | 1 -> 
              game.stacks[stackNum][cardNum]
              |> sprintf "[%O]"
            | x when x > 1 -> 
              "[###]"
            | _ -> 
              "     "            
        )
        |> fun strings -> String.Join (" ", strings)
        |> printfn "%s%s" clearLine
    )
    game //pass it on to the next function
  
  let printTable game =
    let tableLine = 
      match game.table with 
      | []  -> ""
      | [a] -> game.table.Head.ToString()
      | more -> 
        String.init (game.table.Length - 1) (fun _ -> "[")
        + game.table.Head.ToString()
    printfn "%s" clearLine //spacer
    printfn "%sTable: [%s]" clearLine tableLine
    game

  let printDeck game =
    let deckLine = String.init game.deck.Length (fun _ -> "[") 
    printfn "%sDeck:  %s###]" clearLine deckLine
    game

  let printCommands game =
    printfn "%s<d>raw cards, <1-6> put on stack, <q>uit" clearLine
    game

  let printMoveToTop game =
    let maxCardInAnyStack = 
      game.stacks 
      |> List.map (fun stack -> stack.Length )
      |> List.max
    let n = 
      1 //header
      + 1 //stack numbers
      + maxCardInAnyStack //stacks
      + 1 //spacer
      + 1 //table
      + 1 //deck
      + 1 //commands
      + 1 //current line
    moveUpLines n

  let printScreen game = 
    game 
    |> printHeader
    |> printStacks
    |> printTable
    |> printDeck
    |> printCommands
    |> printMoveToTop

  let (|Number|_|) (ch:Char) =
    match Char.GetNumericValue(ch) with
    | -1.0 -> None
    | a -> a |> int |> Some
  
  let drawCards game =
    let withEnoughCardsToDraw =
      match game.deck.Length with
      | n when n < 3 -> 
        // create a new game that's just like the old one
        //  but with the following differences.
        //  (really useful if you have a lot of parts but only want to change a couple)
        {game with  
          deck = game.deck @ game.table
          table = []
        }
      | _ -> game
    // in case there is less than 3 remaining
    let cardsToTake = Math.Min(3, withEnoughCardsToDraw.deck.Length)  
    {withEnoughCardsToDraw with
      table = 
        (withEnoughCardsToDraw.deck |> List.take cardsToTake)
        @ withEnoughCardsToDraw.table //(new cards on top)
      deck = withEnoughCardsToDraw.deck |> List.skip cardsToTake
    }
  
  
  // a helper to add a card to a numbered stack
  let addToStack (stackNum:int) (card:Card) (stacks: Card list list) =
    let updatedStack = stacks[stackNum] @ [card]
    stacks |> List.updateAt stackNum updatedStack
  
  let tableToStack stackNum game =
    match game.table with 
    | [] -> game // do nothing
    | [a] -> 
      {game with 
        table = []; 
        stacks = game.stacks |> addToStack stackNum a 
      }
    | a::rest -> 
      {game with 
        table = rest; 
        stacks = game.stacks |> addToStack stackNum a 
      }

  let updateGame game command =
    match command with 
    | 'd' -> drawCards game
    | Number a when (a >= 1 && a <= 6) -> tableToStack (a - 1) game
    | _ -> game

;;
// DO IT!
let play() =
  newDeck 
  |> shuffle 
  |> Solitaire.deal 
  |> looper Solitaire.printScreen Solitaire.updateGame