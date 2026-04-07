# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet publish CocktailRobot.Server/CocktailRobot.Server.csproj -c Release -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# ⬇️ ВОТ ЭТА СТРОКА — ГЛАВНОЕ
COPY CocktailRobot.Server/firebase-key.json /app/firebase-key.json

COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "CocktailRobot.Server.dll"]
