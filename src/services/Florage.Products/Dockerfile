FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./src/services/Florage.Products/Florage.Products.csproj", "."]
RUN dotnet nuget add source "https://nuget.pkg.github.com/PasinduPrabhashitha/index.json" --name "githubfeed" --username "PasinduPrabhashitha" --password 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 --store-password-in-clear-text
RUN dotnet restore "./Florage.Products.csproj"
COPY ./src/services/Florage.Products/. .
WORKDIR "/src/."
RUN dotnet build "Florage.Products.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Florage.Products.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Florage.Products.dll"]
