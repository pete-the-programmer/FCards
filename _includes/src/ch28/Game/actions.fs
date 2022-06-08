module Fish.Actions

open System
open Cards
open Fish.Model


let initialise args = 
  newDeck
  |> shuffle 
  // |> deal
  |> fun x -> {deck = x}

let drawCards (game: Game) =
  game

type SolitaireCommands = 
  | DrawCards

let applyCommand (cmd: SolitaireCommands) (game: Game) =
  match cmd with 
  | DrawCards      
      -> game |> drawCards
