---
slug: Moving Stacks
concept: 2-phase moves
chapter: "18"
part: "Solitaire"
feature: 
  - Discriminated Union (DU)
  - Updating state
  - Visibility
keyword:
  - private
---

We can now make our stacks grow - let's look at what we've achieved and have yet to do:

{:class="naked"}
- [x] Print Screen
- [x] Get user input and make play loop
- [x] Draw cards to table
- [x] Place cards from table to deck
- [ ] Move cards from stack to stack
- [ ] Ace stacks
- [ ] Rules about which card can go on another
- [ ] King special move (start new stack)
- [ ] End game and winning!

### Moving cards from one stack to another

So far, all of our moves have been a single keystroke command. But when we move cards from one stack to another we need to tell the game a few things:
1. That we want to move a stack
2. Which stack to move
3. How many face-up cards to move
4. Which stack to move the card into

This complicates our update loop, as now we will expect the input and gameplay to come in multiple command _phases_.
However, we don't want to muddy up the `Game` type with user interaction.

We will need to remember which _phase_ the multi-step command is up to, _and_ the values that the player has entered so far.

```fsharp
type Phase = 
  | General
  | SelectingSourceStack       
  | SelectingNumCards of int            // remember the source stack number
  | SelectingTargetStack of (int * int) // remember the source stack and number of cards
                                        //   (Note: this is a tuple of two numbers)
```

We can then wrap the `Game` up into a container along with the command phase
```fsharp
type MultiPhaseGame = {
  game: Game
  phase: Phase
}
```

At each phase we need to accept a different set of command keystrokes, and allow the player to change their mind and back out of the previous choice.

```fsharp
let updateGameGeneral game keystroke = // our old `updateGame`, now one of possible update options
  match keystroke with 
  | 'd'   -> game |> applyUpdate DrawCards
  | 'm'   -> game |> nextPhase SelectingSourceStack
  | Number a when (a >= 1 && a <= 6) 
          -> game |> applyUpdate (TableToStack a)
  | _     -> game


let updateGame (game: MultiPhaseGame) keystroke : MultiPhaseGame =
  match game.phase with 
  | General -> 
      updateGameGeneral game keystroke
  | SelectingSourceStack -> 
      updateGameSourceStack game keystroke
  | SelectingNumCards sourceStack -> 
      updateGameNumCards sourceStack game keystroke
  | SelectingTargetStack (sourceStack, numCards) -> 
      updateGameTargetStack sourceStack numCards game keystroke
```

### Exercise: Updating the game in different phases

Implement the movement through the phases by updating the phase value of our game, and then actually doing the movement of the cards from one stack to another.

Don't forget that if we move the last face-up card in the stack, then we need to flip over the next face-down card (if there is one).

Here a couple of helper functions that may be useful
```fsharp
let private applyUpdate command multiPhaseGame =
  {
    multiPhaseGame with 
      game = multiPhaseGame.game|> applyCommand command
      phase = General   // all updates move the phase back to General
  }

let private nextPhase phase game = {game with phase = phase}
```

> TIP: Hiding and visibility  
> We can use the keyword `private` to say that the function cannot be seen or called from outside the _module_.
> Quite often this is used to allow us to break down the code into small chunks, but with the intention that
> only the _main_ functions for each module should be called.

[See an answer for moving through the phases]({{ site.baseurl }}{{ page.url }}#movePhases)

{:class="collapsible" id="movePhases"}
```fsharp
let private updateGameSourceStack game keystroke =
  match keystroke with 
  | Number stack when (stack >= 1 && stack <= 6) 
            -> game |> nextPhase (SelectingNumCards stack)
  | '\x1B'  -> game |> nextPhase General
  | _       -> game

let private updateGameNumCards sourceStack game keystroke =
  let numCardsInStack = 
    game.game.stacks[sourceStack - 1] 
    |> List.filter (fun a -> a.isFaceUp ) 
    |> List.length
  match keystroke with 
  | Number card -> game |> nextPhase (SelectingTargetStack (sourceStack, card))
  | 'a'         -> game |> nextPhase (SelectingTargetStack (sourceStack, numCardsInStack))
  | '\x1B'      -> game |> nextPhase SelectingSourceStack
  | _           -> game

let private updateGameTargetStack sourceStack numCards game keystroke =
  match keystroke with 
  | Number targetStack when (targetStack >= 1 && targetStack <= 6) -> 
           -> game 
              |> applyUpdate 
                (MoveCards {sourceStack=sourceStack; numCards=numCards; targetStack=targetStack})
  | '\x1B' -> game |> nextPhase (SelectingNumCards sourceStack)
  | _      -> game  

```

[See an answer for moving the cards]({{ site.baseurl }}{{ page.url }}#moveCards)

{:class="collapsible" id="moveCards"}
```fsharp
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


// The _external_ arguments for "MoveCards"
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
```

We also need to print out the commands that are acceptable for the command _phase_.  Try doing this also using a match on the game's _phase_.

[See an answer for showing the appropriate commands for the phase]({{ site.baseurl }}{{ page.url }}#printing)

{:class="collapsible" id="printing"}
```fsharp
let printCommands multiGame =
  match multiGame.phase with
  | General -> 
      printfn 
        "%s<d>raw cards, <1-6> put on stack, <m>ove cards between stacks <q>uit" 
        clearLine
  | SelectingSourceStack -> 
      printfn 
        "%sMove cards from stack ___(1-6), <esc> Go back, <q>uit" 
        clearLine
  | SelectingNumCards stack-> 
      let numCardsInStack = 
        multiGame.game.stacks[stack - 1] 
        |> List.filter (fun a -> a.isFaceUp ) 
        |> List.length
      printfn 
        "%sMove ___(1-%d, or <a>ll) cards from stack %d, <esc> Go back, <q>uit" 
        clearLine numCardsInStack stack
  | SelectingTargetStack (stack, card) -> 
      printfn 
        "%sMove %d cards from stack %d to stack ___, <esc> Go back, <q>uit" 
        clearLine card stack
  multiGame
```

{% include project-so-far.md %}