// Load the contents of another file
#load "ch13_core.fsx"
// ... and then open the file's module (automatically prefixed with filename)
open Ch13_core.Core

module Solitaire =
  open System
  type StackCard = {
    card: Card
    isFaceUp: bool
  } with 
      override this.ToString() =
        if this.isFaceUp then
          this.card.ToString()
        else 
          "###"

  type Game = {
    deck: Card list
    table: Card list
    stacks: StackCard list list
  }

  let deal shuffledDeck = 
    let emptyGame = {
      deck = shuffledDeck
      table = []
      stacks = []
    }
    [6..-1..1]  // a sequence of numbers from 6 to 1 in steps of -1 (i.e. backwards)
    |>  List.fold (fun game i -> 
          let newStack = 
            game.deck 
            |> List.take i                        // flip the last card
            |> List.mapi (fun n card -> { isFaceUp = (n = i - 1); card=card}) 
          {
            stacks = game.stacks @ [ newStack ]
            deck = game.deck |> List.skip i
            table = []
          }
        
        ) emptyGame

;;
// DO IT!
newDeck 
|> shuffle 
|> Solitaire.deal