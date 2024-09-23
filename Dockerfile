FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
COPY ./out .
ENTRYPOINT ["dotnet", "YourApp.dll"]
