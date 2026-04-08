# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем csproj-файлы
COPY CocktailRobot.Server/CocktailRobot.Server.csproj CocktailRobot.Server/
COPY CocktailRobot.Shared/CocktailRobot.Shared.csproj CocktailRobot.Shared/

# Восстанавливаем зависимости
RUN dotnet restore CocktailRobot.Server/CocktailRobot.Server.csproj

# Копируем весь код
COPY CocktailRobot.Server/ CocktailRobot.Server/
COPY CocktailRobot.Shared/ CocktailRobot.Shared/

# Публикуем сервер
RUN dotnet publish CocktailRobot.Server/CocktailRobot.Server.csproj -c Release -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "CocktailRobot.Server.dll"]
