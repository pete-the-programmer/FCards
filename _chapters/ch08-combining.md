---
slug: Taking from the deck
concept: Combining into world view
chapter: "08"
part: "Defining Cards"
feature: 
---
### Taking card from the deck when picking up a card
You may have noticed in [chapter 6]({% link _chapters/ch06-functions.md %}) that when we took a card, that top card was still in the deck.  The function returned a new updated hand but nothing else changed.
The problem is that functions only return _one_ thing, but the action of picking up a card changes _two_ things: the hand and the deck.

So, rather than passing in a hand and a deck separately, we should combine these two things into a single _Game_ thing.

### Exercise:


Modify the `pickupCard` function to use a combined `Game` thing, including defining the `Game` thing
[(hint)]({{ site.baseurl }}{{ page.url }}#hint)

{:class="collapsible" id="hint"}
> HINT: Define the Game as a record with a hand and a deck

> TIP: I generally Prefer creating a record over a tuple, so that you can add names/labels to the parts for clarity - even if it is only two things.

[See an answer]({{ site.baseurl }}{{ page.url }}#game)

{:class="collapsible" id="game"}
```fsharp
type Game = {
  deck: Card list
  hand: Card list
}

let pickupCard (game: Game) =
  match game.deck with 
  | [] -> failwith "No cards left!!!"
  | [a] -> 
      {
        hand = game.hand @ [a]
        deck = []
      }
  | a::rest -> 
      {
        hand = game.hand @ [a]
        deck = rest
      }

let after3Pickups = 
  {
    hand = []
    deck = newDeck
  }
  |> pickupCard
  |> pickupCard
  |> pickupCard


//  result
  {
    hand = [ Hearts Two; Hearts Three; Hearts Four ]
    deck = [ Hearts Five; .... ]
  }

```


{% include sofar.md %}