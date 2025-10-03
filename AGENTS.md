# BackendPro - AI Agent Guidelines

## Architecture

Three-layer Clean Architecture following dependency rule: Web → Infrastructure → Core

- **Core**: Domain entities, business logic, interfaces, DTOs. Zero external dependencies.
- **Infrastructure**: Repository implementations, DbContext, external service clients, caching.
- **Web**: Controllers, views, view models, middleware, DI configuration.

## Dependency Rules

1. Core has no dependencies on other projects (pure domain logic)
2. Infrastructure references Core (implements its interfaces)
3. Web references both Core and Infrastructure
4. Core must never reference Infrastructure or Web

## Layer Responsibilities

**Core (`/Entities`, `/Interfaces`, `/Services`, `/DTOs`, `/Enums`, `/Exceptions`):**
- Domain entities and business rules
- Service and repository interfaces
- Business logic implementations
- Data transfer objects
- Domain-specific exceptions

**Infrastructure:**
- EF Core DbContext and migrations
- Repository implementations
- External API clients
- Third-party service integrations
- Caching implementations

**Web:**
- MVC controllers (thin orchestration only)
- View models (UI-specific, NOT domain entities)
- Razor views and static files
- Middleware pipeline configuration
- DI service registration

## Critical Rules

- Business logic belongs in Core services, never in controllers
- View models stay in Web (they're presentation concerns)
- Data access code only in Infrastructure
- Controllers delegate to Core services, no direct business logic
- No EF Core or framework references in Core
- Raw SQL commands are prohibited; use EF Core abstractions only
- Always express LINQ with method syntax (`Where`, `Select`, etc.); query syntax (`from ... in ... select ...`) is discouraged and should not be used

## Development Workflows

**Note:** Use `dotnet.exe` instead of `dotnet` for all commands.

Build: `dotnet.exe build BackendPro.sln`
Run: `dotnet.exe run --project BackendPro.Web`
Watch mode: `dotnet.exe watch --project BackendPro.Web`

## Feature Development Flow

1. Define domain entities in Core/Entities
2. Create service interfaces in Core/Interfaces
3. Implement business logic in Core/Services
4. Implement data access in Infrastructure
5. Register services in Web/Program.cs
6. Create controllers and views in Web

## Technology Stack

.NET 8, ASP.NET Core MVC, nullable reference types enabled, implicit usings enabled.

## Localization

**Code:** English (variables, methods, classes, comments, commit messages)
**User-Facing:** Spanish (UI text, labels, messages, validation errors, views)

## Database Strategy

**Development & Production:** SQL Server for both environments

Connection string format: `Server=localhost\\SQLEXPRESS;Database=BackendPro;Trusted_Connection=True;TrustServerCertificate=True`

Use SQL Server-specific features as needed. The project is configured to use SQL Server Express for local development and full SQL Server for production deployments.
