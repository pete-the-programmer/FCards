module FishSite.Program
open System
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection


[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder(args)
    builder.Services.AddSignalR() |> ignore

    //use pipeline components here
    let app = builder.Build()
    app.UseDefaultFiles() |> ignore
    app.UseStaticFiles() |> ignore
    app.MapHub<FishHub>("/fishHub") |> ignore

    app.Run()
    0 // Exit code

