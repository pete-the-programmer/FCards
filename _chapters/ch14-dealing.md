---
slug: A simple one-player game
concept: Many Hands
chapter: "14"
part: "Solitaire"
feature: 
  - override
  - Sequence
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

Deck:  [[[[[[[[[[[[[[[[[[[[[[[[[###]
```
### Cards have two sides

Note that now our model for a card in a stack is a bit more complex.  Sometimes the card is face-up and sometimes it is face-down.  We should incorporate that into our model (and make it easier for our future selves to print-out)
```fsharp
type StackCard = {
  card: Card
  isFaceUp: bool
} with 
    override this.ToString() =
      if this.isFaceUp then
        this.card.ToString()
      else 
        "###"
```


### Exercise: Deal the cards out

Given that the model for our game would look like:
```fsharp
let Game = {
  deck: Card list
  table: Card list
  stacks: StackCard list list
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
  [6..-1..1]  // a sequence of numbers from 6 to 1 in steps of -1 (i.e. backwards)
  |>  List.fold (fun game i -> 
        let newStack = 
          game.deck 
          |> List.take i                        
          |> List.mapi           // flip the last card
            (fun n card -> { isFaceUp = (n = i - 1); card=card}) 
        {
          stacks = game.stacks @ [ newStack ]
          deck = game.deck |> List.skip i
          table = []
        }
      
      ) emptyGame

``` 

{% include sofar.md %}
