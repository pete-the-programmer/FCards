run:
	cd _includes/src/ch$(ch); dotnet run

build:
	cd _includes/src/ch$(ch); dotnet watch build

web:
	cd _includes/src/ch$(ch); dotnet watch --project Website

publish:
	cd _includes/src/ch$(ch); dotnet publish Website/Website.fsproj -o ./ -c Release --nologo; mv wwwroot docs
