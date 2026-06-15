FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

USER root
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
USER $APP_UID

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["src/Services/AutoCatalog/AutoCatalog.Web/AutoCatalog.Web.csproj", "src/Services/AutoCatalog/AutoCatalog.Web/"]
COPY ["src/BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj", "src/BuildingBlocks/BuildingBlocks/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Messaging/BuildingBlocks.Messaging.csproj", "src/BuildingBlocks/BuildingBlocks.Messaging/"]
COPY ["src/Services/AutoCatalog/AutoCatalog.Infrastructure/AutoCatalog.Infrastructure.csproj", "src/Services/AutoCatalog/AutoCatalog.Infrastructure/"]
COPY ["src/Services/AutoCatalog/AutoCatalog.Application/AutoCatalog.Application.csproj", "src/Services/AutoCatalog/AutoCatalog.Application/"]
COPY ["src/Services/AutoCatalog/AutoCatalog.Domain/AutoCatalog.Domain.csproj", "src/Services/AutoCatalog/AutoCatalog.Domain/"]
RUN dotnet restore "./src/Services/AutoCatalog/AutoCatalog.Web/AutoCatalog.Web.csproj"
COPY . .
WORKDIR "/src/src/Services/AutoCatalog/AutoCatalog.Web"
RUN dotnet build "./AutoCatalog.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AutoCatalog.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AutoCatalog.Web.dll"]