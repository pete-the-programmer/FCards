#load "_includes/src/ch13.fsx"
open Ch13.Core

module Solitaire =
  open System
  open Ch13.Core

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
                // a list of numbers from 6 to 1 (inclusive)
    [6..-1..1]  // stepping at -1 intervals (i.e. counting down)
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
              "    "            
        )
        |> fun strings -> String.Join (" ", strings)
        |> printfn "%s"
    )
    //also print the remaining deck
    game.deck.Length |> printfn "\nDeck:  [###] - %d cards remaining"



;;
// DO IT!
newDeck 
|> shuffle 
|> Solitaire.deal 
|> Solitaire.printScreen