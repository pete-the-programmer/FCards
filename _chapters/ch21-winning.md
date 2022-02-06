---
slug: Winning Solitaire
concept: End state detection
chapter: "21"
part: "Solitaire"
feature: 
  - Piping
keyword:
  - "|>"
---

That's it - we're finished!

All we need to do is celebrate winning - once we've figured out that the player has won, that is.

### Detecting that the player has won

> __Rule__: When all the cards are in the Ace stacks, then the player has won

From this rule we can say that we can only win after the action to move a card into the ace stack.
Therefore, we only need to check if the player has won in this single place.

```fsharp
let updateAceSourceStack game command =
  let updatedGame = 
    match command with 
    | Number sourceStack 
        when (sourceStack >= 1 && sourceStack <= 6) 
        &&   canAddToAceFromStack sourceStack game
        -> 
          let updatedGame = moveToAceFromStack sourceStack game
          { updatedGame with phase = General }
    | 't' when canAddToAce game.table game ->
        let updatedGame = moveToAceFromTable game
        { updatedGame with phase = General }
    | '\x1B' -> // [esc] key
        { game with phase = General }    
    | _ -> 
      printf "%s" bell // make a noise for an unacceptable input
      game
  // check if the player has won the game after this update
  { updatedGame with phase = if hasWon updatedGame then PlayerHasWon else updatedGame.phase }
```

As you can see, we will need a new state `PlayerHasWon`, which will need the associated `updateGame...` function and a matched line in the `printCommands` function.  An advantage of use a _DU_ is that the compiler can immediately tell you where you're not handling one of the cases.

#### Exercise: Detect player success

Write the `hasWon` function that counts the cards in the Ace stacks and checks if they add up to 52.

[See an answer]({{ site.baseurl }}{{ page.url }}#haswon)

{:class="collapsible" id="haswon"}
```fsharp
let hasWon game =
  game.aces 
  |> List.map List.length
  |> List.sum
  |> (=) 52   // Shortcut: 
              //  It means that we use `=` as a function 
              //  with 52 as the first input
              //  and the piped value as the second input
```

### Tidying up

I'm going to move some of the extensions we added to make the rules work into the `Cards` module, such as `IsRed/Black` and the type extensions that 
got us a number etc.  I'm doing this because in my mind they feel like common "Card" things, rather than something particular to do with Solitaire.

The other thing I'm going to tidy up is the special codes for printing in colour.  This is specific to printing on a terminal, so I will move this work into `printing.fs` as a function that transforms a printed card into a coloured, printed card.

### Celebration

How would you change the code to celebrate the player's success.  A message on the playing surface / fireworks / even an animation!

When the celebration is over, we need to decide how to end.  I have chosen to ask the player if they want to play another game; if they say yes then shuffle and deal a new game out and continue on, or allow them to quit.

{% include project-so-far.md %}
