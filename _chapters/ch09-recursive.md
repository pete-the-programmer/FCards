---
slug: Shuffling
concept: Recursive functions
chapter: "09"
part: "Defining Cards"
---
### Shuffling the deck
So far we have been using a deck that is always in the same predictable order.  It's time we mixed things up a bit and shuffled the cards.

### Recursive functions
> A _recursive_ function is a special kind of function that can call itself.
> This is useful for when we want to dive down into a collection of items and get to an end of some sort and bubble back up to the beginning.
> 
> In __F#__ we need to mark a function as being recursive with the `rec` keyword, to help the compiler with its optimisation.
> 
> For example we can add up numbers in a list we can add the first in the list to whatever the sum of the rest of the list is, 
> which we can get by calling the same function on the rest of the list...
> 
> ```fsharp
> let rec addAll (numbers: int list) =
>   match numbers with 
>   | [] -> 0
>   | [a] -> a
>   | a::rest -> a + (addAll rest)
> 
> addAll [1; 2; 3; 4; 5; 6; 7; 8; 9]  // returns 45
> ```

### Exercise: Shuffle a deck
Write a function that shuffles a list of cards by taking a random card from the list and joining that to a list of random cards from the rest of the list.

> TIP: To get a random number you create a randomiser using `let rrr = System.Random()`  
>      Then call it as `let randomValue = rrr.Next(20)` to get a random number from 0 to 19.

[See an answer]({{ site.baseurl }}{{ page.url }}#shuffle)

{:class="collapsible" id="shuffle"}
```fsharp
let rec shuffle deck = 
  let random = System.Random()
  match deck with 
  | [] -> []
  | [a] -> [a]
  | _ -> // NOTE: `_` means "some value, but I don't care what it is"
      let randomPosition = random.Next(deck.Length)
      let cardAtPosition = deck[randomPosition]
      let rest = deck |> List.removeAt randomPosition
      [cardAtPosition] @ (shuffle rest)
```

### Putting it together

So now we can create a new game with a shuffled deck and pickup some cards!

```fsharp
{
  deck = newDeck |> shuffle
  hand = []
}
|> pickupCard
|> pickupCard
|> pickupCard
|> pickupCard
|> pickupCard

// Result:
Game {
  deck: Card list ( x 47 )
  hand: Card list ( x 5 )
}

```


{% include sofar.md %}