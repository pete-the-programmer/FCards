module Fcards.ch5
open Fcards.ch3

let combineSuitWithNumber (suit, number) =
  suit number

let newDeck = 
  // Note: this is a 'calculated value' as it takes no inputs.
  //  So, once this value is calculated the first time then it 
  //  just keeps the value forever.
  let suits = [Hearts; Diamonds; Clubs; Spades]
  let numbers = [
    Two; Three; Four; Five; Six; Seven; Eight; Nine; Ten;
    Jack; Queen; King; Ace
  ]
  let paired = List.allPairs suits numbers
  List.map combineSuitWithNumber paired

let pickupCard (deck: Card list) (hand: Card list) =
  match deck with 
  | [] -> failwith "No cards left!!!"
  | [a] -> hand @ [a]
  | [a::rest] -> hand @ [a]