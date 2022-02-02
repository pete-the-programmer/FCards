---
slug: Improving readability using matching and piping
concept: "Matching & Piping"
chapter: "07"
part: "Defining Cards"
feature: 
  - match
  - Piping (|>)
---

### Matching

> A `match` statement is a very clean way of dealing with a set of possible inputs 
> that need to be treated in different ways.  It also helps reduce the number of bugs because the compiler will complain if we haven't 
> specified a scenario for every possible value of the input.

When picking up a card we used an `if` statement to test if the deck was empty.  We could instead use a much more readable `match` statement.  

```fsharp
// ORIGINAL

let pickupCard (hand: Card list) (deck: Card list) =
  if deck.Length = 0 then 
    failwith "No cards left!!"
  else
    let topcard = deck[0]
    hand @ [topcard]

// WITH ~~ MATCH ~~

let pickupCard (hand: Card list) (deck: Card list) =
  match deck with 
  | [] -> failwith "No cards left!!!"
  | [a] -> hand @ [a]
  | a::rest -> hand @ [a]
```
Line by line in the `pickupCard` function: 
1. `match` on the `deck` value.
1. if it's empty, fail with an error message
1. if it has one item then return the hand with the item added to the end
1. if it has an item, and some more items, then return the hand with the _first_ item added to the end

> TIP: the compiler takes the first matching test from top to bottom. Something you need to be mindful of if you have two tests that could both possibly be true.

### Piping

The most useful "special" operator that I use in F# is the pipe operator `|>`

> The pipe operator takes whatever is on the left and "pipes" it into the function on the right as the _last_ parameter value.
> i.e `b |> func a` is the same as `func a b`
> 
> What this allows us to do is __chain__ functions together to create a bigger _composite_ function.
> ```fsharp
> let add a b = a + b
> 
> let multiply a b = a * b
> 
> let chained = 
>   7                 // 7
>   |> add 4          // 11
>   |> mutiply 6      // 66
>   |> add 2          // 68 
> ```

With this knowledge we can slightly improve our `newDeck` calculation to
```fsharp
let newDeck = 
  let suits = [Hearts; Diamonds; Clubs; Spades]
  let numbers = [
    Two; Three; Four; Five; Six; Seven; Eight; Nine; Ten;
    Jack; Queen; King; Ace
  ]

  List.allPairs suits numbers
  |> List.map (fun (suit, number) -> suit number)

```

{% include sofar.md %}