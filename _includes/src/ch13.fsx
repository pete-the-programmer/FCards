open System

module Core =
  //COLOR CODES
  let COLOR_DEFAULT = "\x1B[0m"
  let COLOR_RED = "\x1B[91m"
  let COLOR_BLACK = "\x1B[90m"

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
        | Hearts x -> $"{COLOR_RED}\u2665{x}{COLOR_DEFAULT}"
        | Diamonds x -> $"{COLOR_RED}\u2666{x}{COLOR_DEFAULT}"
        | Clubs x ->  $"{COLOR_BLACK}\u2663{x}{COLOR_DEFAULT}"
        | Spades x ->  $"{COLOR_BLACK}\u2660{x}{COLOR_DEFAULT}"
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
    let updated = updater game command
    printScreen updated
    updated 

  let loopGame<'G> 
      (printScreen: 'G -> unit) 
      (updater: 'G -> char -> 'G) 
      (initialGame: 'G) = 
    printScreen initialGame
    (fun _ -> Console.ReadKey().KeyChar |> Char.ToLowerInvariant)
    |> Seq.initInfinite
    |> Seq.takeWhile (fun x -> x <> 'q')
    |> Seq.fold (combineUpdate printScreen updater) initialGame

////////////////////  OUR SIMPLE PICKUP GAME ////////////////////
module Pickup = 
  open Core 

  type Game = {
    deck: Card list
    hand: Card list
  } with
      override this.ToString() =
        $"[###] - {this.deck.Length}\n" + (printOut this.hand)

  let pickupCard (game: Game) =
    match game.deck with 
    | [] -> failwith "No cards left!!!"
    | [a] -> { hand = game.hand @ [a]; deck = [] }
    | a::rest -> { hand = game.hand @ [a]; deck = rest }

  let printPickupScreen game =
    printfn "===Pickup Cards==="
    printfn "%O" game
    printfn "<p>ickup card <q>uit"
    moveUpLines 5

  let updateGame game command = 
    match command with 
    | 'p' -> game |> pickupCard
    | _ -> game

  let play() =
    let startingpoint = 
      {
        deck = newDeck |> shuffle
        hand = []
      }
    loopGame printPickupScreen updateGame startingpoint


;;
// DO IT!
Pickup.play()