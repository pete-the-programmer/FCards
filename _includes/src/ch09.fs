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

let newDeck = 
  let suits = [Hearts; Diamonds; Clubs; Spades]
  let numbers = [
    Two; Three; Four; Five; Six; Seven; Eight; Nine; Ten;
    Jack; Queen; King; Ace
  ]
  List.allPairs suits numbers
  |> List.map (fun (suit, number) -> suit number)

type Game = {
  deck: Card list
  hand: Card list
}

let pickupCard (game: Game) =
  match game.deck with 
  | [] -> failwith "No cards left!!!"
  | [a] -> { hand = game.hand @ [a]; deck = [] }
  | a::rest -> { hand = game.hand @ [a]; deck = rest }

let rec shuffle deck = 
  let random = System.Random()
  match deck with 
  | [] -> []
  | [a] -> [a]
  | _ -> // NOTE: `_` means "some value, but I don't care what it is"
      let randomPosition = random.Next(deck.Length)
      let cardAtPosition = deck[randomPosition]
      let rest = deck |> List.removeAt randomPosition
      [cardAtPosition] @ (shuffle rest)