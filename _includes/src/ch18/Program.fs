open Cards
open Solitaire

newDeck 
  |> shuffle 
  |> Actions.deal 
  |> loopGame Printing.printScreen Actions.updateGame
  |> ignore   // a program is expected to return `unit` (i.e. nothing), but the above returns a Game
              //  `ignore()` takes anything as an input and returns `unit`