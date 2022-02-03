open Cards
open Solitaire
open Solitaire.Model

newDeck 
  // |> shuffle 
  |> fun cards -> 
      let d = cards |> List.except [Joker]
      {
        deck = [ Spades King ]
        table = []
        stacks = List.init 6 (fun _ -> [] )
        aces = List.init 4 (fun i -> d |> List.skip (i * 13) |> List.take 13 |> List.except [Spades King] )
        phase = General
      }
  |> loopGame Printing.printScreen Actions.updateGame
  |> ignore   // a program is expected to return `unit` (i.e. nothing), but the above returns a Game
              //  `ignore()` takes anything as an input and returns `unit`