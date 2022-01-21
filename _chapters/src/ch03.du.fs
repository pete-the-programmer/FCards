module Fcards.ch03  

type Suit = 
  | Hearts
  | Diamonds
  | Clubs 
  | Spades

type Card = {
  suit: Suit
  value: int
}

let myCard = { suit = Clubs; value=3 }