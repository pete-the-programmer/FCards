
dotnet publish Website/Website.fsproj -o ./dist -c Release --nologo

dotnet fsi ./deploy.fsx ./dist/wwwroot
