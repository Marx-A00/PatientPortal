# Database Migrations Guide

This document outlines the steps for creating and applying database migrations in the Patient Portal application.

## Overview

Database migrations allow you to version control your database schema changes and apply them consistently across different environments. Entity Framework Core handles the migration process by generating SQL scripts based on your model changes.

## Adding a New Model to the Database

### Step 1: Add Model to DbContext

Before creating a migration, ensure your model is properly configured in the `ApplicationDbContext`. The model should be added as a `DbSet<T>` property.

**Example:**
```csharp
public class ApplicationDbContext : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    // ... other DbSets
}
```

### Step 2: Create a Migration

Generate a migration to capture the current state of your models and create the necessary SQL scripts:

```bash
dotnet ef migrations add AddPatientTable
```

**What this does:**
- Creates a new migration file in the `Migrations` folder
- Generates SQL scripts to add/modify database tables
- Includes both "Up" (apply changes) and "Down" (rollback changes) methods

**Migration File Location:**
- Generated files are stored in the `Migrations/` folder
- Files follow the naming convention: `YYYYMMDDHHMMSS_MigrationName.cs`

### Step 3: Apply the Migration to Database

Execute the migration to create the actual database tables:

```bash
dotnet ef database update
```

**What this does:**
- Executes the SQL scripts against your database
- Creates tables, columns, indexes, and constraints
- Updates the `__EFMigrationsHistory` table to track applied migrations

## Example: Patient Table Migration

For the Patient model, the migration creates a table with the following structure:

```sql
CREATE TABLE [Patients] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [DOB] datetime2 NULL,
    [Email] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Patients] PRIMARY KEY ([Id])
);
```

## Verification

### Check Migration Status

To verify that migrations have been applied successfully:

```bash
dotnet ef database update
```

If all migrations are up to date, you'll see: `No pending migrations.`

### View Applied Migrations

Check which migrations have been applied to your database:

```bash
dotnet ef migrations list
```

## Migration Best Practices

1. **Always review generated migrations** before applying them to production
2. **Test migrations** in a development environment first
3. **Keep migrations small and focused** - one logical change per migration
4. **Never modify existing migration files** that have been applied to production
5. **Use descriptive migration names** that explain what the migration does

## Common Migration Commands

| Command | Description |
|---------|-------------|
| `dotnet ef migrations add <Name>` | Create a new migration |
| `dotnet ef migrations remove` | Remove the last migration (if not applied) |
| `dotnet ef migrations list` | List all migrations |
| `dotnet ef database update` | Apply pending migrations |
| `dotnet ef database update <MigrationName>` | Update to a specific migration |
| `dotnet ef migrations script` | Generate SQL script for migrations |

## Troubleshooting

### Migration Already Applied
If you get an error about a migration already being applied, check the `__EFMigrationsHistory` table in your database.

### Database Connection Issues
Ensure your connection string in `appsettings.json` is correct and the database server is accessible.

### Model Changes Not Detected
Make sure your model changes are properly reflected in the DbContext and that you've rebuilt the project before creating migrations.

## Rollback Migrations

To rollback to a previous migration:

```bash
dotnet ef database update <PreviousMigrationName>
```

**Warning:** Rolling back migrations can result in data loss. Always backup your database before performing rollback operations.

---

*This documentation covers the basic migration workflow. For more advanced scenarios, refer to the [Entity Framework Core documentation](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/).* 