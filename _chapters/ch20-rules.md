---
slug: Making some rules
concept: happy path rejection
chapter: "20"
part: "Solitaire"
feature: 
  - match
  - when
  - Type extension
  - Active patterns
---

So far we've concentrated on the mechanics of enabling the player interaction.

### Saying no

Now it's time to actually implement some rules of the game.  In order to do this we may have to tell the player (gasp!) __NO__.

When the player makes a move that is not allowed we can make a beeping sound by printing the code:
```fsharp
  let bell = "\x07" //  The common ascii code for a "bell" sound (i.e. beep)
  ...
  printf "%s" bell
```

### Rule: Stacked Cards

1. must be the opposite colour to the bottom face-up card
1. must be the next lower number in sequence

One of the important decisions we need to make is _where_ in the in process do we enforce the rules.

I prefer to leave the functions that actually _do_ the activities (e.g. `moveCardsBetweenStacks`, `moveToAceFromStack`, etc) to be completely ignorant of higher-level rules.  That pushes the rule logic up a level to the various `updateGame...` functions.  These are the functions that deal with the input from the player and decide what to do with it. So let's update these functions to be a bit more sophisticated:

```fsharp
let updateGameGeneral game command =
  match command with 
  | Number a 
      when (a >= 1 && a <= 6) 
      &&   canAddToStack (game.stacks[a - 1]) (game.table.Head)
      -> 
        tableToStack (a - 1) game
  ...     
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
  ...  
  | _ -> 
    printf "%s" bell // make a noise for an unacceptable input
    game

let updateAceSourceStack game command =
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
  ...
  | _ -> 
    printf "%s" bell // make a noise for an unacceptable input
    game    
```

Note the new functions `canAddToAceFromStack`, `canAddToAce`, `canAddToStack`, and `canMoveCardsBetweenStacks` are used as part of the matcher's _when_ clause.  We've also added a beep/bell if nothing matches.


#### Making life easy for ourselves
> We can extend the model a bit to help us find out these things
> 
> ```fsharp
> type Card with
>   member this.Number =
>     match this with 
>     | Hearts   a    
>     | Diamonds a  
>     | Clubs    a     
>     | Spades   a -> a  // can all resolve to these same `a`, because all the DU parts are the same type
>     | Joker      -> failwith "Joker?!?!?"
> ```
> 
> ```fsharp
> type CardNumber with
>   member this.Ordinal = // i.e. the numerical order
>     match this with 
>     | Ace   -> 1 
>     | Two   -> 2
>     | Three -> 3 
>     | Four  -> 4 
>     | Five  -> 5 
>     | Six   -> 6 
>     | Seven -> 7 
>     | Eight -> 8 
>     | Nine  -> 9 
>     | Ten   -> 10
>     | Jack  -> 11
>     | Queen -> 12 
>     | King  -> 13
> ```
> 
> ... and use a couple of active patterns to deal with colours
> ```fsharp
> let (|IsRed|_|) (card:Card) =
>   match card with 
>   | Hearts _
>   | Diamonds _ -> Some card // pass the card on
>   | _          -> None      // end of the road
> 
> let (|IsBlack|_|) (card:Card) =
>   match card with 
>   | IsRed _ -> None      // end of the road
>   | _       -> Some card // pass the card on
> ```

#### Exercise: Rules for adding to stacks

Write the functions `canAddToStack` and `canMoveCardsBetweenStacks` that takes the inputs as used in the update code above.


[See an answer]({{ site.baseurl }}{{ page.url }}#canAdd)

{:class="collapsible" id="canAdd"}
```fsharp
let canAddToStack (stack: StackCard list) (card:Card) =
  if stack = [] && card.Number = King then   // BONUS! we can tick this off too
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
```
#### Rule: Ace Cards

1. must be right suit
1. must be the next higher number in sequence

We can update the update functions for Ace stacks in a similar way

```fsharp
let updateAceSourceStack game command =
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
  ...
  | _ -> 
    printf "%s" bell // make a noise for an unacceptable input
    game  
```

#### Exercise: Rules for adding to ace stacks

Write the functions `canAddToAceFromStack` and `canAddToAce` that takes the inputs as used in the update code above.

[See an answer]({{ site.baseurl }}{{ page.url }}#canAddAces)

{:class="collapsible" id="canAddAces"}
```fsharp
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
```

{% include project-so-far.md %}
