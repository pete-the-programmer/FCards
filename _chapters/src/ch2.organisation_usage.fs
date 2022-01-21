module Fcards.ch1  

let Spades_2 = "Spades 2"
let Spades_3 = "Spades 3"


// ANOTHER file...

module Fcards.ch2
printfn "%s" Fcards.ch1.Spades_2

//... OR ...

module Fcards.ch2
open Fcards.ch1 

printfn "%s" Spades_2