module Console
open System
open Cards

//SYMBOL CODES
let SYMBOL_HEART = "\u2665"
let SYMBOL_DIAMOND = "\u2666"
let SYMBOL_CLUB = "\u2663"
let SYMBOL_SPADE = "\u2660"

let printCardNumber number = 
    match number with 
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

let printCard card = 
    match card with 
    | Hearts x -> $"{SYMBOL_HEART}{printCardNumber x}"
    | Diamonds x -> $"{SYMBOL_DIAMOND}{printCardNumber x}"
    | Clubs x ->  $"{SYMBOL_CLUB}{printCardNumber x}"
    | Spades x ->  $"{SYMBOL_SPADE}{printCardNumber x}"
    | Joker -> "Jok"

let printOut (hand: 'a seq) =  
  "[" + String.Join("] [", hand) + "]"

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
