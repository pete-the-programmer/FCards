module Solitaire.Actions

open System
open Cards
open Solitaire.Model

let deal shuffledDeck = 
  let emptyGame = {
    deck = shuffledDeck
    table = []
    stacks = []
    aces = List.init 4 (fun _ -> [])
  }
  [6..-1..1] 
  |>  List.fold (fun game i -> 
        let newStack = 
          game.deck 
          |> List.take i                        // flip the last card
          |> List.mapi (fun n card -> { isFaceUp = (n = i - 1); card=card}) 
        {game with
          stacks = game.stacks @ [ newStack ]
          deck = game.deck |> List.skip i
        }
      
      ) emptyGame

let (|Number|_|) (ch:Char) =
  match Char.GetNumericValue(ch) with
  | -1.0 -> None
  | a -> a |> int |> Some

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

let private flipNext stack =
  let numFaceUp =
    stack 
    |> List.filter (fun a -> a.isFaceUp)
    |> List.length
  match stack.Length, numFaceUp with 
  | 0, _ -> stack // no cards to flip
  | n, 0 -> // none face up
    stack
    |> List.updateAt 
        (n - 1) 
        {stack[n - 1] with isFaceUp=true}
  | _, _ -> stack //anything else

let private moveCardsBetweenStacks sourceStack numCards targetStack game =
  // remember - on screen we start at one, but lists start at zero
  let numCardsInStack = game.stacks[sourceStack - 1].Length
  // do the move
  let moving = game.stacks[sourceStack - 1] |> List.skip ( numCardsInStack - numCards )
  let source = game.stacks[sourceStack - 1] |> List.take ( numCardsInStack - numCards )
  let target = game.stacks[targetStack - 1] @ moving
  // flip next card?
  let sourceFlipped = flipNext source
  //reconstruct the game
  { game with 
      stacks = 
        game.stacks 
        |> List.updateAt (sourceStack - 1) sourceFlipped 
        |> List.updateAt (targetStack - 1) target 
  }

let private addToAce card game =
  let acesStackNum =
    match card with 
    | Hearts _ -> 0
    | Diamonds _ -> 1
    | Clubs _ -> 2
    | Spades _ -> 3
    | Joker _ -> failwith "AAAAH! A Joker!?!?"
  let target = game.aces[acesStackNum] @ [card]
  {game with 
    aces =
      game.aces
      |> List.updateAt acesStackNum target
  }

let private moveToAceFromStack sourceStack game =
  match game.stacks[sourceStack - 1] with 
  | [] -> game
  | [a] -> 
    let addedToAce = addToAce a.card game
    {addedToAce with 
      stacks = 
        game.stacks 
        |> List.updateAt (sourceStack - 1) [] 
    }
  | a ->
    //we need the last card, not the first
    let source, moving = 
      a 
      |> List.splitAt ( a.Length - 1 )
    let sourceFlipped = flipNext source
    let addedToAce = addToAce moving.Head.card game
    {addedToAce with 
      stacks = 
        game.stacks 
        |> List.updateAt (sourceStack - 1) sourceFlipped 
    }


let private moveToAceFromTable game =
  match game.table with 
  | [] -> game
  | [a] -> 
    let addedToAce = addToAce a game
    {addedToAce with table = [] }
  | a::rest -> 
    let addedToAce = addToAce a game
    {addedToAce with table = rest }


// The _external_ arguments for "MoveCards"
type MoveArgs = { sourceStack: int; numCards: int; targetStack: int; }

type SolitaireCommands = 
  | DrawCards
  | TableToStack of int
  | MoveCards of MoveArgs
  | TableToAce
  | StackToAce of int

let applyCommand (cmd: SolitaireCommands) (game: Game) =
  match cmd with 
  | DrawCards      -> game |> drawCards
  | TableToStack a -> game |> tableToStack (a - 1)
  | MoveCards args -> game |> moveCardsBetweenStacks args.sourceStack args.numCards args.targetStack
  | TableToAce     -> game |> moveToAceFromTable
  | StackToAce a   -> game |> moveToAceFromStack a
