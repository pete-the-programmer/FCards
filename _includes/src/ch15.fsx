#load "ch13_core.fsx"
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

  let printScreen game = 
    let maxCardInAnyStack = 
      game.stacks 
      |> List.map (fun stack -> stack.Length )
      |> List.max
    printfn "| 1  |  2  |  3  |  4  |  5  |  6  |"
    [0..maxCardInAnyStack - 1]
      |> List.iter (fun cardNum ->
        [0..5]
        |> List.map (fun stackNum ->
            match game.stacks[stackNum].Length - cardNum with 
            | 1 -> 
              // the top-most card
              game.stacks[stackNum][cardNum]
              |> sprintf "[%O]"
            | x when x > 1 -> 
              // a card in the stack, but not the top one
              "[###]"
            | _ -> 
              // the stack is out of cards
              "     "            
        )
        |> fun strings -> String.Join (" ", strings)
        |> printfn "%s"
    )
    //also print the remaining deck
    String.init game.deck.Length (fun _ -> "[")  |> printfn "\nDeck:  %s###]"



;;
// DO IT!
newDeck 
|> shuffle 
|> Solitaire.deal 
|> Solitaire.printScreen