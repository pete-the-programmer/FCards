---
slug: A hand of cards
concept: Collections
chapter: "04"
part: "Defining Cards"
feature: 
  - Collections
  - List
  - Sequence
  - Array
  - Set
---
### Lists, sets, sequences, and arrays
There are a bunch of different kinds of thing that can hold a collection of other things.

> A __list__ and __array__ are pretty similar and is like a checklist of stuff, in that they are in order and we can add to the bottom or insert an item into the middle whenever we like.
> 
> A __sequence__ is like some things coming off an assembly line.  We don't know how many they are (it could be infinite!!) and we take them as they come.
> 
> A __set__ is more like a bag of things where no two things in the bag can have the same value.

We'll generally use a __List__ for our collections of things because it has some useful operators to join and split the list:
- __@__ joins two lists - e.g. `[1; 2; 3] @ [8; 9; 10] = [1; 2; 3; 8; 9; 10]`
- __[]__ is a empty list
- __[a]__ is a list with one thing in it
- __a::b__ is a list where `a` is the first thing in the list and `b` is all the rest.  Really useful when you're processing each thing in the list in turn.

### A hand of cards

A hand of cards is just a list of `Card` things
```fsharp
let hand = [Hearts Three; Diamonds Ten; Clubs King; Joker]
```


{% include sofar.md %}