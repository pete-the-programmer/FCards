module Cards
open System

//COLOR CODES
let COLOR_DEFAULT = "\x1B[0m"
let COLOR_RED = "\x1B[91m"
let COLOR_BLACK = "\x1B[90m"

let SYMBOL_HEART = "\u2665"
let SYMBOL_DIAMOND = "\u2666"
let SYMBOL_CLUB = "\u2663"
let SYMBOL_SPADE = "\u2660"

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
      | Two -> "2 "
      | Three -> "3 "
      | Four -> "4 "
      | Five -> "5 "
      | Six -> "6 "
      | Seven -> "7 "
      | Eight -> "8 "
      | Nine -> "9 "
      | Ten -> "10"
      | Jack -> "J "
      | Queen -> "Q "
      | King -> "K "
      | Ace -> "A "

type Card = 
  | Hearts of CardNumber
  | Diamonds of CardNumber
  | Clubs of CardNumber
  | Spades of CardNumber
  | Joker
  with  
    override this.ToString() = 
      match this with 
      | Hearts x -> $"{COLOR_RED}{SYMBOL_HEART}{x}{COLOR_DEFAULT}"
      | Diamonds x -> $"{COLOR_RED}{SYMBOL_DIAMOND}{x}{COLOR_DEFAULT}"
      | Clubs x ->  $"{COLOR_BLACK}{SYMBOL_CLUB}{x}{COLOR_DEFAULT}"
      | Spades x ->  $"{COLOR_BLACK}{SYMBOL_SPADE}{x}{COLOR_DEFAULT}"
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

//moves the cursor up "n" lines
let moveUpLines n = 
  printfn "\x1B[%dA" n

let combineUpdate printScreen updater game command = 
  updater game command
  |> printScreen

let loopGame<'G> 
    (printScreen: 'G -> 'G) 
    (updater: 'G -> char -> 'G) 
    (initialGame: 'G) = 
  printScreen initialGame |> ignore
  (fun _ -> Console.ReadKey().KeyChar |> Char.ToLowerInvariant)
  |> Seq.initInfinite
  |> Seq.takeWhile (fun x -> x <> 'q')
  |> Seq.fold (combineUpdate printScreen updater) initialGame
