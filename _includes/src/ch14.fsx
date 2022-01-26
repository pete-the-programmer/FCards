#load "_includes/src/ch13_core.fsx"
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
                // a list of numbers from 6 to 1 (inclusive)
    [6..-1..1]  // stepping at -1 intervals (i.e. counting down)
    |>  List.fold (fun game i -> 
          {
            stacks = game.stacks @ [ game.deck |> List.take i ]
            deck = game.deck |> List.skip i
            table = []
          }
        
        ) emptyGame

;;
// DO IT!
newDeck 
|> shuffle 
|> Solitaire.deal