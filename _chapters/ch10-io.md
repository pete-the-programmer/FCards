---
slug: Interacting with a player - game state
concept: I/O
chapter: "10"
part: "Defining Cards"
feature: 
  - I/O
  - Character codes
  - Type extension
keyword:
  - override
  - printfn()
---
### Giving the player a way to _do_ things
So far we've been talking about modelling how the data can work and fit together, but that's no fun until a person 
can interact with the game.  To do this we need two things:
1. a way of displaying the state of the game (i.e. who has what cards, and where)
1. a way of commanding the game to make a move

### Making our types more human-friendly
> All types in F# have a built-in method `ToString()`, which converts the value into a string that we can use to print out the value. 
> Some functions that deal with strings will automatically call `ToString()` on values to get the string representation.

If we print out a hand of cards at the moment it would look like:
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

### Extending our types
> We can add new properties, methods, and functions to any types in __F#__ using the keyword `with`.  We can also _override_ any built-in functions that are already defined on the type.

So we can override how the value is converted into a string by overriding the `ToString()` method on our DU types, and our `Game`.

```fsharp
//COLOR CODES
let COLOR_DEFAULT = "\x1B[0m"
let COLOR_RED = "\x1B[91m"
let COLOR_BLACK = "\x1B[90m"

type CardNumber =
  ...
  with 
    override this.ToString() = 
      match this with 
      | Two -> "2 "
      | Three -> "3 "
      | Four -> "4 "
      | Five -> "5 "
      | Six -> "6 "
      | Seven -> "7 "
      | Eight -> "8 "
      | Nine -> "9 "
      | Ten -> "10" //(pesky 2-digit number!!)
      | Jack -> "J "
      | Queen -> "Q "
      | King -> "K "
      | Ace -> "A "

type Card = 
  ...
  with  
    override this.ToString() = 
      match this with 
      | Hearts x -> $"{COLOR_RED}\u2665{x}{COLOR_DEFAULT}"
      | Diamonds x -> $"{COLOR_RED}\u2666{x}{COLOR_DEFAULT}"
      | Clubs x ->  $"{COLOR_BLACK}\u2663{x}{COLOR_DEFAULT}"
      | Spades x ->  $"{COLOR_BLACK}\u2660{x}{COLOR_DEFAULT}"
      | Joker -> "Jok"

type Game = {
  ...
} with
    override this.ToString() =
      $"[###] - {this.deck.Length}\n" + (printOut this.hand)
```
> TIP:  The funny looking `/u1234` values are unicode codes (in hexadecimal) for the suit symbols from [here](https://www.alt-codes.net/suit-cards.php){:target="_blank"}

With this change our `printOut` function will now produce:
```fsharp
"[♥8] [♦10] [♣Q] [♠2] [Jok]"
```


{% include sofar.md %}