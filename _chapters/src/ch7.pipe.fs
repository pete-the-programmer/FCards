module Fcards.ch7pipe

let add a b = a + b

let multiply a b = a * b

let chained = 
  7                 // 7
  |> add 4          // 11
  |> mutiply 6      // 66
  |> add 2          // 68 