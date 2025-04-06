FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all project files first for better layer caching
COPY ["src/Services/Identity/IdentityService.Api/identityservice.api.csproj", "Services/Identity/IdentityService.Api/"]
COPY ["src/Services/Identity/IdentityService.Application/IdentityService.Application.csproj", "Services/Identity/IdentityService.Application/"]
COPY ["src/Services/Identity/IdentityService.Domain/identityservice.domain.csproj", "Services/Identity/IdentityService.Domain/"]
COPY ["src/Services/Identity/IdentityService.Persistence/identityservice.persistence.csproj", "Services/Identity/IdentityService.Persistence/"]
COPY ["src/Shared/Shared.Common/Shared.Common.csproj", "Shared/Shared.Common/"]
COPY ["src/Shared/Shared.Logging/Shared.Logging.csproj", "Shared/Shared.Logging/"]
COPY ["src/Shared/Shared.Messaging/Shared.Messaging.csproj", "Shared/Shared.Messaging/"]
COPY ["src/Shared/Shared.Auditing/Shared.Auditing.csproj", "Shared/Shared.Auditing/"]

# Copy nuget.config
COPY ["nuget.config", "."]

# Restore packages
RUN dotnet restore "Services/Identity/IdentityService.Api/identityservice.api.csproj"

# Copy everything else
COPY src/ .

# Build and publish
WORKDIR "/src/Services/Identity/IdentityService.Api"
RUN dotnet build "identityservice.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "identityservice.api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "identityservice.api.dll"]
