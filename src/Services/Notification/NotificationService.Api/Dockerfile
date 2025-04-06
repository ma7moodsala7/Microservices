FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files first for layer caching
COPY ["src/Services/Notification/NotificationService.Api/NotificationService.Api.csproj", "Services/Notification/NotificationService.Api/"]
COPY ["src/Services/Notification/NotificationService.Application/NotificationService.Application.csproj", "Services/Notification/NotificationService.Application/"]
COPY ["src/Services/Notification/NotificationService.Domain/NotificationService.Domain.csproj", "Services/Notification/NotificationService.Domain/"]
COPY ["src/Services/Notification/NotificationService.Persistence/NotificationService.Persistence.csproj", "Services/Notification/NotificationService.Persistence/"]
COPY ["src/Shared/Shared.Common/Shared.Common.csproj", "Shared/Shared.Common/"]
COPY ["src/Shared/Shared.Messaging/Shared.Messaging.csproj", "Shared/Shared.Messaging/"]

# Restore packages
RUN dotnet restore "Services/Notification/NotificationService.Api/NotificationService.Api.csproj"

# Copy everything else
COPY ["src/", "."]

# Build
WORKDIR "/src/Services/Notification/NotificationService.Api"
RUN dotnet build "NotificationService.Api.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "NotificationService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.Api.dll"]
