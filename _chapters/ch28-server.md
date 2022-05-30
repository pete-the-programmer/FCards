---
slug: Calling Home
concept: client-server
chapter: "28"
part: "Multiplayer"
feature: 
keyword:

---

We now have a finished product and we are ready to share it with the world!

## Publishing to a distributable

The first step of showing off our new game is to package it up into a form that is compatible with a web server.  This is fairly easily done using dotnet's `publish` command.

```bash
dotnet publish Website/Website.fsproj -o ./dist -c Release --nologo
```

This creates a working website in the "./dist" folder with all the extra debug stuff stripped out for release.

If you look at what is produced, you get a web config file, the program, and all the framework libraries to make __F#__ work. 

Technically, this is all you need - you could upload this to a webserver using plain old FTP and point to the _index.html_ file and it would just work.  But this is and __F#__ and dotnet series, so let's see what it takes to get things running on Azure.

## Creating infrastructure using Farmer

[_Farmer_](https://compositionalit.github.io/farmer/){: target="_blank"} is a community build project for generating the definition file (called a ARM template) that is used to manage Azure _resources_.  A resource might be a storage account, or a website, or maybe even a database, that is hosted in [Azure](https://azure.microsoft.com/){: target="_blank"}.

To use _Farmer_, we describe the Azure resources and how they link together using an __F#__ script file, just like the first versions of our game.  The _Farmer_ library is referenced at the top of the script directly from [NuGet.org](https://www.nuget.org/packages/Farmer/){: target="_blank"} and used to create a description of the resources we desire and then save that as a ARM template file.  We can even execute the creation of the resources in Azure directly from within the script!

Note that this is just a normal __F#__ script, and so can be as simple or complex as we want.

{% include project-so-far.md parts='deploy.fsx,deploy.sh'%}

To run this, you will need to have signed up for an _Azure_ account, and will need to install the [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) to login from the terminal.  You may need to execute `az login` first.

Once this has run, you can the url of the new website by:
1. login to the [Azure Portal](https://portal.azure.com/){: target="_blank"},
1. Navigate to the new resource group `solitaire-rg`
1. then to the storage account `solitairestorage`
1. then about halfway down in the left panel find `Data management > Static website`
1. On this page is the _Primary Endpoint_, which is the URL of the page.  
    It should look something like `https://solitairestorage.1234.web.core.windows.net/`

Open the URL in a browser and there you go - your own game on the internet for anyone to enjoy!

## Alternative: using GitHub pages to build and host

This is an alternative to hosting the game on Azure.  As this game is after all a static website, with no backend program, we _COULD_ host it anywhere.  So let's try getting GitHub Pages to build and host our game without have a separate host at all!

Github Pages can use a build action to compile the __F#__ code and then use the `index.html` file as the "Page".  

To make this work we need a couple of things:
- Setup your github repository to use Pages, and use the `gh-pages` branch as the place to store our pages.  If you haven't done this before [read this](https://docs.github.com/en/pages/getting-started-with-github-pages/about-github-pages){: target="_blank"}
- Use a ready-made publishing action by [James Ives](https://github.com/JamesIves/github-pages-deploy-action){: target="_blank"}
- Create the script for building the __F#__ project and making it available as a Page

To do this we need to give it a "workflow" yaml file and store it in the _special_ location `.github/workflows/main.yml`.

If all goes well, you should see something going on in the "Workflows" tab on the github portal every time you push a change to the repo, and be able to see the game at

`https://<your organisation/account name>.github.io/<your repo name>`

There is an example of this (using a custom domain &#x1F60A;) at [https://pete-the-programmer.com/Solitaire-Demo/](https://pete-the-programmer.com/Solitaire-Demo/){: target="_blank"}

{% include project-so-far.md parts='_github/workflows/main.yml'%}

> Note: If you're using GitLab, or Bitbucket, or something similar, then these hosts will usually be able to do something like this as well