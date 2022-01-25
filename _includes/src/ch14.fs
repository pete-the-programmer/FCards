module Solitaire
open System
open Core

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

//  Let do a deal!
newDeck |> shuffle |> deal