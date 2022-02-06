---
slug: Making our site dynamic
concept: mvu
chapter: "23"
part: "On the Web"
feature: 
  - MVU (Model-View-Update)
keyword:
  - bolero

---

### Model-View-Update (MVU)

In order to interact with people _Bolero_ uses a __MVU__ system:
- __Model__  : The data that can change in response to the player's interaction. This would be our `Game` object.
- __View__   : Code to display the model on the page.  In the terminal version, this was the _printing_ functions.
- __Update__ : The list of things called _Messages_ that a player can do, and how they affect the model. This is similar to our _actions_.

### Add interactivity to our game

#### Put it in the page

First of all we need to add a place on the webpage for our "app" to run, and we need to include the standard blazor javascript library to hook it up.

To do this, update the `index.html` file to:
```html
<html>
  <head>
    <title>Solitaire</title>
  </head>
  <body>
    <h1>Solitaire</h1>
    <div id="main">Loading...</div>

    <script src="_framework/blazor.webassembly.js"></script>    
  </body>
</html>
```

#### Add the MVU cycle

Then, we add a new file for our main app called "Main.fs" (and don't forget to add it to the project file _above_ the _Startup.fs_ entry).

```fsharp
module Solitaire.Website.Main

open Elmish
open Bolero
open Bolero.Html

type MyApp() =
  inherit ProgramComponent<SimpleModel, SimpleMessage>()

  override this.Program =
    Program.mkSimple initialise update view
```

From this code you can see that we need to create a few things for the standard bolero code to work.

We need a couple of types for our model and list of messages:
```fsharp
type SimpleModel = string

type SimpleMessage =  // must be a DU
  | DoNothing
```

We also need the functions that `Program.mkSimple` use to make the MVU cycle work
1. Initialise the model (of type `SimpleModel`) at the start, provided some startup arguments (which we'll ignore for now)
  ```fsharp
  let initialise args = ""   
  ```
1. An updater function that takes a message and the current model and returns the updated model.  We're not even doing anything with the message yet.
  ```fsharp
  let update message model = model   
  ```
1. A view function that takes a model and a dispatcher (more on that later) and returns a webpage `Node` like some text, or a heading, or maybe an image.  
  I used the simple `text` function that turns a string into a simple piece of text on a webpage
  ```fsharp
  let view model dispatch = text "Let's play Solitaire!" 
  ```

#### Include the app in the startup

To include the app, we add the following line into the `Startup.fs` _before_ the _builder_ executes it's _Build()_ function.
```fsharp
builder.RootComponents.Add<Main.MyApp>("#main")
```

### See the View

That's it.  Run the project again and now you should see the "Loading..." text pop up for a second before it's replaced by our _view_ text

{% include project-so-far.md parts='Website/Startup.fs,Website/wwwroot/index.html,Website/Main.fs'%}