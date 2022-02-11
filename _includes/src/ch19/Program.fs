open Cards
open Solitaire
open Solitaire.Model

newDeck 
  |> shuffle 
  |> fun cards -> {game=Actions.deal cards; phase=General}
  |> loopGame Printing.printScreen Update.updateGame
  |> ignore   // a program is expected to return `unit` (i.e. nothing), but the above returns a Game
              //  `ignore()` takes anything as an input and returns `unit`