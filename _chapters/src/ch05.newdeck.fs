module Fcards.ch05
open Fcards.ch03

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
  List.map (fun (suit, number) -> suit number) paired
