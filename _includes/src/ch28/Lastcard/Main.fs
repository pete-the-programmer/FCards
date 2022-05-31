module Lastcard.Main

open Elmish
open Bolero
open Lastcard.Views
open Lastcard.Update


type MyApp() =
    inherit ProgramComponent<LastcardGame, Message>()

    override this.Program =
        Program.mkSimple (fun _ -> initModel) update view
