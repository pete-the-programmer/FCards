---
slug: A deck of cards
concept: Calculations
chapter: "05"
part: "Defining Cards"
feature: 
  - Collections
  - Tuples
keyword:
  - map()
---

### Creating a deck of cards
To pick up a card, we will need a deck of cards to pick from.

### Exercise:
Define a thing that has all the cards in the deck as a list

There are some useful list functions that are supplied in the [base libraries](https://fsharp.github.io/fsharp-core-docs/reference/fsharp-collections-listmodule.html){:target="_blank"} :
- __List.allPairs list1 list2__ - creates a new list with all the items in list1 combined with all the items in list 2 as a tuple.
A tuple is two items put together as a small grouping, and is typed as `(a, b)` - with a _comma_.  
- __List.map function list__ - transforms a list of items by running the function against each item in the list.  You end up with a list with the same length as the initial list, containing  the transformed items.

> For `List.map` you will need to define an inline function.  This takes the form  
> `(fun x -> [calculate something here using x])`. 
> 
> So for `List.map` you might do something like this:
> ```fsharp
>   let listOfTuples = [ (1,2) ; (3,4) ]
>   List.map (fun (a,b) -> a + b) listOfTuples
> ```

[See an answer]({{ site.baseurl }}{{ page.url }}#newdeck)

{:class="collapsible" id="newdeck"}
```fsharp
let newDeck = 
  // Note: this is a 'calculated value' as it takes no inputs.
  //  So, once this value is calculated the first time then it 
  //  just keeps the value forever.
  let suits = [Hearts; Diamonds; Clubs; Spades]
  let numbers = [
    Two; Three; Four; Five; Six; Seven; Eight; Nine; Ten;
    Jack; Queen; King; Ace
  ]
  let paired = List.allPairs suits numbers
  List.map (fun (suit, number) -> suit number) paired

```

{% include sofar.md %}