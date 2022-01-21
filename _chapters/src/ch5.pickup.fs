
let pickupCard (deck: Card list) (hand: Card list) =
  if deck.Length = 0 then 
    failwith "No cards left!!"
  else
    let topcard = deck.[0]
    hand @ [topcard]