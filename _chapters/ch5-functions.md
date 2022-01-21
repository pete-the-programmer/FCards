---
title: Picking up a card
concept: Functions
layout: default
---
## Defining a function
In __F#__ a function is just another kind of variable, but one that is _derived_ from another value(s).  So we use the `let` keyword.

```fsharp
let doSomething a b = 
  a + b
```
Note that the values we pass in could also be functions too

```fsharp
{% include_relative src/ch5.functions.fs %}
```
Note that there is no _return_ keyword.  The last result calculate is the value that is returned

## Creating a deck of cards
To pick up a card, we will need a deck of cards to pick from.

### Exercise:
Define a thing that has all the cards in the deck as a list

There are some useful list functions that are supplied in the [base libraries](https://fsharp.github.io/fsharp-core-docs/reference/fsharp-collections-listmodule.html){:target="_blank"} :
- __List.allPairs list1 list2__ - creates a new list with all the items in list1 combined with all the items in list 2 as a tuple.
A tuple is two items put together as a small grouping, and is typed as `(a, b)` - with a _comma_.  
- __List.map function list__ - transforms a list of items by running the function against each item in the list.  You end up with a list with the same length as the initial list, containing  the transformed items.

[See an answer]({{ site.baseurl }}{{ page.url }}#newdeck)

{:class="collapsible" id="newdeck"}
```fsharp
{% include_relative src/ch5.newdeck.fs %}
```

## How to pick up a card

Now that we have a deck, we need to define a function that takes the top card from the deck and puts it in our hand.

[See an answer]({{ site.baseurl }}{{ page.url }}#pickup)

{:class="collapsible" id="pickup"}
```fsharp
{% include_relative src/ch5.pickup.fs %}

"""
Note that we can be more specific about the types of the function's inputs as `(label: type)`.  If we don't do this the compiler tries to figure it out.  Most of the time the compiler's pretty good at that.
"""
```