 
FROM mcr.microsoft.com/dotnet/runtime:5.0

COPY src/JobsFeedScrapper.Service/bin/Release/net5.0/publish/ App/
WORKDIR /App
ENTRYPOINT ["dotnet", "JobsFeedScrapper.Service.dll"]