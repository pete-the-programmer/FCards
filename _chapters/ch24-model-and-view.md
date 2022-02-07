---
slug: Display the game on the Web
concept: MVu
chapter: "24"
part: "On the Web"
feature: 
  - MVU (Model-View-Update)
  - Making HTML elements
keyword:
  - bolero

---

So far we've managed to set up a dynamic website, but it doesn't do much, or have anything to do with our game.

Now we'll swap in our game's _model_, and build some simple _view_ for it.  This work will all be in the `Main.fs` module.

### The Game Model

Good news!  The `Game` type is perfect as the _Model_.  So we can just delete the `SimpleModel` type and change the App at the bottom.
```fsharp
open Solitaire.Model

...

type MyApp() =
  inherit ProgramComponent<Game, SimpleMessage>()

  override this.Program =
    Program.mkSimple initialise update view
```

#### Exercise: initialise the model

Write the `initialise` function so that it returns our starting game.

[See an answer]({{ site.baseurl }}{{ page.url }}#initialise)

{:class="collapsible" id="initialise"}
```fsharp
let initialise args = 
  newDeck 
  |> shuffle 
  |> deal
```

### The Game View

This may get a bit busy with a lot of square brackets.

> TIP: Bolero/Blazor html functions generally have a standard format that takes a list of HTML attributes and a list of HTML child nodes
> There are a lot of HTML functions that are named after HTML elements; e.g. div(), li(), ul(), and text().
> ```fsharp
> div [ ...  attributes of the div ... ] [ ... things inside the div ... ]
> 
> div [ Attr.Classes ["jumbo"; "sparkly"] ] [ text "Hello" ] // <div class="jumbo sparkly">Hello</div>
> 
> text "Hello"
> |> div [ Attr.Classes ["jumbo"; "sparkly"] ]               // <div class="jumbo sparkly">Hello</div>
> 
> // or maybe a list
> game.deck                                                  // Card list
> |> List.map (fun card -> li [] [ text (card.ToString()) ]  // `li` list
> |> ul []                                                   // <ul> <li>♣Q</li> ... </ul>  
>    
> ```

There are a few bits to the view, so we'll break it down in a similar way we did when we created the `printing` module.

We can use `div`s to group parts together into layers:
```
     ______(topHalf)________________________________________
    | _________________________     ______________________  |
    ||   Stacks                |   |     Aces             | |
    ||                         |   |                      | |
    ||_________________________|   |______________________| |
    |_______________________________________________________|

     _______________________________________________________
    |      Table                                            |
    |_______________________________________________________|

     _______________________________________________________
    |      Deck                                             |
    |_______________________________________________________|
```

```fsharp
let viewMain game dispatch = 
  let stacks = viewStacks game
  let aces = viewAces game
  let table = viewTable game
  let deck = viewDeck game
  let topHalf = div [Attr.Classes ["topHalf"]] [stacks; aces]
  div [Attr.Classes ["game"]] [topHalf; table; deck]
```
... and use some CSS to shape them up using classes.  I won't go into the CSS as that's a whole other topic, but I will include it at the bottom of the chapter.

### Excerise: Sub-views

To create the sub-view for printing out the table we can do something similar to below
```fsharp
let viewCard card =
  let txt = text $"[{card}]"
  let colorAttr = 
    match card with 
    | IsRed _ -> ["red"]
    | IsBlack _ -> ["black"]
    | _ -> []
    |> Attr.Classes
  span [ colorAttr ] [txt]
  
let viewCardBack card = span [ ] [ text "[###]"] 

let viewTable game =
  match game.table with 
    | [] -> []
    | [a] -> [viewCard a]
    | topcard::rest -> 
        let facedowns = rest |> List.map viewCardBack
        facedowns @ [viewCard topcard]
  |> List.map ( fun a -> li [] [a])
  |> ul []
  |> fun a -> [ h3 [] [text "Table"]; a]
  |> div [ Attr.Classes ["table"] ]
```

Write the sub-view functions for `viewDeck`, `viewAces`, and `viewStacks`.

> To make things a little easier I used a one-line function to wrap a thing in a list
> ```fsharp
> let wrap = List.singleton  // shortcut to wrap a thing in a list
> ```

[See an answer for viewDeck]({{ site.baseurl }}{{ page.url }}#viewDeck)

{:class="collapsible" id="viewDeck"}
```fsharp
let viewDeck game =
  game.deck
  |> List.map ( fun card -> card |> viewCardBack |> wrap |> li [] )
  |> ul []
  |> fun a -> [ h3 [] [text "Deck"]; a]
  |> div [ Attr.Classes ["deck"] ]
```

[See an answer for viewAces]({{ site.baseurl }}{{ page.url }}#viewAces)

{:class="collapsible" id="viewAces"}
```fsharp
let suits = [SYMBOL_HEART; SYMBOL_DIAMOND; SYMBOL_CLUB; SYMBOL_SPADE]

let viewAces game =
  game.aces
  |> List.mapi (fun i stack -> 
    stack
    |> List.map ( fun card -> card |> viewCard |> wrap |> li [] )
    |> ul []
    |> fun x -> [h4 [] [suits[i] |> text]; x]
    |> div []
  )
  |> div [ Attr.Classes ["aces"] ]
```

[See an answer for viewStacks]({{ site.baseurl }}{{ page.url }}#viewStacks)

{:class="collapsible" id="viewStacks"}
```fsharp
let viewStacks game =
  game.stacks
  |> List.mapi (fun i stack -> 
    stack
    |> List.map ( fun card -> card |> viewStackCard |> wrap |> li [] )
    |> ul []
    |> fun x -> [h4 [] [(i + 1).ToString() |> text]; x]
    |> div []
  )
  |> div [ Attr.Classes ["stacks"] ]
```

### Result

![Solitaire at ch 24]({{site.baseurl}}/assets/img/ch24.png)
{:class="center"}


{% include project-so-far.md parts='Website/Startup.fs,Website/Main.fs,Website/views.fs,Website/wwwroot/index.html,Website/wwwroot/solitaire.css'%}