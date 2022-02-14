module Solitaire.Website.Views

open Bolero
open Bolero.Html
open Cards
open Solitaire.Model
open Solitaire.Website.Update

let suits = [SYMBOL_HEART; SYMBOL_DIAMOND; SYMBOL_CLUB; SYMBOL_SPADE]

let wrap = List.singleton  // shortcut to wrap a thing in a list

type CardDisplay = {
  card: Card
  isFaceUp: bool
  isSelected: bool
  selection: CardSelection
}

let viewCardBack = span [ ] [ text "[###]"] 

let viewCard dispatch cardDisplay =
  match cardDisplay with 
  | { CardDisplay.isFaceUp=false } -> viewCardBack
  | { card=card; isSelected=isSelected; selection=selection } -> 
    let txt = text $"[{card}]"
    let colorAttr = 
      match card with 
      | IsRed _ -> ["red"]
      | IsBlack _ -> ["black"]
      | _ -> []
    let selectionAttr = if isSelected then ["selected"] else []
    let classAttr = colorAttr @ selectionAttr |> Attr.Classes
    span [ classAttr; on.click( fun _ -> selection |> SelectCard |> dispatch ) ] [txt]

let viewSelected (cardView:Node) = 
  match cardView with 
  | Elt (name, attrs, children) -> Elt (name, attrs @ [ Classes ["selected"] ], children)
  | _ -> cardView

let viewStacks dispatch webgame =
  webgame.game.stacks
  |> List.mapi (fun stacknum stack -> 
    stack
    |> List.mapi ( fun cardnum card -> 
        let location={stacknum=stacknum+1; cardnum=cardnum}
        { 
          card=card.card
          isFaceUp=card.isFaceUp
          isSelected=(webgame.selectedCard=StackCardSelected location)
          selection=StackCardSelected location
        }
        |> viewCard dispatch
    )
    |> fun cards -> cards @ [ span [ Classes ["dropTarget"]; on.click(fun _ -> StackTarget (stacknum + 1) |> PlaceCard |> dispatch ) ] [ text "[___]" ]  ]
    |> List.map ( fun x -> x |> wrap |> li [] )
    |> ul []
    |> fun x -> [h4 [] [(stacknum + 1).ToString() |> text]; x]
    |> div []
  )
  |> div [ Attr.Classes ["stacks"] ]

let viewAces dispatch webgame =
  webgame.game.aces
  |> List.mapi (fun stacknum stack -> 
    stack
    |> List.map ( fun card -> 
        {
          card=card
          isFaceUp=true
          isSelected=false
          selection=NoSelection
        } 
        |> viewCard dispatch
    )
    |> fun cards -> cards @ [ a [ Classes ["dropTarget"]; on.click(fun _ -> AceTarget (stacknum + 1) |> PlaceCard |> dispatch ) ] [ text "[___]" ]  ]
    |> List.map ( fun x -> x |> wrap |> li [] )
    |> ul []
    |> fun x -> [h4 [] [suits[stacknum] |> text]; x]
    |> div []
  )
  |> div [ Attr.Classes ["aces"] ]

let viewTable dispatch webgame =
  match webgame.game.table with 
    | [] -> []
    | [card] -> 
        [
          {
            card=card
            isFaceUp=true
            isSelected=(webgame.selectedCard=TableCardSelected)
            selection=NoSelection
          } 
          |> viewCard dispatch
        ]
    | topcard::rest -> 
        let facedowns = List.init (rest.Length) (fun _ -> viewCardBack)
        let faceup = 
          {
            card=topcard
            isFaceUp=true
            isSelected=(webgame.selectedCard=TableCardSelected)
            selection=TableCardSelected
          }
          |> viewCard dispatch
        facedowns @ [ faceup ]
  |> List.map ( fun a -> li [] [a])
  |> ul []
  |> fun a -> [ h3 [] [text "Table"]; a]
  |> div [ Attr.Classes ["table"] ]

let viewDeck dispatch webgame =
  List.init webgame.game.deck.Length ( fun _ -> viewCardBack |> wrap |> li [] )
  |> ul [ on.click( fun _ -> DrawCards |> dispatch  ) ]
  |> fun a -> [ h3 [] [text "Deck"]; a]
  |> div [ Attr.Classes ["deck"] ]

let viewMain webgame dispatch = 
  let stacks = viewStacks dispatch webgame
  let aces = viewAces dispatch webgame
  let table = viewTable dispatch webgame
  let deck = viewDeck dispatch webgame
  let topHalf = div [Attr.Classes ["topHalf"]] [stacks; aces]
  let mode = 
    match webgame.selectedCard with 
    | NoSelection -> "mode_unselected"
    | _ -> "mode_selected" 
  div [Attr.Classes ["game"; mode]] [topHalf; table; deck]