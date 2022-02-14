module Solitaire.Website.Views

open Bolero
open Bolero.Html
open Cards
open Solitaire.Model

let suits = [SYMBOL_HEART; SYMBOL_DIAMOND; SYMBOL_CLUB; SYMBOL_SPADE]

let wrap = List.singleton  // shortcut to wrap a thing in a list

type CardDisplay = {
  card: Card
  isFaceUp: bool
}

let viewCardBack = span [ ] [ text "[###]"] 

let viewCard (cardDisplay: CardDisplay) =
  match cardDisplay with 
  | {isFaceUp=false} -> viewCardBack
  | {card=card} -> 
    let txt = text $"[{card}]"
    let colorAttr = 
      match card with 
      | IsRed _ -> ["red"]
      | IsBlack _ -> ["black"]
      | _ -> []
      |> Attr.Classes
    span [ colorAttr ] [txt]

let viewStacks game =
  game.stacks
  |> List.mapi (fun i stack -> 
    stack
    |> List.map ( fun card -> { card=card.card; isFaceUp=card.isFaceUp } |> viewCard |> wrap |> li [] )
    |> ul []
    |> fun x -> [h4 [] [(i + 1).ToString() |> text]; x]
    |> div []
  )
  |> div [ Attr.Classes ["stacks"] ]

let viewAces game =
  game.aces
  |> List.mapi (fun i stack -> 
    stack
    |> List.map ( fun card -> {card=card; isFaceUp=true} |> viewCard |> wrap |> li [] )
    |> ul []
    |> fun x -> [h4 [] [suits[i] |> text]; x]
    |> div []
  )
  |> div [ Attr.Classes ["aces"] ]

let viewTable game =
  match game.table with 
    | [] -> []
    | [a] -> [viewCard {card=a; isFaceUp=true}]
    | topcard::rest -> 
        let facedowns = List.init rest.Length (fun _ -> viewCardBack)
        facedowns @ [viewCard {card=topcard;isFaceUp=true}]
  |> List.map ( fun a -> li [] [a])
  |> ul []
  |> fun a -> [ h3 [] [text "Table"]; a]
  |> div [ Attr.Classes ["table"] ]

let viewDeck game =
  List.init game.deck.Length ( fun _ -> viewCardBack |> wrap |> li [] )
  |> ul []
  |> fun a -> [ h3 [] [text "Deck"]; a]
  |> div [ Attr.Classes ["deck"] ]

let viewMain game dispatch = 
  let stacks = viewStacks game
  let aces = viewAces game
  let table = viewTable game
  let deck = viewDeck game
  let topHalf = div [Attr.Classes ["topHalf"]] [stacks; aces]
  div [Attr.Classes ["game"]] [topHalf; table; deck]