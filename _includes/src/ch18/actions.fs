module Solitaire.Actions

open System
open Cards
open Solitaire.Model

let (|Number|_|) (ch:Char) =
  match Char.GetNumericValue(ch) with
  | -1.0 -> None
  | a -> a |> int |> Some

let deal shuffledDeck = 
  let emptyGame = {
    deck = shuffledDeck
    table = []
    stacks = []
    phase = General
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
          phase = General
        }
      
      ) emptyGame

let drawCards game =
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

let moveCardsBetweenStacks sourceStack numCards targetStack game =
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

let updateGameGeneral game command =
  match command with 
  | 'd' -> drawCards game
  | Number a when (a >= 1 && a <= 6) -> tableToStack (a - 1) game
  | 'm' -> 
      { game with 
          phase = SelectingSourceStack
      }
  | _ -> game

let updateGameSourceStack game command =
  match command with 
  | Number stack when (stack >= 1 && stack <= 6) -> 
      { game with 
          phase = SelectingNumCards stack
      }
  | '\x1B' -> // [esc] key
      { game with 
          phase = General
      }    
  | _ -> game

let updateGameNumCards sourceStack game command =
  let numCardsInStack = 
    game.stacks[sourceStack - 1] 
    |> List.filter (fun a -> a.isFaceUp ) 
    |> List.length
  match command with 
  | Number card when (card >= 1 && card <= numCardsInStack) -> 
      { game with 
          phase = SelectingTargetStack (sourceStack, card)
      }
  | '\x1B' -> // [esc] key
      { game with 
          phase = SelectingSourceStack
      }    
  | _ -> game

let updateGameTargetStack sourceStack numCards game command =
  match command with 
  | Number targetStack when (targetStack >= 1 && targetStack <= 6) -> 
      let updatedGame = 
        moveCardsBetweenStacks sourceStack numCards targetStack game
      { updatedGame with 
          phase = General
      }
  | '\x1B' -> // [esc] key
      { game with 
          phase = SelectingNumCards sourceStack
      }    
  | _ -> game  

let updateGame game command =
  match game.phase with 
  | General -> 
      updateGameGeneral game command
  | SelectingSourceStack -> 
      updateGameSourceStack game command
  | SelectingNumCards sourceStack -> 
      updateGameNumCards sourceStack game command
  | SelectingTargetStack (sourceStack, numCards) -> 
      updateGameTargetStack sourceStack numCards game command
