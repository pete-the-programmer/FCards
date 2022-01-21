---
title: Picking up a card
concept: Functions
layout: default
---
## Picking up a card

Now that we have a deck, we can pick a card up and add it to our hand

## Defining a function
In __F#__ a function is just another kind of variable, but one that is _derived_ from another value(s), so we use the `let` keyword.

```fsharp
let doSomething a b = 
  a + b
```
> TIP: the values we pass in could also be functions too

```fsharp
{% include_relative src/ch06.functions.fs %}
```
> TIP: there is no _return_ keyword.  The last result calculated is the value that is returned

### Exercise:

Define a function that takes the top card from the deck and puts it in our hand.

[See an answer]({{ site.baseurl }}{{ page.url }}#pickup)

{:class="collapsible" id="pickup"}
```fsharp
{% include_relative src/ch06.pickup.fs %}

"""
Note that we can be more specific about the types of the function's inputs as `(label: type)`.  If we don't do this the compiler tries to figure it out.  Most of the time the compiler's pretty good at that.
"""
```