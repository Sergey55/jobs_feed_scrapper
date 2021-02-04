FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

COPY src/ /app/src
WORKDIR /app/src/
RUN dotnet restore

WORKDIR /app/src/JobsFeedScrapper.Service
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/runtime:5.0

WORKDIR /app
COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "JobsFeedScrapper.Service.dll"]