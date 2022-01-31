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
    | SelectingNumCards of int
    | SelectingTargetStack of int * int
    | SelectingAceSource

  type Game = {
    deck: Card list
    table: Card list
    stacks: StackCard list list
    aces: Card list list
    phase: Phase
  }

  let deal shuffledDeck = 
    let emptyGame = {
      deck = shuffledDeck
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

  let clearLine = "\x1B[K"

  let printHeader game =
    printfn "%s========================== Solitaire ===========================" clearLine
    game

  let maxCardInAnyStack game = 
    let maxCardInStacks = 
      game.stacks 
      |> List.map (fun stack -> stack.Length )
      |> List.max
    let maxCardInAces = 
      game.aces 
      |> List.map (fun stack -> stack.Length )
      |> List.max
    Math.Max(maxCardInAces, maxCardInStacks)

  let printStacks game = 
    printfn "%s| 1  |  2  |  3  |  4  |  5  |  6  |===|  %s  |  %s  |  %s  |  %s  |" 
      clearLine SYMBOL_HEART SYMBOL_DIAMOND SYMBOL_CLUB SYMBOL_SPADE
    [0..(maxCardInAnyStack game) - 1]
      |> List.iter (fun cardNum ->
        let stackline = 
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
        let aceline =
          [0..3]
          |> List.map (fun stackNum ->
              if game.aces[stackNum].Length > cardNum then 
                game.aces[stackNum][cardNum]
                |> sprintf "[%O]"
              else
                // the ace stack is out of cards
                "     "         
          )
          |> fun strings -> String.Join (" ", strings)          
        printfn "%s%s     %s" clearLine stackline aceline
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
    | General -> 
        printfn 
          "%s<d>raw cards, <1-6> put on stack, <m>ove cards between stacks, <a>ce cards, <q>uit" 
          clearLine
    | SelectingSourceStack -> 
        printfn 
          "%s<1-6> select source stack to move from, <esc> Go back, <q>uit" 
          clearLine
    | SelectingNumCards stack-> 
        let numCardsInStack = 
          game.stacks[stack - 1] 
          |> List.filter (fun a -> a.isFaceUp ) 
          |> List.length
        printfn 
          "%sMove from stack %d at card ___(1-%d), <esc> Go back, <q>uit" 
          clearLine stack numCardsInStack
    | SelectingTargetStack (stack, card) -> 
        printfn 
          "%sMove from stack %d at card %d to stack ___, <esc> Go back, <q>uit" 
          clearLine stack card
    | SelectingAceSource ->
        printfn 
          "%sMove to ACE stack from stack ___(1-6) or <t>able, <esc> Go back, <q>uit" 
          clearLine          
    game

  let printMoveToTop game =
    let n = 
      1 //header
      + 1 //stack numbers
      + (maxCardInAnyStack game) //stacks & aces
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

  let addToAce card game =
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

  let updateGameGeneral game command =
    match command with 
    | 'd' -> drawCards game
    | Number a when (a >= 1 && a <= 6) -> tableToStack (a - 1) game
    | 'm' -> 
        { game with phase = SelectingSourceStack }
    | 'a' -> 
        { game with phase = SelectingAceSource }        
    | _ -> game

  let updateGameSourceStack game command =
    match command with 
    | Number stack when (stack >= 1 && stack <= 6) -> 
        { game with phase = SelectingNumCards stack }
    | '\x1B' -> // [esc] key
        { game with phase = General }    
    | _ -> game

  let updateGameNumCards sourceStack game command =
    let numCardsInStack = 
      game.stacks[sourceStack - 1] 
      |> List.filter (fun a -> a.isFaceUp ) 
      |> List.length
    match command with 
    | Number card when (card >= 1 && card <= numCardsInStack) -> 
        { game with phase = SelectingTargetStack (sourceStack, card) }
    | '\x1B' -> // [esc] key
        { game with phase = SelectingSourceStack }    
    | _ -> game

  let updateGameTargetStack sourceStack numCards game command =
    match command with 
    | Number targetStack when (targetStack >= 1 && targetStack <= 6) -> 
        let updatedGame = 
          moveCardsBetweenStacks sourceStack numCards targetStack game
        { updatedGame with phase = General }
    | '\x1B' -> // [esc] key
        { game with phase = SelectingTargetStack (sourceStack, numCards) }    
    | _ -> game  

  let updateAceSourceStack game command =
    match command with 
    | Number sourceStack when (sourceStack >= 1 && sourceStack <= 6) -> 
        let updatedGame = moveToAceFromStack sourceStack game
        { updatedGame with phase = General }
    | 't' ->
        let updatedGame = moveToAceFromTable game
        { updatedGame with phase = General }
    | '\x1B' -> // [esc] key
        { game with phase = General }    
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
    | SelectingAceSource -> 
        updateAceSourceStack game command
;;
// DO IT!
let play() =
  newDeck 
  |> shuffle 
  |> Solitaire.deal 
  |> looper Solitaire.printScreen Solitaire.updateGame