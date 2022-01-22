module Fcards.ch10
open System

let looper() = 
  (fun _ -> Console.ReadKey().KeyChar |> Char.ToLowerInvariant )
  |> Seq.initInfinite
  |> Seq.takeWhile (fun x -> x <> 'q')
  |> Seq.iter (fun x -> printfn "%A" x)