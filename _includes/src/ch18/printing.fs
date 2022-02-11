module Solitaire.Printing

open System
open Cards
open Solitaire.Model

let clearLine = "\x1B[K"

let printHeader multiGame =
  printfn "%s============ Solitaire =============" clearLine
  multiGame

let printStacks multiGame = 
  printfn "%s| 1  |  2  |  3  |  4  |  5  |  6  |" clearLine
  [0..19] |> List.iter (fun cardNum ->
    [0..5] |> List.map (fun stackNum ->
      if multiGame.game.stacks[stackNum].Length > cardNum then 
        multiGame.game.stacks[stackNum][cardNum]
        |> sprintf "[%O]"
      else
        // the stack is out of cards
          "     "         
    )
    |> fun strings -> String.Join (" ", strings)
    |> printfn "%s%s" clearLine
  )
  multiGame //pass it on to the next function

let printTable multiGame =
  let tableLine = 
    match multiGame.game.table with 
    | []  -> ""
    | a -> 
      String.init a.Length (fun _ -> "[")
      + a.Head.ToString()
      + "]"
  printfn "%s" clearLine //spacer
  printfn "%sTable: %s" clearLine tableLine
  multiGame

let printDeck multiGame =
  let deckLine = String.init multiGame.game.deck.Length (fun _ -> "[") 
  printfn "%sDeck:  %s###]" clearLine deckLine
  multiGame

let printCommands multiGame =
  match multiGame.phase with
  | General -> 
      printfn 
        "%s<d>raw cards, <1-6> put on stack, <m>ove cards between stacks <q>uit" 
        clearLine
  | SelectingSourceStack -> 
      printfn 
        "%sMove cards from stack ___(1-6), <esc> Go back, <q>uit" 
        clearLine
  | SelectingNumCards stack-> 
      let numCardsInStack = 
        multiGame.game.stacks[stack - 1] 
        |> List.filter (fun a -> a.isFaceUp ) 
        |> List.length
      printfn 
        "%sMove ___(1-%d, or <a>ll) cards from stack %d, <esc> Go back, <q>uit" 
        clearLine numCardsInStack stack
  | SelectingTargetStack (stack, card) -> 
      printfn 
        "%sMove %d cards from stack %d to stack ___, <esc> Go back, <q>uit" 
        clearLine card stack
  multiGame

let printMoveToTop multiGame =
  let maxCardInAnyStack = 
    multiGame.game.stacks 
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
  multiGame

let printScreen multiGame = 
  multiGame 
  |> printMoveToTop
  |> printHeader
  |> printStacks
  |> printTable
  |> printDeck
  |> printCommands

