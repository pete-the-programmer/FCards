module Fcards.ch10
open System

let printOut (hand: 'a seq) =  String.Join("] [", hand) 

[ Hearts Eight; Diamonds Ten; Clubs Queen; Spades Two; Joker ]
|> printOut
|> printfn "[%s]"

// OUTPUT //
"[Hearts Eight] [Diamonds Ten] [Clubs Queen] [Spades Two] [Joker]"