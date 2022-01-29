#load "ch13_core.fsx"
open Ch13_core.Core

module Solitaire =
  open System

  type StackCard = {
    card: Card
    isFaceUp: bool
  } with 
      override this.ToString() =
        if this.isFaceUp then
          this.card.ToString()
        else 
          "###"

  type Game = {
    deck: Card list
    table: Card list
    stacks: StackCard list list
  }

  let deal shuffledDeck = 
    let emptyGame = {
      deck = shuffledDeck
      table = []
      stacks = []
    }
    [6..-1..1] 
    |>  List.fold (fun game i -> 
          let newStack = 
            game.deck 
            |> List.take i                        // flip the last card
            |> List.mapi (fun n card -> { isFaceUp = (n = i - 1); card=card}) 
          {
            stacks = game.stacks @ [ newStack ]
            deck = game.deck |> List.skip i
            table = []
          }
        
        ) emptyGame

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



;;
// DO IT!
newDeck 
|> shuffle 
|> Solitaire.deal 
|> Solitaire.printScreen