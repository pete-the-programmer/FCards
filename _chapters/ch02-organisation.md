---
slug: Organising your things
concept: Organisation
chapter: "02"
part: "Defining Cards"
---
### Modules
You can organise your code into "modules".

E.g. a simple module in a file
```fsharp
module Fcards.ch01  

let Spades_2 = "Spades 2"
let Spades_3 = "Spades 3"
```

... or even multiple modules under a namespace in one file
```fsharp
namespace Fcards

module ch01 =
  let Spades_2 = "Spades 2"
  let Spades_3 = "Spades 3"

module another = 
  let Spades_2 = "2 of Spades"
  let Spades_3 = "3\u9824"
```

### Using Modules
From another file/module you can reference the contents as a name-space, or you can also include a module/name-space using the keyword __open__
```fsharp
module Fcards.ch01  

let Spades_2 = "Spades 2"
let Spades_3 = "Spades 3"


// ANOTHER file...

module Fcards.ch02
printfn "%s" Fcards.ch01.Spades_2

//... OR ...

module Fcards.ch02
open Fcards.ch01 

printfn "%s" Spades_2
```

{% include sofar.md %}