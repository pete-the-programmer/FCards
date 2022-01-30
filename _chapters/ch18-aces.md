---
slug: Aces
concept: more phases
chapter: "18"
part: "Solitaire"
---

We can now make our stacks grow - let's look at what we've achieved and have yet to do:

{:class="naked"}
- [x] Print Screen
- [x] Get user input and make play loop
- [x] Draw cards to table
- [x] Place cards from table to deck
- [x] Move cards from stack to stack
- [ ] Ace stacks
- [ ] King special move (start new stack)
- [ ] Rules about which card can go on another
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
```



### Exercise: Add to Ace stacks

Add the update function `updateGameAceSource` to move cards to the Ace stacks. 

Also, include a change to the `printCommands` functions to keep the player informed.  

> TIP: The ace stacks may be long and should change the spacing.  Extract a helper method to calculate the height:
> ```fsharp
> let maxCardInAnyStack game = 
>   let maxCardInStacks = 
>     game.stacks 
>     |> List.map (fun stack -> stack.Length )
>     |> List.max
>   let maxCardInAces = 
>     game.aces 
>     |> List.map (fun stack -> stack.Length )
>     |> List.max
>   Math.Max(maxCardInAces, maxCardInStacks)
> ```

[See an answer]({{ site.baseurl }}{{ page.url }}#movingAces)

{:class="collapsible" id="movingAces"}
```fsharp
let printCommands game =
  match game.phase with
  ...
  | SelectingAceSource ->
      printfn 
        "%s<1-6> select source stack to move from, <t>abled card to ace stack, <esc> Go back, <q>uit" 
        clearLine    
```

{% include sofar.md %}