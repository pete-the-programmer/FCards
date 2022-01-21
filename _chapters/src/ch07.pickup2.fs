module Fcards.ch07
open Fcards.ch03

let pickupCard (hand: Card list) (deck: Card list) =
  match deck with 
  | [] -> failwith "No cards left!!!"
  | [a] -> hand @ [a]
  | a::rest -> hand @ [a]