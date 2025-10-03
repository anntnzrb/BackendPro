# BackendPro.Core - Domain Layer

## Purpose

Heart of the application containing all business logic, domain entities, and contracts. Zero external dependencies - defines what the application does, not how.

## Structure

- `/Entities` - Domain models (User, Product, Order, etc.)
- `/Interfaces` - Service and repository contracts
- `/Services` - Business logic implementations
- `/DTOs` - Data Transfer Objects for service layer communication
- `/Enums` - Domain enumerations
- `/Exceptions` - Domain-specific exceptions

## Core Principles

**Dependency Inversion:** This layer defines interfaces; Infrastructure implements them.
**Pure Business Logic:** No framework dependencies. No EF Core, no ASP.NET, no HttpClient.
**Technology Agnostic:** Testable without infrastructure. Domain logic works regardless of database or UI choice.

## What Belongs Here

- Domain entities with business behavior
- Repository interfaces (IUserRepository, IProductRepository)
- Service interfaces and implementations
- DTOs for cross-layer communication
- Domain exceptions (UserAlreadyExistsException, InvalidOrderException)
- Business-level enums

## What Does NOT Belong

- View models (those are UI concerns - belong in Web)
- Data access implementations (DbContext, EF Core mappings - belong in Infrastructure)
- External service clients (HttpClient, email clients - belong in Infrastructure)
- Framework-specific attributes ([ApiController], [FromBody], etc.)

## Design Patterns

**Repository Pattern:** Define interfaces here, Infrastructure implements with EF Core.
**Service Layer Pattern:** Business logic in services, not controllers. Services orchestrate entities and repositories.
**Domain-Driven Design:** Rich domain models with behavior, not just property bags. Entities encapsulate business rules.

## Dependencies

**Current:** None by design.
**Allowed:** Pure utility libraries without infrastructure coupling.
**Never Add:** EF Core, ASP.NET Core, database drivers, external API SDKs.

## Testing

Unit test services and entities without any infrastructure. Mock repository interfaces for isolated service tests.
