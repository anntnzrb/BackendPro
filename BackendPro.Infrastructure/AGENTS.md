# BackendPro.Infrastructure - Data Access & External Services

## Purpose

Implements interfaces defined in Core. Handles all external concerns: database, file storage, email, external APIs, caching. The "how" to Core's "what".

## Typical Structure

- `/Data` - DbContext, migrations, configurations
- `/Repositories` - Repository implementations (IUserRepository → UserRepository)

## Key Responsibilities

**Data Access:** EF Core DbContext, migrations, repository implementations, entity configurations.
**External Integrations:** Third-party API clients, email services, file storage, payment gateways.
**Caching:** Redis, in-memory cache implementations.

## What Belongs Here

- DbContext and database migrations
- Repository implementations (implementing Core interfaces)
- EF Core entity configurations using IEntityTypeConfiguration<T>
- External service implementations (EmailService, StorageService)
- Third-party API clients
- Caching infrastructure

## What Does NOT Belong

- Business logic (belongs in Core)
- Controllers or views (belongs in Web)
- Domain entity definitions (defined in Core, configured here)

## Dependencies

**References:** BackendPro.Core (implements its interfaces)
**Common Packages:** Microsoft.EntityFrameworkCore, provider packages (SqlServer, PostgreSQL), Azure/AWS SDKs.

## Design Patterns

**Repository Pattern:** One repository per aggregate root. Keep focused, avoid generic mega-repositories.
**Unit of Work:** DbContext implements this pattern. Consider explicit interface if needed.
**Configuration Pattern:** Use IEntityTypeConfiguration<T> for clean EF mappings separate from DbContext.

## Integration Points

**With Core:** Implements Core interfaces, uses Core entities and DTOs.
**With Web:** Web registers Infrastructure services in DI. Connection strings from Web configuration.

## Configuration

Connection strings expected in Web's appsettings.json. DI registration happens in Web's Program.cs.
