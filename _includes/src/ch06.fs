type CardNumber =
  | Two 
  | Three
  | Four
  | Five
  | Six
  | Seven
  | Eight
  | Nine
  | Ten
  | Jack
  | Queen
  | King
  | Ace

type Card = 
  | Hearts of CardNumber
  | Diamonds of CardNumber
  | Clubs of CardNumber
  | Spades of CardNumber
  | Joker

let hand = [Hearts Three; Diamonds Ten; Clubs King; Joker]

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

let pickupCard (hand: Card list) (deck: Card list) =
  if deck.Length = 0 then 
    failwith "No cards left!!"
  else
    let topcard = deck[0]
    hand @ [topcard]

;;
// DO IT!
pickupCard hand newDeck