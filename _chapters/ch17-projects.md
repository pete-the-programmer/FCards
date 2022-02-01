---
slug: Organising Projects
concept: projects
chapter: "17"
part: "Solitaire"
---

We now have code for print stuff, code for change the game, the game itself, and more. There is quite a bit of code!

So far it has all been in one file that can be easily executed in the interpreter (fsi), but now it's starting to get unwieldy.
So this chapter is going to be a bit of admin on creating a "project" that can be built and run as a .NET program.

### Projects

A _project_ is a group of code that can be compiled into a single executable, or library (e.g. dll). 

To create an __F#__ Console App project we can use the `dotnet` cli.  We need to specify that it should be for __F#__ and we want to name it "Solitaire"
```bash
dotnet new console -lang F# -n Solitaire
```
This will create a Program.fs file and a simple project file like this:
```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <Compile Include="Program.fs" />
  </ItemGroup>

</Project>
```
You can now build and run the program!
```bash
dotnet run

> Hello from F#
```

### The Solitaire project

I have broken down our single solitaire code block into a number of file and added them to the project (including the core cards code from chapter 13)
```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <Compile Include="cards.fs" />
    <Compile Include="model.fs" />
    <Compile Include="actions.fs" />
    <Compile Include="printing.fs" />
    <Compile Include="Program.fs" />
  </ItemGroup>

</Project>
```

> TIP: __F#__ is compiled in a single-pass, so we have to put the files in our project in the correct order so that things are defined _before_ they are used

You can copy the contents of the code below, or just download it as a [zip file to get you started]({{site.baseurl}}/ch17.zip) .

{% include project-so-far.md %}
