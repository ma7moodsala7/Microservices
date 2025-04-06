FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore as distinct layers
COPY ["src/Services/_CrossCutting/AuditSink/AuditSinkService.Api/AuditSinkService.Api.csproj", "Services/_CrossCutting/AuditSink/AuditSinkService.Api/"]
COPY ["src/Services/_CrossCutting/AuditSink/AuditSinkService.Application/AuditSinkService.Application.csproj", "Services/_CrossCutting/AuditSink/AuditSinkService.Application/"]
COPY ["src/Services/_CrossCutting/AuditSink/AuditSinkService.Domain/AuditSinkService.Domain.csproj", "Services/_CrossCutting/AuditSink/AuditSinkService.Domain/"]
COPY ["src/Services/_CrossCutting/AuditSink/AuditSinkService.Persistence/AuditSinkService.Persistence.csproj", "Services/_CrossCutting/AuditSink/AuditSinkService.Persistence/"]
COPY ["src/Shared/Shared.Common/Shared.Common.csproj", "Shared/Shared.Common/"]
COPY ["src/Shared/Shared.Messaging/Shared.Messaging.csproj", "Shared/Shared.Messaging/"]
COPY ["src/Shared/Shared.Logging/Shared.Logging.csproj", "Shared/Shared.Logging/"]
COPY ["src/Shared/Shared.Auditing/Shared.Auditing.csproj", "Shared/Shared.Auditing/"]
COPY ["nuget.config", "."]

RUN dotnet restore "Services/_CrossCutting/AuditSink/AuditSinkService.Api/AuditSinkService.Api.csproj"

# Copy everything else
COPY src/ .

# Build and publish
WORKDIR "/src/Services/_CrossCutting/AuditSink/AuditSinkService.Api"
RUN dotnet build "AuditSinkService.Api.csproj" -c Release -o /app/build
RUN dotnet publish "AuditSinkService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AuditSinkService.Api.dll"]
