# 1. Base pour l'exécution
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# 2. SDK pour la compilation
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copie des fichiers projets (tes chemins corrigés tout à l'heure)
COPY ["Backend/ProductManager.API.csproj", "Backend/"]
COPY ["AXIOCRM.Application/AXIOCRM.Application.csproj", "AXIOCRM.Application/"]
COPY ["AXIOCRM.Domain/AXIOCRM.Domain.csproj", "AXIOCRM.Domain/"]
COPY ["AXIOCRM.Infrastructure/AXIOCRM.Infrastructure.csproj", "AXIOCRM.Infrastructure/"]
COPY ["AXIOCRM.Shared/AXIOCRM.Shared.csproj", "AXIOCRM.Shared/"]

RUN dotnet restore "Backend/ProductManager.API.csproj"
COPY . .
WORKDIR "/src/Backend"
RUN dotnet build "ProductManager.API.csproj" -c Release -o /app/build

# 3. ÉTAPE CRUCIALE : Le "AS publish"
FROM build AS publish
RUN dotnet publish "ProductManager.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 4. Image finale
FROM base AS final
WORKDIR /app
# C'est ici que le --from=publish fera le lien avec l'étape 3
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductManager.API.dll"]