# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

PatientPortal is a Blazor Server application (.NET 9.0) being built as a training project for healthcare payment space. The application implements patient management with Okta authentication, Entity Framework Core, and SQL Server.

## Build and Run Commands

```bash
# Build the project
dotnet build

# Run the application
dotnet run --launch-profile "PatientPortal"

# Database operations
dotnet ef migrations add <MigrationName>    # Create new migration
dotnet ef database update                   # Apply migrations
dotnet ef migrations remove                 # Remove last migration

# Future test commands (when test projects are added)
dotnet test                                 # Run all tests
dotnet test --filter FullyQualifiedName~<TestName>  # Run specific test
```

Application URLs:
- HTTPS: https://localhost:7047
- HTTP: http://localhost:5245

## Architecture

The application follows a layered architecture:

1. **Presentation Layer**: Blazor Server pages in `/Pages`
2. **Data Layer**: Entity Framework Core with `ApplicationDbContext` in `/Data`
3. **Models**: Domain entities in `/Models` (currently only `Patient`)

### Key Components

- **Database**: SQL Server on `localhost,1433` with database `PatientPortalDb`
- **Authentication**: Planned Okta integration (not yet implemented)
- **API**: Planned Web API controllers (not yet implemented)

### Current State Issues

1. **Empty Migration**: The existing migration at `Migrations/20250715221236_InitialCreate.cs` is empty and doesn't create the Patient table. This needs to be fixed before any database operations.

2. **Missing Authentication**: No Okta configuration exists yet. When implementing:
   - Add Okta NuGet packages
   - Configure in `Program.cs`
   - Add Okta settings to `appsettings.json`

3. **No Repository Pattern**: Currently using DbContext directly. Plan includes implementing repository pattern.

## Development Workflow

Follow the PROJECT_TIMELINE.md for structured development approach. Priority order:

1. Fix database migration (delete and recreate)
2. Implement Okta authentication
3. Create API layer with token validation
4. Build patient management UI components
5. Add comprehensive testing

## Configuration

- **User Secrets**: Configured with ID `4d936ed1-1683-42db-a2cb-a19ac4b9c53b`
- **Connection String**: Currently in `appsettings.json` - should move sensitive parts to user secrets
- **Entity Framework**: Using version 6.0.25 (consider upgrading to match .NET 9.0)

## Common Tasks

### Fix the Empty Migration
```bash
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Add a New Model to Database
1. Create model class in `/Models`
2. Add DbSet to `ApplicationDbContext`
3. Create migration: `dotnet ef migrations add AddModelName`
4. Update database: `dotnet ef database update`

### Working with Blazor Components
- Pages go in `/Pages` with `.razor` extension
- Shared components in `/Shared`
- Services should be registered in `Program.cs`
- Use `@inject` to inject services into components

## Dependencies to Note

- Entity Framework Core 6.0.25 (SQL Server provider)
- .NET 9.0 target framework
- No test frameworks installed yet
- No Okta packages installed yet

## Future Considerations

When implementing features, ensure:
- All database operations go through repository pattern (once implemented)
- API endpoints require Okta token validation
- Use DTOs for API requests/responses
- Implement proper error handling and logging
- Follow the established project structure