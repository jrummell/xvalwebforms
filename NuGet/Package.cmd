rem %1 - version
rem %2 - api key

del *.nupkg
nuget Pack ..\xVal.WebForms\xVal.WebForms.csproj
nuget Push xVal.WebForms.%1.nupkg %2
