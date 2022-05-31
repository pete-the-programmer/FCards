module Lastcard.Views
open Bolero
open Bolero.Html
open Cards
open Lastcard.Update

type Main = Template<"wwwroot/main.html">
let main = Main()

let suits = [SYMBOL_HEART; SYMBOL_DIAMOND; SYMBOL_CLUB; SYMBOL_SPADE]

let wrap = List.singleton  // shortcut to wrap a thing in a list

let SuitNumber card =
  match card with 
  | Hearts _ -> 1
  | Diamonds _ -> 3
  | Clubs _ -> 0
  | Spades _ -> 2
  | Joker -> 4

let private viewDeck dispatch model : Node =
    model.deck
    |> List.map (fun _ -> Main.CardBack().Elt() )
    |> concat

let view model dispatch =
    main
      .Deck(viewDeck dispatch model)
      .Elt()