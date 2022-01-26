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
