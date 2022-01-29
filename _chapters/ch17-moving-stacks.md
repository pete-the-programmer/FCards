---
slug: Moving Stacks
concept: 2-phase moves
chapter: "17"
part: "Solitaire"
---

We can now make our stacks grow - let's look at what we've achieved and have yet to do:

{:class="naked"}
- [x] Print Screen
- [x] Get user input and make play loop
- [x] Draw cards to table
- [x] Place cards from table to deck
- [ ] Move cards from stack to stack
- [ ] Ace stacks
- [ ] King special move (start new stack)
- [ ] Rules about which card can go on another
- [ ] End game and winning!

### Moving cards from one stack to another

So far, all of our moves have been a single keystroke command. But when we move cards from one stack to another we need to tell the game a few things:
1. That we want to move a stack
2. Which stack to move
3. How many cards to move
4. Which stack to move the card into

This complicates our update loop, as now we will expect the input and gameplay to come in multiple command _phases_.
We will need to remember which _phase_ the gameplay is up to, _and_ the values that the player has entered so far.

To do this we can add a another part to the `Game`.

```fsharp
type Phase = 
  | General
  | SelectingSourceStack
                        //remember the source stack number
  | SelectingSourceCard of int  
                         // remember the source stack and card
                         // (this is a tuple of two numbers)
  | SelectingTargetStack of (int * int) 

type Game = {
  ...
  phase: Phase
}
```

At each phase we need to accept a different set of command keystrokes, and allow the player to change their mind and back out of the previous choice.

```fsharp
let updateGameGeneral game command =  // our old `updateGame` called below now.
  match command with 
  | 'd' -> drawCards game
  | Number a when (a >= 1 && a <= 6) -> tableToStack (a - 1) game
  | 'm' -> 
      // Go to next phase and wait
      { game with 
        phase = SelectingSourceStack 
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
```

### Exercise: Updating the game in different phases

Implement the movement through the phases by updating the phase value of our game, and then actually doing the movement of the cards from one stack to another.

[See an answer for moving through the phases]({{ site.baseurl }}{{ page.url }}#movePhases)

{:class="collapsible" id="movePhases"}
```fsharp
let updateGameSourceStack game command =
  match command with 
  | Number sourceStack when (sourceStack >= 1 && sourceStack <= 6) -> 
      { game with 
          phase = SelectingSourceCard sourceStack
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
        moveCardsBetweenStacks 
          sourceStack 
          sourceCard 
          targetStack 
          game
      { updatedGame with 
          phase = General
      }
  | '\x1B' -> // [esc] key
      { game with 
          phase = SelectingTargetStack (sourceStack, sourceCard)
      }    
  | _ -> game  
```

[See an answer for moving the cards]({{ site.baseurl }}{{ page.url }}#moveCards)

{:class="collapsible" id="moveCards"}
```fsharp
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
```

We also need to print out the commands that are acceptable for the command _phase_.  Try doing this also using a match on the game's _phase_.

[See an answer for showing the appropriate commands for the phase]({{ site.baseurl }}{{ page.url }}#printing)

{:class="collapsible" id="printing"}
```fsharp
let printCommands game =
  match game.phase with
  | General -> 
    printfn 
      "%s<d>raw cards, <1-6> put on stack, <m>ove between stacks, <q>uit" 
      clearLine
  | SelectingSourceStack -> 
    printfn 
      "%s<1-6> select source stack to move from, <esc> Go back, <q>uit" 
      clearLine
  | SelectingSourceCard stack-> 
    let numCardsInStack = game.stacks[stack - 1].Length
    printfn 
      "%sMove from stack %d at card ___(1-%d), <esc> Go back, <q>uit"
      clearLine
      stack
      numCardsInStack
  | SelectingTargetStack (stack, card) -> 
    printfn 
      "%sMove from stack %d at card %d to stack __, <esc> Go back, <q>uit"
      clearLine 
      stack 
      card
  // return the game for the next in line
  game
  ```

{% include sofar.md %}