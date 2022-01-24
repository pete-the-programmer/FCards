---
slug: Picking up a card
concept: Functions
chapter: "06"
---
## Picking up a card

Now that we have a deck, we can pick a card up and add it to our hand

## Defining a function
In __F#__ a function is just another kind of variable, but one that is _derived_ from another value(s), so we use the `let` keyword.  Also, as it's just a kind of value, they can be passed into other functions too.

```fsharp
let add a b = a + b

let multiply a b = a * b

let combine combiner a b =
  combiner (a + 1) (b * 2)

// to use it...
let added = combine add 3 4  // should equal 12
let multiplied = combine multiply 3 4  // should equal 32
```
> TIP: there is no _return_ keyword.  The last result calculated is the value that is returned

### Exercise:

Define a function that takes the top card from the deck and puts it in our hand.

[See an answer]({{ site.baseurl }}{{ page.url }}#pickup)

{:class="collapsible" id="pickup"}
```fsharp
let pickupCard (hand: Card list) (deck: Card list) =
  if deck.Length = 0 then 
    failwith "No cards left!!"
  else
    let topcard = deck[0]
    hand @ [topcard]

let hand = [Hearts Three; Diamonds Ten; Clubs King]
let updatedHand = pickupCard hand aNewDeck

"""
Note that we can be more specific about the types of the function's inputs as `(label: type)`.  If we don't do this the compiler tries to figure it out.  Most of the time the compiler's pretty good at that.
"""
```

{% include sofar.md %}