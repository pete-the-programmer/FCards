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