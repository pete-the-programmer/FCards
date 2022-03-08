#r "nuget: Farmer"  // Get the Farmer libraries directly from NuGet

open System
open Farmer
open Farmer.Builders

// We'll pass the path to our published game in as an argument
let pathToGame = Environment.GetCommandLineArgs().[1]

let storage = storageAccount {
  name "solitairestorage"
  sku Storage.Sku.Standard_LRS
  use_static_website pathToGame "index.html"
  static_website_error_page "error.html"
}

let deployment = arm {
  location Location.AustraliaSoutheast
  add_resource storage
}

// Save as a ARM template file, just to check we got it right (optional)
deployment 
|> Writer.quickWrite "output"

// // Deploy into resource group directly to Azure
deployment
|> Deploy.execute "solitaire-rg" Deploy.NoParameters
|> printfn "%A"  // (print out any results - not expecting any)