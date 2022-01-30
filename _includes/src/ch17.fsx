#load "./ch13_core.fsx"
open Ch13_core.Core

module Solitaire =
  open System

  type StackCard = {
    card: Card
    isFaceUp: bool
  } with 
      override this.ToString() =
        if this.isFaceUp then
          this.card.ToString()
        else 
          "###"

  type Phase = 
    | General
    | SelectingSourceStack
    | SelectingSourceCard of int
    | SelectingTargetStack of int * int

  type Game = {
    deck: Card list
    table: Card list
    stacks: StackCard list list
    phase: Phase
  }

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

  let clearLine = "\x1B[K"

  let printHeader game =
    printfn "%s============ Solitaire =============" clearLine
    game

  let printStacks game = 
    let maxCardInAnyStack = 
      game.stacks 
      |> List.map (fun stack -> stack.Length )
      |> List.max
    printfn "%s| 1  |  2  |  3  |  4  |  5  |  6  |" clearLine
    [0..maxCardInAnyStack - 1]
      |> List.iter (fun cardNum ->
        [0..5]
        |> List.map (fun stackNum ->
            if game.stacks[stackNum].Length > cardNum then 
              game.stacks[stackNum][cardNum]
              |> sprintf "[%O]"
            else
              // the stack is out of cards
              "     "         
        )
        |> fun strings -> String.Join (" ", strings)
        |> printfn "%s%s" clearLine
    )
    game

  let printTable game =
    let tableLine = 
      match game.table with 
      | []  -> ""
      | a -> 
        String.init a.Length (fun _ -> "[")
        + a.Head.ToString()
        + "]"
    printfn "%s" clearLine //spacer
    printfn "%sTable: %s" clearLine tableLine
    game

  let printDeck game =
    let deckLine = String.init game.deck.Length (fun _ -> "[") 
    printfn "%sDeck:  %s###]" clearLine deckLine
    game

  let printCommands game =
    match game.phase with
    | General -> printfn "%s<d>raw cards, <1-6> put on stack, <m>ove cards between stacks <q>uit" clearLine
    | SelectingSourceStack -> printfn "%s<1-6> select source stack to move from, <esc> Go back, <q>uit" clearLine
    | SelectingSourceCard stack-> 
        let numCardsInStack = game.stacks[stack - 1].Length
        printfn "%sMove from stack %d at card ___(1-%d), <esc> Go back, <q>uit" clearLine stack numCardsInStack
    | SelectingTargetStack (stack, card) -> printfn "%sMove from stack %d at card %d to stack ___, <esc> Go back, <q>uit" clearLine stack card
    game

  let printMoveToTop game =
    let maxCardInAnyStack = 
      game.stacks 
      |> List.map (fun stack -> stack.Length )
      |> List.max
    let n = 
      1 //header
      + 1 //stack numbers
      + maxCardInAnyStack //stacks
      + 1 //spacer
      + 1 //table
      + 1 //deck
      + 1 //commands
      + 1 //current line
    moveUpLines n

  let printScreen game = 
    game 
    |> printHeader
    |> printStacks
    |> printTable
    |> printDeck
    |> printCommands
    |> printMoveToTop

  let (|Number|_|) (ch:Char) =
    match Char.GetNumericValue(ch) with
    | -1.0 -> None
    | a -> a |> int |> Some

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

  let moveCardsBetweenStacks sourceStack sourceCard targetStack game =
    // remember - on screen we start at one, but lists start at zero
    let moving = game.stacks[sourceStack - 1] |> List.skip (sourceCard - 1)
    let source = game.stacks[sourceStack - 1] |> List.take (sourceCard - 1)
    let target = game.stacks[targetStack - 1] @ moving
    { game with 
        stacks = 
          game.stacks 
          |> List.updateAt (sourceStack - 1) source 
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
            phase = SelectingSourceCard stack
        }
    | '\x1B' -> // [esc] key
        { game with 
            phase = General
        }    
    | _ -> game

  let updateGameSourceCard sourceStack game command =
    let numCardsInStack = game.stacks[sourceStack - 1].Length
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

  let updateGameTargetStack sourceStack sourceCard game command =
    match command with 
    | Number targetStack when (targetStack >= 1 && targetStack <= 6) -> 
        let updatedGame = 
          moveCardsBetweenStacks sourceStack sourceCard targetStack game
        { updatedGame with 
            phase = General
        }
    | '\x1B' -> // [esc] key
        { game with 
            phase = SelectingTargetStack (sourceStack, sourceCard)
        }    
    | _ -> game  

  let updateGame game command =
    match game.phase with 
    | General -> 
        updateGameGeneral game command
    | SelectingSourceStack -> 
        updateGameSourceStack game command
    | SelectingSourceCard sourceStack -> 
        updateGameSourceCard sourceStack game command
    | SelectingTargetStack (sourceStack, sourceCard) -> 
        updateGameTargetStack sourceStack sourceCard game command

;;
// DO IT!
let play() =
  newDeck 
  |> shuffle 
  |> Solitaire.deal 
  |> looper Solitaire.printScreen Solitaire.updateGame