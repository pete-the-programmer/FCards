module Solitaire.Website.Main

open Elmish
open Bolero
open Bolero.Html

type SimpleModel = string

type SimpleMessage =  // must be a DU
  | DoNothing

let initialise args = ""

let update message model = model

let view model dispatch = text "Let's play Solitaire!" 

type MyApp() =
  inherit ProgramComponent<SimpleModel, SimpleMessage>()

  override this.Program =
    Program.mkSimple initialise update view
