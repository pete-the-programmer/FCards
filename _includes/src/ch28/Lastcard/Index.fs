module Lastcard.Index

open Bolero
open Bolero.Html
open Bolero.Server.Html
open Lastcard

type Page = Template<"wwwroot/index.html">

let page = 
  let node = rootComp<Main.MyApp>
  Page()
    .Main(node)
    .Scripts(boleroScript)
    .Elt()
