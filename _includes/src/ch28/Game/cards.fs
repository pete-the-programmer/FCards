module Cards
open System

type CardNumber =
  | Ace
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
  with 
    member this.Ordinal =
      match this with 
      | Ace   -> 1 
      | Two   -> 2
      | Three -> 3 
      | Four  -> 4 
      | Five  -> 5 
      | Six   -> 6 
      | Seven -> 7 
      | Eight -> 8 
      | Nine  -> 9 
      | Ten   -> 10
      | Jack  -> 11
      | Queen -> 12 
      | King  -> 13
    
type Card = 
  | Hearts of CardNumber
  | Diamonds of CardNumber
  | Clubs of CardNumber
  | Spades of CardNumber
  | Joker
  with  
    member this.Number =
      match this with 
      | Hearts a    
      | Diamonds a  
      | Clubs a     
      | Spades a   -> a
      | Joker      -> failwith "Joker?!?!?"

let (|IsRed|_|) (card:Card) =
  match card with 
  | Hearts _
  | Diamonds _ -> Some card
  | _ -> None

let (|IsBlack|_|) (card:Card) =
  match card with 
  | IsRed _ -> None
  | _ -> Some card


let (|Number|_|) (ch:Char) =
  match Char.GetNumericValue(ch) with
  | -1.0 -> None
  | a -> a |> int |> Some


let newDeck = 
  let suits = [Hearts; Diamonds; Clubs; Spades]
  let numbers = [
    Ace; Two; Three; Four; Five; Six; Seven; Eight; Nine; Ten;
    Jack; Queen; King
  ]
  List.allPairs suits numbers
  |> List.map (fun (suit, number) -> suit number)

let rec shuffle deck = 
  let random = System.Random()
  match deck with 
  | [] -> []
  | [a] -> [a]
  | _ ->
      let randomPosition = random.Next(deck.Length)
      let cardAtPosition = deck[randomPosition]
      let rest = deck |> List.removeAt randomPosition
      [cardAtPosition] @ (shuffle rest)