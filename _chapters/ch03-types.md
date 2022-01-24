---
slug: Describing what a card is
concept: Types
chapter: "03"
---
## Records
> A _record_ is a type of thing that has multiple parts to its value, like a name, or id, or a number.
> 
> Each of these parts has a label and a type, which itself can even be another _record_ type.  Traditionally, types of things (like a record definition) start with a capital letter, and instances of these things (e.g a variable) start with a small letter.
>
> Parts of a type are separated with a semicolon (;) or are put on separate lines at the next level of indentation

So, we can define a playing Card as having a couple of parts - the suit and the number value

```fsharp
type Card = {
  suit: string
  value: int
}

let myCard = { suit = "Clubs"; value=3 }
```



## Discriminated Union 
> A _discriminated union_ (DU for short) is a type of thing that can be either of its parts, but __only one at a time__.
 
For instance a DU for the suits of a deck of cards would be...

```fsharp
type Suit = 
  | Hearts
  | Diamonds
  | Clubs 
  | Spades

type Card = {
  suit: Suit
  value: int
}

let myCard = { suit = Clubs; value=3 }
```

A more complex form of a DU can specify a type for each of the parts.  The types of the parts don't have to be the same.
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

type Card = 
  | Hearts of CardNumber
  | Diamonds of CardNumber
  | Clubs of CardNumber
  | Spades of CardNumber
  | Joker



let myCard = Hearts Three
```
> TIP: The labels for a DU _have_ to start with a capital letter, so we can't just use the number for the label directly.


{% include sofar.md %}