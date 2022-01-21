module Fcards.ch6

let pickupCard (hand: Card list) (deck: Card list) =
  if deck.Length = 0 then 
    failwith "No cards left!!"
  else
    let topcard = deck.[0]
    hand @ [topcard]

let hand = [Hearts Three; Diamonds Ten; Clubs King]
let updatedHand = pickupCard hand aNewDeck