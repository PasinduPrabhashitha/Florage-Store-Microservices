FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./src/services/Florage.Authentication/Florage.Authentication.csproj", "."]
RUN dotnet nuget add source "https://nuget.pkg.github.com/PasinduPrabhashitha/index.json" --name "githubfeed" --username "PasinduPrabhashitha" --password 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 --store-password-in-clear-text
RUN dotnet restore "./Florage.Authentication.csproj"
COPY ./src/services/Florage.Authentication/. .
WORKDIR "/src/."
RUN dotnet build "Florage.Authentication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Florage.Authentication.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Florage.Authentication.dll"]