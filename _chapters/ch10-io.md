---
title: Interacting with a player - game state
concept: I/O
layout: default
---
## Giving the player a way to _do_ things
So far we've been talking about modelling how the data can work and fit together, but that's no fun until a person 
can interact with the game.  To do this we need two things:
1. a way of displaying the state of the game (i.e. who has what cards, and where)
1. a way of commanding the game to make a move

## Making our types more human-friendly
If we print out a hand of cards using `ToString` it would look like:
```fsharp
{% include_relative src/ch10.printing.fs %}
```
> TIP: `String.Join()` is a built-in function that joins a sequence of things into a single string, separated by the first parameter.  It is in the _System_ namespace so you will need to `open System` to make the function available to you.  BTW, a list can automatically be converted into a sequence by the compiler - that's why it works here.

All types in F# have a built-in method `ToString()`, which converts the value into a string that we can use to print out the value.  But, all types in F# are also extendable/overridable using the keyword `with`.  So we can override how the value is converted into a string by overriding the `ToString()` method on our DU types.

```fsharp
{% include_relative src/ch10.printable.fs %}
```
> TIP:  The funny looking `/u1234` values are unicode codes (in hexadecimal) for the suit symbols from [here](https://www.alt-codes.net/suit-cards.php){:target="_blank"}

With this change our output will now look like
```fsharp
"[♥8] [♦10] [♣Q] [♠2] [Jok]"
```
