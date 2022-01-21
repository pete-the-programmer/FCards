---
title: Improving readability using matching and piping
concept: "Matching & Piping"
layout: default
---
## Matching

When picking up a card we used an `if` statement to test if the deck was empty...

```fsharp
{% include_relative src/ch6.pickup.fs %}
```

We could instead use a much more readable `match` statement.  A `match` statement is a very clean way of dealing with a set of possible inputs
that need to be treated in different ways.  It also helps reduce the number of bugs because the compiler will complain if we haven't 
specified a scenario for every possible value of the input.

```fsharp
{% include_relative src/ch7.pickup2.fs %}
```
Line by line in the `pickupCard` function: 
1. `match` on the `deck` value.
1. if it's empty, fail with an error message
1. if it has one item then return the hand with the item added to the end
1. if it has an item, and some more items, then return the hand with the _first_ item added to the end

> TIP: the compiler takes the first matching test from top to bottom. Something you need to be mindful of if you have two tests that could both possibly be true.

## Piping

The most useful "special" operator that I use in F# is the pipe operator `|>`

This operator takes whatever is on the left and "pipes" it into the function on the right as the _last_ parameter value.  
i.e `b |> func a` is the same as `func a b`

What this allows us to do is __chain__ functions together to create a bigger _composite_ function.
```fsharp
{% include_relative src/ch7.pipe.fs %}
```

With this knowledge we can slightly improve our `newDeck` calculation to
```fsharp
{% include_relative src/ch7.newdeck2.fs %}
```