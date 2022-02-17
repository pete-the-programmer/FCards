---
slug: Interacting with a player - making a move
concept: Fold
chapter: "12"
part: "Simple Pickup Game"
feature: 
  - I/O
  - Character codes
  - Updating state
  - Partial Application
  - Currying
keyword:
  - fold()
---

### Listening to the player
To play a game, we can expand to a standard play loop:
1. Print out state of the game
2. Print out list of commands the player can perform (optional?)
4. Wait for player to type a command
5. Execute the command
6. Print out result of command (maybe - depends if the state is obvious enough)
7. Go back to 1.

So that means within our play loop we need to include a function that updates the game according to the command.
```fsharp
let moveUpLines n = printfn "\x1B[%dA" n  //moves the cursor up "n" lines

let printScreen game =
  printfn "===FCARDS==="
  printfn "%O" game
  printfn "<p>ickup card <q>uit"
  moveUpLines 5

let combineUpdaterAndPrinter updater game command = 
  let updated = updater game command
  printScreen updated
  updated 

let loopGame (updater: Game -> char -> Game) (initialGame: Game) = 
  printScreen initialGame
  (fun _ -> Console.ReadKey().KeyChar |> Char.ToLowerInvariant)
  |> Seq.initInfinite
  |> Seq.takeWhile (fun x -> x <> 'q')
  |> Seq.fold (combineUpdaterAndPrinter updater) initialGame
```

#### Fold
> One the last line we use a new standard function called `fold`.
> This loops through the collection of things (in this case the keystrokes) and applies them to an _accumulator_ (in our case it's the initial Game),
> but in each step in the loop it uses the updated Game from the previous step.
> It's a way of doing the following but with a collection of things that you may not know the value of yet (e.g. the player's chosen keystrokes):
> ```fsharp
> game
> |> updater 'p'
> |> updater 'p'
> |> updater 'r'
> |> updater 'a'
> |> updater '?'
> |> updater 'q'  //yay we quit!
> ```

#### Partial Application

Also, on that last line, we called the function `combineUpdaterAndPrinter`.  But the function has three inputs and we only provided one - what's going on?!?

> Functions in __F#__ are pretty cool.  They _actually_ only take one input at a time and then generate a sub-function for taking the next input, and so on, and so on.
> 
> So, the function "`let combineUpdaterAndPrinter updater game command = ...`" is actually
> ```fsharp
> let combineUpdaterAndPrinter updater =
>   let sub_function1 game =
>     let sub_function2 command =
>       ...
> ```
> That means when we only provided the first _updater_ input, it actually returned a function that takes a _game_ ( and _command_ ), which just happens to be the exact right shape for the `fold` operation.  In this way we can pre-bake a function into a desired shape with already setup values.
>
> TIP: To take advantage of this _partial application_ of functions, the order of the inputs is important.  You can only pre-bake from left to right.  Generally, this means that we list the inputs that change the least (or are considered configuration?) first, and the things that change the most (or get passed along to the next function in the chain of pipes?) go to the right.

> Extra TIP for experts:  Functions that aren't built in __F#__  (i.e. probably built in C#) _don't_ use partial application, and instead have a single _tuple_ of inputs.
> 
> You may have noticed this when we used the standard function `String.Join()` in {% include link-chapter.md chapter="10" %}. It takes a tuple of a separator and an array of strings
> - So it's `String.Join: (string * string array) -> string` called as `String.Join(", ", values)`
> - __Not__ as  `String.Join: string -> string array -> string`,   called as `String.Join ", " values`

### Exercise:

Design the `updater` function.  It takes the parameters of a `Game` and a `Char` and returns a newly updated `Game`.

We want it to pickup a card only if the player presses 'p', otherwise return the Game unchanged.

[See an answer]({{ site.baseurl }}{{ page.url }}#updateGame)

{:class="collapsible" id="updateGame"}
```fsharp
let updateGame game command = 
  match command with 
  | 'p' -> game |> pickupCard
  | _ -> game
```

Now we can play a simple game of "pick up the cards"!

```fsharp
let play() =
  let startingpoint = 
    {
      deck = newDeck |> shuffle
      hand = []
    }
  loopGame updateGame startingpoint
```

{% include sofar.md %}