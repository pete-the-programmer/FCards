module Solitaire.Actions

open System
open Cards
open Solitaire.Model

let deal shuffledDeck = 
  let emptyGame = {
    deck = shuffledDeck
    table = []
    stacks = []
  }
  [6..-1..1] 
  |>  List.fold (fun game i -> 
        let newStack = 
          game.deck 
          |> List.take i                        // flip the last card
          |> List.mapi (fun n card -> { isFaceUp = (n = i - 1); card=card}) 
        {
          stacks = game.stacks @ [ newStack ]
          deck = game.deck |> List.skip i
          table = []
        }
      
      ) emptyGame

let (|Number|_|) (ch:Char) =
  match Char.GetNumericValue(ch) with
  | -1.0 -> None
  | a -> a |> int |> Some

let drawCards game =
  let withEnoughCardsToDraw =
    match game.deck.Length with
    | n when n < 3 -> 
      // create a new game that's just like the old one
      //  but with the following differences.
      //  (really useful if you have a lot of parts but only want to change a couple)
      {game with  
        deck = game.deck @ game.table
        table = []
      }
    | _ -> game
  // in case there is less than 3 remaining
  let cardsToTake = Math.Min(3, withEnoughCardsToDraw.deck.Length)  
  {withEnoughCardsToDraw with
    table = 
      (withEnoughCardsToDraw.deck |> List.take cardsToTake)
      @ withEnoughCardsToDraw.table //(new cards on top)
    deck = withEnoughCardsToDraw.deck |> List.skip cardsToTake
  }


// a helper to add a card to a numbered stack
let addToStack (stackNum:int) (card:Card) (stacks: StackCard list list) =
  let updatedStack = stacks[stackNum] @ [ { isFaceUp=true; card=card} ]
  stacks |> List.updateAt stackNum updatedStack

let tableToStack stackNum game =
  match game.table with 
  | [] -> game // do nothing
  | [a] -> 
    {game with 
      table = []; 
      stacks = game.stacks |> addToStack stackNum a 
    }
  | a::rest -> 
    {game with 
      table = rest; 
      stacks = game.stacks |> addToStack stackNum a 
    }

type SolitaireCommands = 
  | DrawCards
  | TableToStack of int

let applyCommand (cmd: SolitaireCommands) (game: Game) =
  match cmd with 
  | DrawCards -> game |> drawCards
  | TableToStack a -> game |> tableToStack (a - 1)

let updateGame game command =
  match command with 
  | 'd' -> game |> applyCommand DrawCards
  | Number a when (a >= 1 && a <= 6) -> game |> applyCommand (TableToStack a)
  | _ -> game
