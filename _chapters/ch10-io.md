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
open System

let printOut (hand: 'a seq) =  "[" + String.Join("] [", hand) + "]"

[ Hearts Eight; Diamonds Ten; Clubs Queen; Spades Two; Joker ]
|> printOut
|> printfn "%s"

// OUTPUT //
"[Hearts Eight] [Diamonds Ten] [Clubs Queen] [Spades Two] [Joker]"
```
> TIP: `String.Join()` is a built-in function that joins a sequence of things into a single string, separated by the first parameter.  It is in the _System_ namespace so you will need to `open System` to make the function available to you.  BTW, a list can automatically be converted into a sequence by the compiler - that's why it works here.

All types in F# have a built-in method `ToString()`, which converts the value into a string that we can use to print out the value.  But, all types in F# are also extendable/overridable using the keyword `with`.  So we can override how the value is converted into a string by overriding the `ToString()` method on our DU types.

```fsharp
type CardNumber =
  | Two 
  | Three
  | Four
  | Five
  | Six
  | Seven
  | Eight
  | Nine
  | Ten
  | Jack
  | Queen
  | King
  | Ace
  with 
    override this.ToString() = 
      match this with 
      | Two -> "2"
      | Three -> "3"
      | Four -> "4"
      | Five -> "5"
      | Six -> "6"
      | Seven -> "7"
      | Eight -> "8"
      | Nine -> "9"
      | Ten -> "10"
      | Jack -> "J"
      | Queen -> "Q"
      | King -> "K"
      | Ace -> "A"

type Card = 
  | Hearts of CardNumber
  | Diamonds of CardNumber
  | Clubs of CardNumber
  | Spades of CardNumber
  | Joker
  with  
    override this.ToString() = 
      match this with 
      | Hearts x -> "\u2665" + x.ToString()
      | Diamonds x -> "\u2666" + x.ToString()
      | Clubs x -> "\u2663" + x.ToString()
      | Spades x -> "\u2660" + x.ToString()
      | Joker -> "Jok"

let myCard = Hearts Three
```
> TIP:  The funny looking `/u1234` values are unicode codes (in hexadecimal) for the suit symbols from [here](https://www.alt-codes.net/suit-cards.php){:target="_blank"}

With this change our output will now look like
```fsharp
"[♥8] [♦10] [♣Q] [♠2] [Jok]"
```
