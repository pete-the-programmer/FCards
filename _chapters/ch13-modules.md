---
slug: Separation of core and our game
concept: Modules
chapter: "13"
part: "Simple Pickup Game"
---

### What's part of "pickup" and what is just generally Cards?

In the next part we want to make a more complex game.  In light of this, we can ask ourselves "What parts of our code are re-useable across different card games?"

| | |
|:--------------|---------:|
| CardNumber    | _Re-usable_                            |
| Card          | _Re-usable_                            |
| printOut(hand)| _Re-usable_                            |
| newDeck       | _Re-usable_                            |
| Game          |             other games may have many hands or face up cards               |
| pickupCard    |             only work for our definition of Game                |
| shuffle       | _Re-usable_                            |
| moveUpLines   | _Re-usable_                            |
| printScreen   |             only prints our game                |
| loopGame        | (loopGame will need to be tweaked to make it more general) _Re-usable_                           |
| updateGame    |             this is our specific game logic                |
| play          |             runs our game only                |

### Generics - Specifying a type like a variable

We would like to generalise the `loopGame()` function so that it can operate on _any_ kind of "Game", as it doesn't actually need to know any about the specifics of how the game works.  

> In __F#__, we can say that the type of something can be anything, as long as it's consistent using a _type parameter_.  A Type parameter is shown in __F#__ by starting with a single apostrophe before the name of the label - `'T`.  Usually, type parameters are a single capital letter, but they can be anything that starts with a letter (e.g. `'myAwesomeTypeParameter_Variable`).
>
> When we use a type parameter, we add the list of type parameters used in the function after the function name in angle brackets.  This gives users and the compiler a hint that this function is generic.
> ```fsharp
> let doAwesome<'A, 'B, 'C> (something: 'A) (another: 'A) (b:int) : 'C = ...
> ```
> This enforces that `something` and `another` _must_ have the same type, then followed by `b` that is an `int`, and the function returns a value of another type again.

So, we can modify our `loopGame` function to take any kind of Game object

```fsharp
// ORIGINAL
let loopGame (updater: Game -> char -> Game) (initialGame: Game) = 

//GENERIC
let loopGame<'G> (updater: 'G -> char -> 'G) (initialGame: 'G) = 
```

We also identified that the `printScreen` function is specific to our game, so we will need to supply it to the `loopGame`...
```fsharp
let loopGame<'G> (printScreen: 'G -> unit) (updater: 'G -> char -> 'G) (initialGame: 'G) = 
```
> TIP: __F#__ uses the type `unit` to mean a _nothing_.  Other languages can use the word _void_ or _null_.

### Excercise: 

Separate the general stuff into a _Core_ module, followed by a _Pickup_ module

{% include sofar.md %}