---
slug: A basic website
concept: Blazor, Bolero, and the web
chapter: "22"
part: "On the Web"
feature: 
  - Webpages
keyword:
  - bolero
  - blazor
---

Playing on a terminal is fine, but everybody does stuff on the web now-a-days.  Let's create a website!

> As this is a series on __F#__, not javascript or even HTML, we're going to use a completely .NET solution.
> You may have heard of the .NET project called _Blazor_.  This is a technology that allows us to use the 
> power of .NET by compiling into a web assembly component (WASM for short).
> This technology is quite _C#_ oriented, so we are going to use a set of libraries that work over the top 
> of _Blazor_ to give us some __F#__ goodness, called _Bolero_.

### Projects and Solutions

We now have another layer to our application to handle website stuff, so it is useful to keep them in separate _projects_.

To connect a set of _projects_ together into a whole application we use a _solution_ file.
This _solution_ is really just a list of projects.  To create one...

```bash
dotnet new sln -n Solitaire

dotnet new classlib -lang f# -n Game
dotnet sln add Game

dotnet build
```

Now we have a _solution_ called "Solitaire", with a class library (i.e. a dll) called "Game".

Add the `actions.fs`, `cards.fs`, and `model.fs` files from the last chapter into this library folder, and list them in the project like this:
```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <Compile Include="cards.fs" />
    <Compile Include="model.fs" />
    <Compile Include="actions.fs" />
  </ItemGroup>

</Project>
```

Note that we don't need the printing file because we're not printing to the terminal screen,
nor the program file because we're making a library not a self-running program.

### Creating the Bolero Website

Let's create another _project_ for the website that uses our "Game" library

```bash
dotnet new classlib -lang f# -n Website

cd Solitaire.Client
# Add Nuget packages for Bolero to work
dotnet add package Bolero
dotnet add package Microsoft.AspNetCore.Components.WebAssembly.DevServer
# Make it so this project can "see" our core project 
dotnet add reference ../Game

cd ..
dotnet sln add Website
```

> TIP: `dotnet add` and _NuGet_:  
> _NuGet_ is the .NET system for creating and sharing packages with the community over the internet.
> To add a package to your project you can use the command:
> ```bash
> dotnet add package <name>
> ```
> You can search for packages at [nuget.org](https://www.nuget.org/){:target='_nuget'}.
>
> To allow one project to use another project, create a reference link:
> ```bash
>  dotnet add reference <project>
> ```

#### A basic website

We will need to tweak the "Website" project slightly to tell the compiler that it's a `Blazor` style project
```xml
<Project Sdk="Microsoft.NET.Sdk.BlazorWebAssembly">
  ...
</Project>
```

If you try to build this solution, it will currently show an error saying that it doesn't have a starting point for the blazor site.

We need to tell it how to start.  So, rename the automagically created file "Library.fs" to "Startup.fs" (also in the project file) and replace the contents with the standard setup:
```fsharp
namespace Solitaire.Website

open System
open System.Net.Http
open Microsoft.AspNetCore.Components.WebAssembly.Hosting
open Microsoft.Extensions.DependencyInjection

module Program =

  [<EntryPoint>]
  let Main args =
    let builder = WebAssemblyHostBuilder.CreateDefault(args)
    builder.Services.AddScoped<HttpClient>(
        fun _ -> new HttpClient(BaseAddress = Uri builder.HostEnvironment.BaseAddress)
      ) 
      |> ignore
    builder.Build().RunAsync() |> ignore
    0
```

We also need a starting web page.  Create a sub-folder in the Website folder called "wwwroot" (this is a standard - don't use another name!), and add a file call "index.html" with some simple contents:
```html
<html>
  <head>
    <title>Solitaire</title>
  </head>
  <body>
    <h1>Solitaire</h1>

  </body>
</html>
```

We now have the most basic of websites! 

To see it, run `dotnet run` in the Website folder, and open a browser tab to the localhost port that is printed out (the default is [localhost:5000](http://localhost:5000){:target='_solitaire'} )


{% include project-so-far.md parts='Website/Startup.fs,Website/wwwroot/index.html'%}