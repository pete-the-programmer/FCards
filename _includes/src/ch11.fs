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
  with 
    override this.ToString() = 
      match this with 
      | Two -> "2"
      | Three -> "3"
      | Four -> "4"
      | Five -> "5"
      | Six -> "6"
      | Seven -> "7"
      | Eight -> "8"
      | Nine -> "9"
      | Ten -> "10"
      | Jack -> "J"
      | Queen -> "Q"
      | King -> "K"
      | Ace -> "A"

type Card = 
  | Hearts of CardNumber
  | Diamonds of CardNumber
  | Clubs of CardNumber
  | Spades of CardNumber
  | Joker
  with  
    override this.ToString() = 
      match this with 
      | Hearts x -> "\u2665" + x.ToString()
      | Diamonds x -> "\u2666" + x.ToString()
      | Clubs x -> "\u2663" + x.ToString()
      | Spades x -> "\u2660" + x.ToString()
      | Joker -> "Jok"

let printOut (hand: 'a seq) =  
  "[" + String.Join("] [", hand) + "]"

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

let looper() = 
  (fun _ -> Console.ReadKey().KeyChar |> Char.ToLowerInvariant )
  |> Seq.initInfinite
  |> Seq.takeWhile (fun x -> x <> 'q')
  |> Seq.iter (fun x -> printfn "%A" x)