# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем ТОЛЬКО серверный .csproj
COPY CocktailRobot.Server/CocktailRobot.Server.csproj CocktailRobot.Server/
RUN dotnet restore CocktailRobot.Server/CocktailRobot.Server.csproj

# Копируем весь серверный проект
COPY CocktailRobot.Server/ CocktailRobot.Server/

# Публикуем сервер
RUN dotnet publish CocktailRobot.Server/CocktailRobot.Server.csproj -c Release -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "CocktailRobot.Server.dll"]
