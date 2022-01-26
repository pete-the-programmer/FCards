---
slug: Displaying the stacks
concept: complex printing
chapter: "15"
part: "Solitaire"
---

Now that we have some stacks of cards, we can show them to the player.  From last chapter we want it to look something like:
```
| 1  |  2  |  3  |  4  |  5  |  6  |
[###] [###] [###] [###] [###] [♥A ]
[###] [###] [###] [###] [♥K ]     
[###] [###] [###] [♦8 ]          
[###] [###] [♠8 ]               
[###] [♠Q ]                    
[♠10]                         

Deck:  [###] - 31 cards remaining
```

### Exercise: 
Write the `printScreen` function that takes a `Game` and prints out the above.

[See an answer]({{ site.baseurl }}{{ page.url }}#printStacks)

{:class="collapsible" id="printStacks"}
```fsharp
let printScreen game = 
  let maxCardInAnyStack = 
    game.stacks 
    |> List.map (fun stack -> stack.Length )
    |> List.max
  printfn "| 1  |  2  |  3  |  4  |  5  |  6  |"
  [0..maxCardInAnyStack - 1]
    |> List.iter (fun cardNum ->
      [0..5]
      |> List.map (fun stackNum ->
          match game.stacks[stackNum].Length - cardNum with 
          | 1 -> 
            // the top-most card
            game.stacks[stackNum][cardNum]
            |> sprintf "[%O]"  // prints to a string
          | x when x > 1 -> 
            // a card in the stack, but not the top one
            "[###]"
          | _ -> 
            // Everything else (the stack is out of cards)
            "    "            
      )
      |> fun strings -> String.Join (" ", strings)
      |> printfn "%s"
  )
  //also print the remaining deck
  game.deck.Length |> printfn "\nDeck:  [###] - %d cards remaining"
```

{% include sofar.md %}


<!-- 
__Play loop__: 
  - Take 3 cards from the remainder of the `deck`, place them on the `table`, and display the top one only to the player
  - Player can move the top card on the `table` onto a selected `stack`.  This reveals the next card in the `table` until there are no more on the `table`
  - Player may also choose to move a subset of the top-most face-up cards in a `stack` to the top of another `stack`.  If this reveals a face-down card on a `stack`, then it is turned face-up.

```
========= Solitaire ==========
| 1 |  2 |  3 |  4 |  5 |  6 |
[##] [##] [##] [##] [##] [♠9]
[##] [##] [##] [##] [♣Q]
[##] [##] [##] [♠2]
[##] [##] [♥8]
[##] [♦K]
[♦4]
--space--
Table: [##][##][♦5]
Deck : [##] - 32 Cards remaining
<t>able cards, <1-6> put on stack, <q>uit
```

So that means we have more "rows" to display for our game:
- Header - _1_
- Stack number - _1_
- Stacks - _6_
- A spacer - _1_
- Table  - _1_
- Deck - _1_
- Prompt - _1_
- __Total: 12__ -->