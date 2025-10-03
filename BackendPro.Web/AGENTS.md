# BackendPro.Web - Presentation Layer

## Purpose

ASP.NET Core MVC application handling HTTP requests, rendering views, orchestrating user interactions. Application entry point where DI and middleware are configured.

## Structure

- `/Controllers` - MVC controllers (orchestrate, no business logic)
- `/Views` - Razor views (.cshtml)
- `/Models` - View models (NOT domain entities)
- `/wwwroot` - Static files (CSS, JS, images)
- `Program.cs` - Entry point, DI configuration, middleware pipeline
- `appsettings.json` - Configuration

## Key Responsibilities

**Request Handling:** Controllers receive HTTP requests, delegate to Core services, return views or JSON.
**Dependency Injection:** Register all services in Program.cs, configure database connections, set up middleware.
**View Rendering:** Transform domain data into view models, render Razor views, serve static assets.

## What Belongs Here

- Thin controllers that orchestrate only (no business logic)
- View models shaped for UI needs
- Razor views and layouts
- Middleware configuration and pipeline setup
- DI service registration for all layers
- Application configuration (appsettings.json)

## What Does NOT Belong

- Business logic (belongs in Core services)
- Data access code (no DbContext in controllers - use services)
- Domain entities passed directly to views (transform to view models first)

## Critical Principles

**Thin Controllers:** Delegate to Core services. No business rules, calculations, or validation logic in controllers.
**View Models vs Domain Entities:** View models are UI-shaped and may flatten/combine entities. Domain entities are business-focused.
**Separation of Concerns:** Controllers handle HTTP (routing, status codes). Services handle business logic. Repositories handle data access.

## Dependencies

**References:** BackendPro.Core (interfaces and DTOs), BackendPro.Infrastructure (for DI registration only)
**Note:** Controllers depend on Core interfaces, not Infrastructure implementations.

## When Adding Features

1. Define business logic in Core first
2. Implement data access in Infrastructure
3. Register services in Program.cs
4. Create controller and views here
5. Use view models to shape data for views
