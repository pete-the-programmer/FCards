module Fcards.ch07
open Fcards.ch03

let newDeck = 
  let suits = [Hearts; Diamonds; Clubs; Spades]
  let numbers = [
    Two; Three; Four; Five; Six; Seven; Eight; Nine; Ten;
    Jack; Queen; King; Ace
  ]

  List.allPairs suits numbers
  |> List.map (fun (suit, number) -> suit number)
