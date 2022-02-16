---
slug: Playing the game on the Web
concept: MVU
chapter: "25"
part: "On the Web"
feature: 
  - MVU (Model-View-Update)
keyword:
  - bolero
  - dispatch
---
We have the data in the _model_, and the _view_ to display it, now we need to interact with the player to allow them to play the game.

### Dispatching messages from our View

You may remember that one of the inputs passed to the `viewMain` function was called "dispatch".  

> TIP: `dispatch()` is a function that takes a command message and passes it to our `update()` function, so that we can change the state of the model
> and therefore make progress in our game

It's passed into the view function so that we can create links/buttons/interactions that can generate messages that we can then _dispatch_ to the _bolero_ MVU system.

We need to pass the `dispatch` function to our sub-view functions.  For instance, in the _viewDeck_ function we can generate a `DrawCards` message when the deck cards are clicked.

```fsharp
let viewDeck dispatch webgame =
  List.init webgame.game.deck.Length ( fun _ -> viewCardBack |> wrap |> li [] )
  |> ul [ on.click( fun _ -> DrawCards |> dispatch  ) ]
  |> fun a -> [ h3 [] [text "Deck"]; a]
  |> div [ Attr.Classes ["deck"] ]

let viewMain webgame dispatch = 
  let stacks = viewStacks dispatch webgame
  let aces = viewAces dispatch webgame
  let table = viewTable dispatch webgame
  let deck = viewDeck dispatch webgame
  ...
```

The `on.click()` _bolero_ function has a _MouseEventArgs_ input (which we'll just ignore), and then we "dispatch" a `DrawCards` message.

### Dealing with messages

Similarly to our terminal game we are going to need a couple of phases in the interaction with the player to generate a single move.

This time rather than multiple keystrokes, we can pretty much get away with two clicks:
1. select a card to move
2. select the destination for the card

We can model this by wrapping the base `Game` in a `WebGame` record and using a DU for the messages.  The exception is the `DrawCards` message, which is really only needs to be a single click.
```fsharp
type StackCardLocation = {
  stacknum: int;
  cardnum: int;
}

type CardSelection =
  | NoSelection
  | TableCardSelected
  | StackCardSelected of StackCardLocation

type TargetSelection = 
  | StackTarget of int
  | AceTarget of int

type WebCommands =  // must be a DU
  | SelectCard of CardSelection
  | PlaceCard of TargetSelection
  | DrawCards

type WebGame = {
  selectedCard: CardSelection
  game: Game
}

...
type MyApp() =
  inherit ProgramComponent<WebGame, WebCommands>()

  override this.Program =
    Program.mkSimple initialise update viewMain
```

### Responding to messages

Now that we have the commands/messages sorted we can respond to the messages to update the game
```fsharp
let update message webgame =
  printfn "%A" message   // actually quite useful for debugging.  It prints the messages in the browser's console
  match message with
  | DrawCards -> 
      {webgame with         // Note how we deal with two DU's with the same names
        game = applyCommand Solitaire.Actions.DrawCards webgame.game
        selectedCard = NoSelection
      }
  | SelectCard selection -> 
      {webgame with selectedCard = selection}
  | PlaceCard target -> 
      match target, webgame.selectedCard with   // matching on a combo of values
      | _, NoSelection -> webgame
      | AceTarget _, TableCardSelected ->
          {webgame with 
            game = applyCommand TableToAce webgame.game
            selectedCard = NoSelection 
          }
      | AceTarget _, StackCardSelected selection -> 
          {webgame with 
            game = applyCommand (StackToAce selection.stacknum) webgame.game
            selectedCard = NoSelection 
          } 
      | StackTarget toStack, TableCardSelected ->       
          {webgame with 
            game = applyCommand (TableToStack toStack) webgame.game
            selectedCard = NoSelection 
          } 
      | StackTarget toStack, StackCardSelected selection ->       
          {webgame with 
            game = 
              applyCommand (
                {
                  sourceStack=selection.stacknum       // Needs some maths!
                  numCards=(webgame.game.stacks[selection.stacknum-1].Length - selection.cardnum) 
                  targetStack=toStack
                }
                |> MoveCards
              ) webgame.game
            selectedCard = NoSelection 
          } 
```

### Exercise: Click cards & highlight the selected one

There actually three parts to this exercise:
1. Add a click dispatcher to cards
2. Add "drop target" options for the selected card (with its own dispatch) at the bottom of each stack and ace stack
3. Add a "selected" css class to the card that is selected

> TIP: I extended the `CardDisplay` record to include selection so that a lot of the work can be done in the `ViewCard` function
> ```fsharp
> type CardDisplay = {
>   card: Card
>   isFaceUp: bool
>   isSelected: bool
>   selection: CardSelection
> }
> ```

As an example I have done the `viewTable` function for you

```fsharp
let viewTable dispatch webgame =
  match webgame.game.table with 
    | [] -> []
    | [card] -> 
        [
          {
            card=card
            isFaceUp=true
            isSelected=(webgame.selectedCard=TableCardSelected)
            selection=NoSelection
          } 
          |> viewCard dispatch
        ]
    | topcard::rest -> 
        let facedowns = List.init (rest.Length) (fun _ -> viewCardBack)
        let faceup = 
          {
            card=topcard
            isFaceUp=true
            isSelected=(webgame.selectedCard=TableCardSelected)
            selection=TableCardSelected
          }
          |> viewCard dispatch
        facedowns @ [ faceup ]
  |> List.map ( fun a -> li [] [a])
  |> ul []
  |> fun a -> [ h3 [] [text "Table"]; a]
  |> div [ Attr.Classes ["table"] ]
```

See the code below for an answer



{% include project-so-far.md parts='Website/Startup.fs,Website/Main.fs,Website/views.fs,Website/update.fs,Website/wwwroot/index.html,Website/wwwroot/solitaire.css'%}