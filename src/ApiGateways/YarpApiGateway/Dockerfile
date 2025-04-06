FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["src/ApiGateways/YarpApiGateway/YarpApiGateway.csproj", "ApiGateways/YarpApiGateway/"]
COPY ["nuget.config", "."]

# Restore packages
RUN dotnet restore "ApiGateways/YarpApiGateway/YarpApiGateway.csproj"

# Copy everything else
COPY ["src/ApiGateways/YarpApiGateway/", "ApiGateways/YarpApiGateway/"]

# Build
WORKDIR "/src/ApiGateways/YarpApiGateway"
RUN dotnet build "YarpApiGateway.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "YarpApiGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YarpApiGateway.dll"]
