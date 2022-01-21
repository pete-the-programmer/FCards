module Fcards.ch07pipe

let add a b = a + b

let multiply a b = a * b

let chained = 
  7                 // 7
  |> add 4          // 11
  |> mutiply 6      // 66
  |> add 2          // 68 

let rec addAll (numbers: int list) =
  match numbers with 
  | [] -> 0
  | [a] -> a
  | a::rest -> a + (addAll rest)

addAll [1; 2; 3; 4; 5; 6; 7; 8; 9]  // returns 45