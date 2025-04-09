FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore as distinct layers
COPY ["src/Services/Appointment/AppointmentService.Api/AppointmentService.Api.csproj", "Services/Appointment/AppointmentService.Api/"]
COPY ["src/Services/Appointment/AppointmentService.Application/AppointmentService.Application.csproj", "Services/Appointment/AppointmentService.Application/"]
COPY ["src/Services/Appointment/AppointmentService.Domain/AppointmentService.Domain.csproj", "Services/Appointment/AppointmentService.Domain/"]
COPY ["src/Services/Appointment/AppointmentService.Persistence/AppointmentService.Persistence.csproj", "Services/Appointment/AppointmentService.Persistence/"]
COPY ["src/Shared/Shared.Common/Shared.Common.csproj", "Shared/Shared.Common/"]
COPY ["src/Shared/Shared.Messaging/Shared.Messaging.csproj", "Shared/Shared.Messaging/"]
COPY ["src/Shared/Shared.Logging/Shared.Logging.csproj", "Shared/Shared.Logging/"]
COPY ["src/Shared/Shared.Auditing/Shared.Auditing.csproj", "Shared/Shared.Auditing/"]
COPY ["src/Shared/Shared.Settings/Shared.Settings.csproj", "Shared/Shared.Settings/"]
COPY ["nuget.config", "."]

RUN dotnet restore "Services/Appointment/AppointmentService.Api/AppointmentService.Api.csproj"

# Copy everything else
COPY src/ .

# Build and publish
WORKDIR "/src/Services/Appointment/AppointmentService.Api"
RUN dotnet build "AppointmentService.Api.csproj" -c Release -o /app/build
RUN dotnet publish "AppointmentService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AppointmentService.Api.dll"]
