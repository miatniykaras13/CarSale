FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

USER root
RUN mkdir -p /app/data && chown $APP_UID:$APP_UID /app/data
RUN apt-get update && apt-get install -y sqlite3 curl && rm -rf /var/lib/apt/lists/*
USER $APP_UID


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["src/Services/FileManagement/FileManagement.Grpc/FileManagement.Grpc.csproj", "src/Services/FileManagement/FileManagement.Grpc/"]
RUN dotnet restore "./src/Services/FileManagement/FileManagement.Grpc/FileManagement.Grpc.csproj"
COPY . .
WORKDIR "src/Services/FileManagement/FileManagement.Grpc"
RUN dotnet build "./FileManagement.Grpc.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FileManagement.Grpc.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "FileManagement.Grpc.dll"]