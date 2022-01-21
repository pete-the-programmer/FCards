module Fcards.ch8
open Fcards.ch7

type Game = {
  deck: Card list
  hand: Card list
}

let pickupCard (game: Game) =
  match game.deck with 
  | [] -> failwith "No cards left!!!"
  | [a] -> 
      {
        hand = game.hand @ [a]
        deck = []
      }
  | [a::rest] -> 
      {
        hand = hand @ [a]
        deck = rest
      }

let after3Pickups = 
  {
    hand = []
    deck = newDeck
  }
  |> pickupCard
  |> pickupCard
  |> pickupCard


//  result
  {
    hand = [ Hearts Two; Hearts Three; Hearts Four ]
    deck = [ Hearts Five; .... ]
  }