---
slug: The beautiful game
concept: MVU
chapter: "26"
part: "On the Web"
feature: 
  - MVU (Model-View-Update)
  - Type Providers
  - Templates
keyword:
  - bolero
---

By the end of the last chapter we had a working game; model, view, and updates.

But it started getting pretty ugly with _lots_ of square brackets and a [mixing of concerns](https://wikipedia.org/wiki/Separation_of_concerns) between the program data vs how it looks.  So this chapter we are going to add the idea of HTML templates

### HTML Templates

The blazor/bolero system allows us to define a template webpage using HTML, because that's what it's good for!  In the template we can put in some "Holes" for the data that are marked as `${...}`.

```html
<h3>Person ${Name}</h3>
<p>Age: ${Age}</p>
<p>Height: ${Height}</p>
```

We can even have little snippets of HTML that we can re-use
```html
<p>Here are my cards:</p>
<ul>
    ${Cards}
    <template id="Card">
        <li><img src="${Suit}${Number}.png"/></li>
    </template>
</ul>
```
In this example `${Cards}` is a hole that may be filled with list items from the template `Card`, that itself has a hole for the image source of each card.

In our `view.fs` code we can fill in the templates with the data from our Game.  The templates can be accessed in the code using the _Template_ type provider.

> A __Type Provider__ is a special feature of __F#__ that creates a _dynamic_ type at compile-time based on an external data source (in this case the template HTML).
> ```fsharp
> type Main = Template<"wwwroot/main.html">
> ```
> This line creates the type _Main_ that changes as you change the referenced html file so that it contains parts that reflect it.
> 
> For instance, the _Main_ type has the function `Card()` because there is a template with the id of "Card", and a function called `Suit()` because
> there is a Hole called "${Suit}".
>
> Type providers can be used on all sorts of data such as databases (with fields for tables that contain fields for their columns) or even JSON data sources on the internet.
> All before you even write the rest of the program!  Just hit "." in your editor after the type / variable and let the code-completion show you what's available!


### Moving the HTML into templates

Let's start with the card itself.  I found an image that contains all the cards in a deck, including the back.

{:class="center"}
![Cards]({{site.baseurl}}/assets/img/cards.png){:class="small"}

We can display a bit of this image at a time by setting the background image and position and therefore create a _template_ like this.
It includes holes for data and also an event hole to allow us to get a _callback_ when the player clicks the card.
```html
<template id="Card">
  <li onclick="${CardClicked}" class="">
    <div 
      class="card ${Selected}" 
      title="${CardText}" 
      alt="${CardText}" 
      style="
        background-position-x: calc( ${NumberOffset} * -45px);
        background-position-y: calc( ${SuitOffset} * -63px);
        "
    ></div>
  </li>
</template>
```
Note that the we put the amount of pixels that the image needs to offset into the html template itself, so that the calling code doesn't need to know how big a card image is (separating concerns).

Our view code can now just fill in the holes

```fsharp
type Main = Template<"wwwroot/main.html">

let SuitNumber card =
  match card with 
  // Note the image isn't in the same order
  //  but we can easily deal with that in this matcher
  | Hearts _ -> 1
  | Diamonds _ -> 3
  | Clubs _ -> 0
  | Spades _ -> 2
  | Joker -> 4

let viewCard dispatch cardDisplay =
  match cardDisplay with 
  | { CardDisplay.isFaceUp=false } -> Main.CardBack().Elt()
  | { card=card; isSelected=isSelected; selection=selection } -> 
    Main.Card()
      .NumberOffset(card.Number.Ordinal - 1 |> string) // holes expect a string value
      .SuitOffset(SuitNumber card |> string)
      .CardText(card.ToString())
      .Selected(if isSelected then "selected" else "notselected")
      .CardClicked(fun _ -> selection |> SelectCard |> dispatch )
      .Elt()
```
Once the template has been completed we call the function `Elt()`, which compiles the template into a single _element node_ that can be returned for display.

> TIP: The templates are _types_, and so are accessed as a sub-type of main: `Main.Card()`  (no brackets after "Main")  
> The holes are _fields_ of a template (incl the main template) and so are accessed as: `Main().Deck` or `Main.Card().CardText()`
> (This took me waaaay too long to figure out! &#x1F612; )

### Exercise: Convert to using template HTML

Given the main view function, create the views and templates to make it all look beautiful

```fsharp
let mainPage webgame dispatch = 
  Main()
    .SelectionMode(if webgame.selectedCard = NoSelection then "mode_unselected" else "mode_selected")
    .Deck(viewDeck dispatch webgame)
    .Table(viewTable dispatch webgame)
    .Aces(viewAces dispatch webgame)
    .Stacks(viewStacks dispatch webgame)
    .DrawSomeCards(fun _ -> DrawCards |> dispatch)
    .Elt()
```


{% include project-so-far.md parts='Website/Startup.fs,Website/Main.fs,Website/views.fs,Website/update.fs,Website/wwwroot/index.html,Website/wwwroot/main.html,Website/wwwroot/solitaire.css'%}