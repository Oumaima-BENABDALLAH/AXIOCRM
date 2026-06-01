FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
WORKDIR /src

# Copie des fichiers projets
COPY ["Backend/ProductManager.API.csproj", "Backend/"]
COPY ["AXIOCRM.Application/AXIOCRM.Application.csproj", "AXIOCRM.Application/"]
COPY ["AXIOCRM.Domain/AXIOCRM.Domain.csproj", "AXIOCRM.Domain/"]
COPY ["AXIOCRM.Infrastructure/AXIOCRM.Infrastructure.csproj", "AXIOCRM.Infrastructure/"]
COPY ["AXIOCRM.Shared/AXIOCRM.Shared.csproj", "AXIOCRM.Shared/"]

RUN dotnet restore "Backend/ProductManager.API.csproj"

COPY . .
WORKDIR "/src/Backend"
RUN dotnet build "ProductManager.API.csproj" -c Release -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "ProductManager.API.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductManager.API.dll"]