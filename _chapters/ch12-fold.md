---
slug: Interacting with a player - making a move
concept: Fold
chapter: "12"
part: "Simple Pickup Game"
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

let combineUpdaterAndPrinter updater game command= 
  let updated = updater game command
  printScreen updated
  updated 

let looper (updater: Game -> char -> Game) (initialGame: Game) = 
  printScreen initialGame
  (fun _ -> Console.ReadKey().KeyChar |> Char.ToLowerInvariant)
  |> Seq.initInfinite
  |> Seq.takeWhile (fun x -> x <> 'q')
  |> Seq.fold (combineUpdaterAndPrinter updater) initialGame
```

### Fold
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
  looper updateGame startingpoint
```

{% include sofar.md %}