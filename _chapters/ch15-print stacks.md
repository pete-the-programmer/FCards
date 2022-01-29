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
            "     "            
      )
      |> fun strings -> String.Join (" ", strings)
      |> printfn "%s"
  )
  //also print the remaining deck
  game.deck.Length |> printfn "\nDeck:  [###] - %d cards remaining"
```

{% include sofar.md %}
