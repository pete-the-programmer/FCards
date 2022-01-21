module Fcards.ch5

let add a b = a + b

let multiply a b = a * b

let combine combiner a b =
  combiner (a + 1) (b * 2)

// to use it...
let added = combine add 3 4  // should equal 12
let multiplied = combine multiply 3 4  // should equal 32
