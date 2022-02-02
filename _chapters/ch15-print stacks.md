---
slug: Displaying the stacks
concept: complex printing
chapter: "15"
part: "Solitaire"
feature: 
  - I/O
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

Deck:  [[[[[[[[[[[[[[[[[[[[[###]
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
          if game.stacks[stackNum].Length > cardNum then 
            game.stacks[stackNum][cardNum]
            |> sprintf "[%O]"
          else
            // the stack is out of cards
            "     "            
      )
      |> fun strings -> String.Join (" ", strings)
      |> printfn "%s"
  )
  //also print the remaining deck
  String.init game.deck.Length (fun _ -> "[")  
  |> printfn "\nDeck:  %s###]"
```

{% include sofar.md %}
