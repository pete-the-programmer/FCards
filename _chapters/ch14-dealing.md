---
slug: A simple one-player game
concept: Many Hands
chapter: "14"
part: "Solitaire"
---

### Let's play a real game!

What's a popular single-player game that we can try - Solitaire, or course!

First we'll list out some interesting bits of the Solitaire game:
- Dealing
- The play loop
- Rules about which cards can go on other cards
- Aces and Kings have special moves
- End game

### Dealing
Let's start with dealing:
  - 6 `stacks` of cards
  - Increasing numbers of cards in each consecutive `stack`
  - Only the top card in each `stack` is face up
  - But the player can see all the cards in the `stack` (even if they are face down)

It may look something like:
```
| 1  |  2  |  3  |  4  |  5  |  6  |
[###] [###] [###] [###] [###] [♥A ]
[###] [###] [###] [###] [♥K ]     
[###] [###] [###] [♦8 ]          
[###] [###] [♠8 ]               
[###] [♠Q ]                    
[♠10]                         

Deck:  [###] - 31 cards remaining
```

### Exercise: Deal the cards out

Given that the model for our game would look like:
```fsharp
let Game = {
  deck: Card list
  table: Card list
  stacks: Card list list
}
```

Write the dealing function that populates the deck and the stacks, leaving the table empty.

[See an answer]({{ site.baseurl }}{{ page.url }}#deal)

{:class="collapsible" id="deal"}
```fsharp
let deal shuffledDeck = 
  let emptyGame = {
    deck = shuffledDeck
    table = []
    stacks = []
  }
              // a list of numbers from 6 to 1 (inclusive)
  [6..-1..1]  // stepping at -1 intervals (i.e. counting down)
  |>  List.fold (fun game i -> 
        {
          stacks = game.stacks @ [ game.deck |> List.take i ]
          deck = game.deck |> List.skip i
          table = []
        }
      
      ) emptyGame
``` 

{% include sofar.md %}


<!-- 
__Play loop__: 
  - Take 3 cards from the remainder of the `deck`, place them on the `table`, and display the top one only to the player
  - Player can move the top card on the `table` onto a selected `stack`.  This reveals the next card in the `table` until there are no more on the `table`
  - Player may also choose to move a subset of the top-most face-up cards in a `stack` to the top of another `stack`.  If this reveals a face-down card on a `stack`, then it is turned face-up.

```
========= Solitaire ==========
| 1 |  2 |  3 |  4 |  5 |  6 |
[##] [##] [##] [##] [##] [♠9]
[##] [##] [##] [##] [♣Q]
[##] [##] [##] [♠2]
[##] [##] [♥8]
[##] [♦K]
[♦4]
--space--
Table: [##][##][♦5]
Deck : [##] - 32 Cards remaining
<t>able cards, <1-6> put on stack, <q>uit
```

So that means we have more "rows" to display for our game:
- Header - _1_
- Stack number - _1_
- Stacks - _6_
- A spacer - _1_
- Table  - _1_
- Deck - _1_
- Prompt - _1_
- __Total: 12__ -->