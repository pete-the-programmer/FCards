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

let private drawCards game =
  let withEnoughCardsToDraw =
    match game.deck.Length with
    | n when n < 3 -> 
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
      @ withEnoughCardsToDraw.table
    deck = withEnoughCardsToDraw.deck |> List.skip cardsToTake
  }

// a helper to add a card to a numbered stack
let private addToStack (stackNum:int) (card:Card) (stacks: StackCard list list) =
  let updatedStack = stacks[stackNum] @ [ { isFaceUp=true; card=card} ]
  stacks |> List.updateAt stackNum updatedStack

let private tableToStack stackNum game =
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

let private moveCardsBetweenStacks sourceStack numCards targetStack game =
  // remember - on screen we start at one, but lists start at zero
  let numCardsInStack = game.stacks[sourceStack - 1].Length
  // do the move
  let moving = game.stacks[sourceStack - 1] |> List.skip ( numCardsInStack - numCards )
  let source = game.stacks[sourceStack - 1] |> List.take ( numCardsInStack - numCards )
  let target = game.stacks[targetStack - 1] @ moving
  let numFaceUp =
    source 
    |> List.filter (fun a -> a.isFaceUp)
    |> List.length
  // flip next card?
  let sourceFlipped =
    match source.Length, numFaceUp with 
    | 0, _ -> source // no cards to flip
    | n, 0 -> // none face up
      source
      |> List.updateAt 
          (n - 1) 
          {source[n - 1] with isFaceUp=true}
    | _, _ -> source //anything else

  //reconstruct the game
  { game with 
      stacks = 
        game.stacks 
        |> List.updateAt (sourceStack - 1) sourceFlipped 
        |> List.updateAt (targetStack - 1) target 
  }

type MoveArgs = { sourceStack: int; numCards: int; targetStack: int; }

type SolitaireCommands = 
  | DrawCards
  | TableToStack of int
  | MoveCards of MoveArgs

let applyCommand (cmd: SolitaireCommands) (game: Game) =
  match cmd with 
  | DrawCards -> game |> drawCards
  | TableToStack a -> game |> tableToStack (a - 1)
  | MoveCards args -> game |> moveCardsBetweenStacks args.sourceStack args.numCards args.targetStack
