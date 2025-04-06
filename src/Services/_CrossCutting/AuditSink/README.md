# AuditSinkService

A cross-cutting microservice responsible for collecting and persisting audit logs from other services in the microservices ecosystem.

## Purpose
- Centralized audit log collection and persistence
- Provides a unified storage for all service audit trails
- Currently supports direct HTTP ingestion (POST /api/audit)
- Future support for RabbitMQ message-based ingestion

## Tech Stack
- .NET 8
- PostgreSQL with Entity Framework Core
- MediatR for CQRS pattern
- Minimal APIs

## API Endpoints

### POST /api/audit
Accepts audit log entries for persistence.

Request body:
```json
{
  "userId": "string",
  "action": "string",
  "serviceName": "string",
  "payload": "string"
}
```

## Project Structure
- `AuditSink.API`: HTTP endpoints and service configuration
- `AuditSink.Application`: Business logic and CQRS handlers
- `AuditSink.Domain`: Core entities and interfaces
- `AuditSink.Infrastructure`: Data persistence and external integrations
