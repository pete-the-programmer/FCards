---
title: Taking from the deck
concept: Combining into world view
layout: default
---
## Taking card from the deck when picking up a card
You may have noticed in [chapter 6]({% link _chapters/ch06-functions.md %}) that when we took a card, that top card was still in the deck.  The function returned a new updated hand but nothing else changed.
The problem is that functions only return _one_ thing, but the action of picking up a card changes _two_ things: the hand and the deck.

So, rather than passing in a hand and a deck separately, we should combine these two things into a single _Game_ thing.

### Exercise:
Modify the `pickupCard` function to use a combined `Game` thing, including defining the `Game` thing.

[See an answer]({{ site.baseurl }}{{ page.url }}#game)

{:class="collapsible" id="game"}
```fsharp
{% include_relative src/ch08.game.fs %}

```
> TIP: Prefer creating a record over a tuple, so that you can add names/labels to the parts for clarity - even if it is only two things.