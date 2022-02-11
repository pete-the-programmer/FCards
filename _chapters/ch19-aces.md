---
slug: Aces
concept: more phases
chapter: "19"
part: "Solitaire"
feature: 
  - Discriminated Union (DU)
keyword: sprintf()
---

We can now move cards around - let's look at what we've achieved and have yet to do:

{:class="naked"}
- [x] Print Screen
- [x] Get user input and make play loop
- [x] Draw cards to table
- [x] Place cards from table to deck
- [x] Move cards from stack to stack
- [ ] Ace stacks
- [ ] Rules about which card can go on another
- [ ] King special move (start new stack)
- [ ] End game and winning!

### Ace Stacks

This is part of the end game when we peel off cards from the stacks onto a special stack just for a single suit - starting with the _Ace_.

So interesting things about these stacks are:
- one stack per suit
- cards count up from Ace, Two, Three, ..., up to King
- cards can come from the bottom visible card of a stack, or the table

As the cards in an Ace stack are always face-up we can just use a plain `Card`:
```fsharp
type Game = {
  ...
  aces: Card list list
}
```

### Showing the "Ace" stacks
So we'll need to see these stacks:
```
========================== Solitaire ===========================
| 1  |  2  |  3  |  4  |  5  |  6  |===|  ♥  |  ♦  |  ♣  |  ♠  |
[###] [###] [###] [###] [###]                 [♦A ] [♣A ]
[###] [###] [###] [###]                             [♣2 ]
[###] [###] [###] [♠8 ]                             [♣3 ]
[###] [###] [♥5 ]                  
[###] [♣10]                        
[♣Q ]                              

Table: 
Deck:  [[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[###]
<d>raw cards, <1-6> put on stack, <m>ove cards between stacks, <a>ce cards, <q>uit
```

### Moving cards onto the Ace stacks

This is again going to have to be a multi-phase operation.  The player will need to tell the game:
1. That we want to put something on an ace stack
1. That the card is coming from the table, or to select a stack

We can extend the game phase type to include these possibilities:

```fsharp
type Phase = 
  ...
  SelectingAceSource
```

It doesn't need to carry any info into a further state, as once we know the stack number/table that the ace is coming from then we can automatically put it in the right stack based on its suit.

### Exercise: Showing Ace stacks

Change the code that prints the screen to include the Ace Stacks

[See an answer]({{ site.baseurl }}{{ page.url }}#showingAces)

{:class="collapsible" id="showingAces"}
```fsharp
let printStacks multiGame = 
  printfn "%s| 1  |  2  |  3  |  4  |  5  |  6  |===|  %s  |  %s  |  %s  |  %s  |" 
    clearLine SYMBOL_HEART SYMBOL_DIAMOND SYMBOL_CLUB SYMBOL_SPADE
  [0..19] |> List.iter (fun cardNum ->
    let stackline = 
      [0..5] |> List.map (fun stackNum ->
        if multiGame.game.stacks[stackNum].Length > cardNum then 
          multiGame.game.stacks[stackNum][cardNum]
          |> sprintf "[%O]"
        else
          // the stack is out of cards
            "     "         
      )
      |> fun strings -> String.Join (" ", strings)
    let aceline =
      [0..3] |> List.map (fun stackNum ->
        if multiGame.game.aces[stackNum].Length > cardNum then 
          multiGame.game.aces[stackNum][cardNum]
          |> sprintf "[%O]"
        else
          // the ace stack is out of cards
          "     "         
        )
        |> fun strings -> String.Join (" ", strings)          
    printfn "%s%s     %s" clearLine stackline aceline   // print stacks then aces on the same line
  )
  multiGame //pass it on to the next function
```



### Exercise: Add to Ace stacks

Add the update function `updateGameAceSource` to move cards to the Ace stacks. 

Also, include a change to the `printCommands` functions to keep the player informed.  

[See an answer for printing commands]({{ site.baseurl }}{{ page.url }}#printingAces)

{:class="collapsible" id="printingAces"}
```fsharp
let printCommands game =
  match game.phase with
  | General -> 
      printfn 
        "%s<d>raw cards, <1-6> put on stack, <m>ove cards between stacks, <a>ce cards, <q>uit" 
        clearLine  
  ...
  | SelectingAceSource ->
      printfn 
        "%sMove to ACE stack from stack ___(1-6) or <t>able, <esc> Go back, <q>uit" 
        clearLine    
```

[See an answer for moving to Ace stacks]({{ site.baseurl }}{{ page.url }}#movingAces)

{:class="collapsible" id="movingAces"}
```fsharp
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

let applyCommand (cmd: SolitaireCommands) (game: Game) =
  match cmd with 
  ...
  | TableToAce     -> game |> moveToAceFromTable
  | StackToAce a   -> game |> moveToAceFromStack a

... 

let updateAceSourceStack game keystroke =
  match keystroke with 
  | Number sourceStack when (sourceStack >= 1 && sourceStack <= 6) 
            -> game |> applyUpdate (StackToAce sourceStack)
  | 't'     -> game |> applyUpdate TableToAce
  | '\x1B'  -> game |> nextPhase General
  | _       -> game   

```

{% include project-so-far.md %}
