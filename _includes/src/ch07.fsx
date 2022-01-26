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
  let suits = [Hearts; Diamonds; Clubs; Spades]
  let numbers = [
    Two; Three; Four; Five; Six; Seven; Eight; Nine; Ten;
    Jack; Queen; King; Ace
  ]
  List.allPairs suits numbers
  |> List.map (fun (suit, number) -> suit number)

let pickupCard (hand: Card list) (deck: Card list) =
  match deck with 
  | [] -> failwith "No cards left!!!"
  | [a] -> hand @ [a]
  | a::rest -> hand @ [a]

;;
// DO IT!
pickupCard hand newDeck