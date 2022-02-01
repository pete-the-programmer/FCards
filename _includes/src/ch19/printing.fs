module Solitaire.Printing

open System
open Cards
open Solitaire.Model

let clearLine = "\x1B[K"

let printHeader game =
  printfn "%s========================== Solitaire ===========================" clearLine
  game

let maxCardInAnyStack game = 
  let maxCardInStacks = 
    game.stacks 
    |> List.map (fun stack -> stack.Length )
    |> List.max
  let maxCardInAces = 
    game.aces 
    |> List.map (fun stack -> stack.Length )
    |> List.max
  Math.Max(maxCardInAces, maxCardInStacks)

let printStacks game = 
  printfn "%s| 1  |  2  |  3  |  4  |  5  |  6  |===|  %s  |  %s  |  %s  |  %s  |" 
    clearLine SYMBOL_HEART SYMBOL_DIAMOND SYMBOL_CLUB SYMBOL_SPADE
  [0..(maxCardInAnyStack game) - 1]
    |> List.iter (fun cardNum ->
      let stackline = 
        [0..5]
        |> List.map (fun stackNum ->
            if game.stacks[stackNum].Length > cardNum then 
              game.stacks[stackNum][cardNum]
              |> sprintf "[%O]"
            else
              // the stack is out of cards
              "     "         
        )
        |> fun strings -> String.Join (" ", strings)
      let aceline =
        [0..3]
        |> List.map (fun stackNum ->
            if game.aces[stackNum].Length > cardNum then 
              game.aces[stackNum][cardNum]
              |> sprintf "[%O]"
            else
              // the ace stack is out of cards
              "     "         
        )
        |> fun strings -> String.Join (" ", strings)          
      printfn "%s%s     %s" clearLine stackline aceline
  )
  game

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
        "%s<d>raw cards, <1-6> put on stack, <m>ove cards between stacks, <a>ce cards, <q>uit" 
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
  | SelectingAceSource ->
      printfn 
        "%sMove to ACE stack from stack ___(1-6) or <t>able, <esc> Go back, <q>uit" 
        clearLine          
  game

let printMoveToTop game =
  let n = 
    1 //header
    + 1 //stack numbers
    + (maxCardInAnyStack game) //stacks & aces
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
