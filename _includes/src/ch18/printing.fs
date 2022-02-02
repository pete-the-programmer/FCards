module Solitaire.Printing

open System
open Cards
open Solitaire.Model

let clearLine = "\x1B[K"

let printHeader game =
  printfn "%s============ Solitaire =============" clearLine
  game

let printStacks game = 
  printfn "%s| 1  |  2  |  3  |  4  |  5  |  6  |" clearLine
  [0..19] |> List.iter (fun cardNum ->
    [0..5] |> List.map (fun stackNum ->
      if game.stacks[stackNum].Length > cardNum then 
        game.stacks[stackNum][cardNum]
        |> sprintf "[%O]"
      else
        // the stack is out of cards
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
    | a -> 
      String.init a.Length (fun _ -> "[")
      + a.Head.ToString()
      + "]"
  printfn "%s" clearLine //spacer
  printfn "%sTable: %s" clearLine tableLine
  game

let printDeck game =
  let deckLine = String.init game.deck.Length (fun _ -> "[") 
  printfn "%sDeck:  %s###]" clearLine deckLine
  game

let printCommands game =
  match game.phase with
  | General -> 
      printfn 
        "%s<d>raw cards, <1-6> put on stack, <m>ove cards between stacks <q>uit" 
        clearLine
  | SelectingSourceStack -> 
      printfn 
        "%s<1-6> select source stack to move from, <esc> Go back, <q>uit" 
        clearLine
  | SelectingNumCards stack-> 
      let numCardsInStack = 
        game.stacks[stack - 1] 
        |> List.filter (fun a -> a.isFaceUp ) 
        |> List.length
      printfn 
        "%sMove from stack %d at card ___(1-%d), <esc> Go back, <q>uit" 
        clearLine stack numCardsInStack
  | SelectingTargetStack (stack, card) -> 
      printfn 
        "%sMove from stack %d at card %d to stack ___, <esc> Go back, <q>uit" 
        clearLine stack card
  game

let printMoveToTop game =
  let maxCardInAnyStack = 
    game.stacks 
    |> List.map (fun stack -> stack.Length )
    |> List.max
  let n = 
    1 //header
    + 1 //stack numbers
    + 21 //stacks
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
