rem %1 - version
rem %2 - api key

nuget Pack ..\xVal.WebForms\xVal.WebForms.csproj
rem nuget Push ..\xVal.WebForms\xVal.WebForms.%1.nupkg %2