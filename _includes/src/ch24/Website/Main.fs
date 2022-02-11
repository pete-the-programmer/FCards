module Solitaire.Website.Main

open Elmish
open Bolero
open Bolero.Html
open Cards
open Solitaire.Model
open Solitaire.Actions
open Solitaire.Website.Views


type SimpleMessage =  // must be a DU
  | DoNothing

let initialise args = 
  newDeck 
  |> shuffle 
  |> deal
  |> applyCommand DrawCards
  
let update message model = model


type MyApp() =
  inherit ProgramComponent<Game, SimpleMessage>()

  override this.Program =
    Program.mkSimple initialise update viewMain
