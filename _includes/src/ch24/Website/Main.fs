module Solitaire.Website.Main

open Elmish
open Bolero
open Bolero.Html
open Cards
open Solitaire.Model
open Solitaire.Actions

type SimpleMessage =  // must be a DU
  | DoNothing

let initialise args = 
  newDeck 
  |> shuffle 
  |> deal
  |> drawCards

let update message model = model

let viewCard card =
  let txt = 
    $"[{card}]"
    |> text
  let colorAttr = 
    match card with 
    | IsRed _ -> ["red"]
    | IsBlack _ -> ["black"]
    | _ -> []
    |> Attr.Classes
  [ span [ colorAttr ] [txt] ]
  
let viewCardBack card = [ span [ ] [ text "[###]"] ]  
    
let viewStackCard stackCard = 
  if stackCard.isFaceUp then  
    viewCard stackCard.card
  else
    viewCardBack stackCard.card

let viewCoveredCard card = [ span [ ] [ text "[#"] ]  

let viewStacks game =
  game.stacks
  |> List.mapi (fun i stack -> 
    stack
    |> List.map ( fun card -> card |> viewStackCard |> li [] )
    |> ul []
    |> fun x -> [h4 [] [(i + 1).ToString() |> text]; x]
    |> div []
  )
  |> div [ Attr.Classes ["stacks"] ]

let viewAces game =
  let suits = [SYMBOL_HEART; SYMBOL_DIAMOND; SYMBOL_CLUB; SYMBOL_SPADE]
  game.aces
  |> List.mapi (fun i stack -> 
    stack
    |> List.map ( fun card -> card |> viewCard |> li [] )
    |> ul []
    |> fun x -> [h4 [] [suits[i] |> text]; x]
    |> div []
  )
  |> div [ Attr.Classes ["aces"] ]

let viewTable game =
  match game.table with 
    | [] -> []
    | [a] -> viewCard a
    | a::rest -> 
      rest
      |> List.map ( fun card -> card |> viewCardBack |> li [] )
      |> fun facedown -> facedown @ (viewCard a)
  |> ul []
  |> fun a -> [ h3 [] [text "Table"]; a]
  |> div [ Attr.Classes ["table"] ]

let viewDeck game =
  game.deck
  |> List.map ( fun card -> card |> viewCardBack |> li [] )
  |> ul []
  |> fun a -> [ h3 [] [text "Deck"]; a]
  |> div [ Attr.Classes ["deck"] ]

let view game dispatch = 
  let stacks = viewStacks game
  let aces = viewAces game
  let table = viewTable game
  let deck = viewDeck game
  let topHalf = div [Attr.Classes ["topHalf"]] [stacks; aces]
  div [Attr.Classes ["game"]] [topHalf; table; deck]

type MyApp() =
  inherit ProgramComponent<Game, SimpleMessage>()

  override this.Program =
    Program.mkSimple initialise update view
