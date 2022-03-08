dotnet publish Website/Website.fsproj -o ./dist -c Release --nologo
dotnet fsi Deployment/deploy.fsx -- ./dist/wwwroot
# dotnet run --project Deployment/Deployment.fsproj -- ./dist/wwwroot