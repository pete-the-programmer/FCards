---
slug: Tabling/Placing cards
concept: 
chapter: "16"
part: "Solitaire"
---

Now that we can see the cards let's give the player some actions.

#### Initial play loop
Let's allow the player to draw some cards out of the deck, and put a card on a stack:
  - Take 3 cards from the remainder of the `deck`, place them on the `table`, and display the top one only to the player
  - Player can move the top card on the `table` onto a selected `stack`.  This reveals the next card in the `table` until there are no more on the `table`

So now our print screen includes the commands and the tabled cards:
```
============ Solitaire =============
| 1 |  2 |  3 |  4 |  5 |  6 |
[##] [##] [##] [##] [##] [♠9]
[##] [##] [##] [##] [♣Q]
[##] [##] [##] [♠2]
[##] [##] [♥8]
[##] [♦K]
[♦4]
--space--
Table: [[[♦5]
Deck : [[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[##]
<d>raw cards, <1-6> put on stack, <q>uit
```

### Matching a stack number
In order to match the input of the stack we not only have to check that the character typed is a valid number, but also that it is in the range of 1 to 6.

__F#__ has a very useful tool called [__Active Patterns__](https://fsharpforfunandprofit.com/posts/convenience-active-patterns/) to help with the first part:

> An _Active Pattern_ is a dynamic pattern matching tool that either returns `None` for a non-match, or `Some(a)` for a match that is also parsed for us.
> 
> The following example takes a `Char` and uses the library function `GetNumericValue` to parse the char.  A _-1.0_ indicates that the value isn't a number, and so returns `None`.  A successful conversion is forced into being an `int` (a whole number) and returned as a `Some`
> ```fsharp
> let (|Number|_|) (ch:Char) =
>   match Char.GetNumericValue(ch) with
>   | -1.0 -> None
>   | a -> a |> int |> Some
> ```

We can use the above active pattern to make our `updateGame` function:
```fsharp
  let updateGame game command =
    match command with 
    | 'd' -> drawCards game
    | Number a when (a >= 1 && a <= 6) -> tableToStack a game
    | _ -> game
```
Note how we can specify a range in our matcher using the `when` keyword.

### Exercise
Include the extra parts of the `printScreen` function, and don't forget to move the cursor up to the top of the screen.

> TIP: You may need to clear a line that has gotten shorter.  Here's a value to print to do that:
>
>  `let clearLine = "\x1B[K"`
 

_Also_, write the `drawCards` and `tableToStack` functions to update the `Game` object.

[See an answer for printing]({{ site.baseurl }}{{ page.url }}#printStacks)

{:class="collapsible" id="printStacks"}
```fsharp
  let printHeader game =
    printfn "============ Solitaire ============="
    game

  let printStacks game = 
    let maxCardInAnyStack = 
      game.stacks 
      |> List.map (fun stack -> stack.Length )
      |> List.max
    printfn "%s| 1  |  2  |  3  |  4  |  5  |  6  |" clearLine
    [0..maxCardInAnyStack - 1]
      |> List.iter (fun cardNum ->
        [0..5]
        |> List.map (fun stackNum ->
            if game.stacks[stackNum].Length > cardNum then 
              game.stacks[stackNum][cardNum]
              |> sprintf "[%O]"
            else
              // the stack is out of cards
              "     "         
        )
        |> fun strings -> String.Join (" ", strings)
        |> printfn "%s%s" clearLine
    )
    game //pass it on to the next function
  
  let printTable game =
    let tableLine = 
      match game.table with 
      | []  -> ""
      | [a] -> game.table.Head.ToString()
      | more -> 
        String.init game.table.Length (fun _ -> "[")
        + game.table.Head.ToString()
    printfn "\nTable: %s]" tableLine
    game

  let printDeck game =
    String.init game.deck.Length (fun _ -> "[") 
      |> printfn "Deck:  %s###]"
    game

  let printCommands game =
    printfn "<d>raw cards, <1-6> put on stack, <q>uit"
    game

  let printMoveToTop game =
    let maxCardInAnyStack = 
      game.stacks 
      |> List.map (fun stack -> stack.Length )
      |> List.max
    let n = 
      1 //header
      + 1 //stack numbers
      + maxCardInAnyStack //stacks
      + 1 //spacer
      + 1 //table
      + 1 //deck
      + 1 //commands
      + 1 //current line
    moveUpLines n

  let printScreen game = 
    game 
    |> printHeader
    |> printStacks
    |> printTable
    |> printDeck
    |> printCommands
    |> printMoveToTop
```

[See an answer for updating the game]({{ site.baseurl }}{{ page.url }}#game)

{:class="collapsible" id="game"}
```fsharp
let drawCards game =
  let withEnoughCardsToDraw =
    match game.deck.Length with
    | n when n < 3 -> 
      // create a new game that's just like the old one
      //  but with the following differences.
      //  (really useful if you have a lot of parts but only want to change a couple)
      {game with  
        deck = game.deck @ game.table
        table = []
      }
    | _ -> game
  // in case there is less than 3 remaining
  let cardsToTake = Math.Min(3, withEnoughCardsToDraw.deck.Length)  
  {withEnoughCardsToDraw with
    table = 
      (withEnoughCardsToDraw.deck |> List.take cardsToTake)
      @ withEnoughCardsToDraw.table //(new cards on top)
    deck = withEnoughCardsToDraw.deck |> List.skip cardsToTake
  }


// a helper to add a card to a numbered stack
let addToStack (stackNum:int) (card:Card) (stacks: StackCard list list) =
  let updatedStack = stacks[stackNum] @ [ stackCard true card ]
  stacks |> List.updateAt stackNum updatedStack

let tableToStack stackNum game =
  match game.table with 
  | [] -> game // do nothing
  | [a] -> 
    {game with 
      table = []; 
      stacks = game.stacks |> addToStack stackNum a 
    }
  | a::rest -> 
    {game with 
      table = rest; 
      stacks = game.stacks |> addToStack stackNum a 
    }
```

{% include sofar.md %}
