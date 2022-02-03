module Solitaire.Actions

open System
open Cards
open Solitaire.Model

let bell = "\x07"

let (|Number|_|) (ch:Char) =
  match Char.GetNumericValue(ch) with
  | -1.0 -> None
  | a -> a |> int |> Some

let deal shuffledDeck = 
  let emptyGame = {
    deck = shuffledDeck |> List.except [Joker]
    table = []
    stacks = []
    aces = List.init 6 (fun _ -> [])
    phase = General
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

type Card with 
  member this.Number =
    match this with 
    | Hearts a    
    | Diamonds a  
    | Clubs a     
    | Spades a   -> a
    | Joker      -> failwith "Joker?!?!?"

type CardNumber with 
  member this.Ordinal =
    match this with 
    | Ace   -> 1 
    | Two   -> 2
    | Three -> 3 
    | Four  -> 4 
    | Five  -> 5 
    | Six   -> 6 
    | Seven -> 7 
    | Eight -> 8 
    | Nine  -> 9 
    | Ten   -> 10
    | Jack  -> 11
    | Queen -> 12 
    | King  -> 13

let (|IsRed|_|) (card:Card) =
  match card with 
  | Hearts _
  | Diamonds _ -> Some card
  | _ -> None

let (|IsBlack|_|) (card:Card) =
  match card with 
  | IsRed _ -> None
  | _ -> Some card

let canAddToStack (stack: StackCard list) (card:Card) =
  if stack = [] && card.Number = King then 
    true
  else
    let bottomCard = stack |> List.last
    match bottomCard.card, card with 
    | IsRed a, IsBlack b
    | IsBlack a, IsRed b 
        when a.Number.Ordinal = b.Number.Ordinal + 1
            -> true
    | _, _  -> false

let canMoveCardsBetweenStacks sourceStack numCards targetStack game =
  // make things a bit easier to call the above function
  //  by making the arguments the same as the move...() function
  let stack = game.stacks[targetStack - 1]
  let card = 
    game.stacks[sourceStack - 1] 
    |> List.skip ( game.stacks[sourceStack - 1].Length - numCards )
    |> List.head
  canAddToStack stack card.card

let addToStack (stackNum:int) (card:Card) (stacks: StackCard list list) =
  let updatedStack = stacks[stackNum] @ [ { isFaceUp=true; card=card} ]
  stacks |> List.updateAt stackNum updatedStack

let tableToStack stackNum game =
  let stack = game.stacks[stackNum]
  match game.table with 
  | [] -> game // do nothing
  | [a]-> 
    {game with 
      table = []; 
      stacks = game.stacks |> addToStack stackNum a 
    }
  | a::rest -> 
    {game with 
      table = rest; 
      stacks = game.stacks |> addToStack stackNum a 
    }

let flipNext stack =
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

let moveCardsBetweenStacks sourceStack numCards targetStack game =
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

let acesStackNum card =
  match card with 
  | Hearts _ -> 0
  | Diamonds _ -> 1
  | Clubs _ -> 2
  | Spades _ -> 3
  | Joker _ -> failwith "AAAAH! A Joker!?!?"

let canAddToAce cards game =
  match cards with 
  | [] -> false
  | [card]
  | card::_ ->
    let stackNum = acesStackNum card
    let target = game.aces[stackNum] |> List.rev // (so we can easily see the last card as the "head") 
    match target, card with 
    | [], c when c.Number = Ace -> true
    | [a], c 
    | a::_, c 
        when a.Number.Ordinal = c.Number.Ordinal - 1 
        -> true
    | _ -> false

let canAddToAceFromStack sourceStack game =
  // Make it easier to call the above function for stacks
  let cards = 
    game.stacks[sourceStack - 1] 
    |> List.map (fun a -> a.card)
    |> List.rev
  canAddToAce cards game

let addToAce card game =
  let stackNum = acesStackNum card
  let target = game.aces[stackNum] @ [card]
  {game with 
    aces =
      game.aces
      |> List.updateAt stackNum target
  }

let moveToAceFromStack sourceStack game =
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


let moveToAceFromTable game =
  match game.table with 
  | [] -> game
  | [a] -> 
    let addedToAce = addToAce a game
    {addedToAce with table = [] }
  | a::rest -> 
    let addedToAce = addToAce a game
    {addedToAce with table = rest }

let hasWon game =
  game.aces 
  |> List.map List.length
  |> List.sum
  |> (=) 52   // Shortcut: 
              //  It means that we use `=` as a function 
              //  with 52 as the first input
              //  and the piped value as the second input

let updateGameGeneral game command =
  match command with 
  | 'd' -> drawCards game
  | Number a 
      when (a >= 1 && a <= 6) 
      &&   canAddToStack (game.stacks[a - 1]) (game.table.Head)
      -> 
        tableToStack (a - 1) game
  | 'm' -> 
      { game with phase = SelectingSourceStack }
  | 'a' -> 
      { game with phase = SelectingAceSource }        
  | _ -> 
    printf "%s" bell // make a noise for an unacceptable input
    game

let updateGameSourceStack game command =
  match command with 
  | Number stack when (stack >= 1 && stack <= 6) -> 
      { game with phase = SelectingNumCards stack }
  | '\x1B' -> // [esc] key
      { game with phase = General }    
  | _ -> 
    printf "%s" bell // make a noise for an unacceptable input
    game

let updateGameNumCards sourceStack game command =
  let numCardsInStack = 
    game.stacks[sourceStack - 1] 
    |> List.filter (fun a -> a.isFaceUp ) 
    |> List.length
  match command with 
  | Number card when (card >= 1 && card <= numCardsInStack) -> 
      { game with phase = SelectingTargetStack (sourceStack, card) }
  | 'a' ->
      { game with phase = SelectingTargetStack (sourceStack, numCardsInStack) }
  | '\x1B' -> // [esc] key
      { game with phase = SelectingSourceStack }    
  | _ -> 
    printf "%s" bell // make a noise for an unacceptable input
    game

let updateGameTargetStack sourceStack numCards game command =
  match command with 
  | Number targetStack 
      when (targetStack >= 1 && targetStack <= 6) 
      &&   canMoveCardsBetweenStacks sourceStack numCards targetStack game
      -> 
        let updatedGame = 
          moveCardsBetweenStacks sourceStack numCards targetStack game
        { updatedGame with phase = General }
  | '\x1B' -> // [esc] key
      { game with phase = SelectingNumCards sourceStack }    
  | _ -> 
    printf "%s" bell // make a noise for an unacceptable input
    game

let updateAceSourceStack game command =
  let updatedGame = 
    match command with 
    | Number sourceStack 
        when (sourceStack >= 1 && sourceStack <= 6) 
        &&   canAddToAceFromStack sourceStack game
        -> 
          let updatedGame = moveToAceFromStack sourceStack game
          { updatedGame with phase = General }
    | 't' when canAddToAce game.table game ->
        let updatedGame = moveToAceFromTable game
        { updatedGame with phase = General }
    | '\x1B' -> // [esc] key
        { game with phase = General }    
    | _ -> 
      printf "%s" bell // make a noise for an unacceptable input
      game
  { updatedGame with phase = if hasWon updatedGame then PlayerHasWon else updatedGame.phase }


let updatePlayerHasWon game command =
  match command with 
  | 'y' ->
      newDeck
      |> shuffle
      |> deal
  | _ -> 
    printf "%s" bell // make a noise for an unacceptable input
    game

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
  | SelectingAceSource -> 
      updateAceSourceStack game command
  | PlayerHasWon -> 
      updatePlayerHasWon game command      
