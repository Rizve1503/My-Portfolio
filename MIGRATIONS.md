# Entity Framework Core Migrations

## Prerequisites
Ensure you have the EF Core tools installed:
```powershell
dotnet tool install --global dotnet-ef
```

## Creating Initial Migration

From the solution root directory, run:

```powershell
dotnet ef migrations add InitialCreate --project RizvePortfolio.Infrastructure --startup-project RizvePortfolio.WebApi
```

## Applying Migrations

### Development (SQLite)
The application is configured to use `Database.EnsureCreated()` in Development mode for quick startup.

To use migrations instead, update Program.cs to use:
```csharp
dbContext.Database.Migrate();
```

Then apply migrations:
```powershell
dotnet ef database update --project RizvePortfolio.Infrastructure --startup-project RizvePortfolio.WebApi
```

### Production (SQL Server)
Update appsettings.json or appsettings.Production.json with SQL Server connection string:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=RizvePortfolioDB;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
  }
}
```

Then apply migrations:
```powershell
dotnet ef database update --project RizvePortfolio.Infrastructure --startup-project RizvePortfolio.WebApi --configuration Production
```

## Seeded Credentials

In Development mode, the following admin user is automatically seeded:

- **Email**: admin@portfolio.com
- **Password**: Adm12in@portf@lio.
- **Role**: Admin

## Common Migration Commands

```powershell
# List migrations
dotnet ef migrations list --project RizvePortfolio.Infrastructure --startup-project RizvePortfolio.WebApi

# Remove last migration (if not applied)
dotnet ef migrations remove --project RizvePortfolio.Infrastructure --startup-project RizvePortfolio.WebApi

# Generate SQL script
dotnet ef migrations script --project RizvePortfolio.Infrastructure --startup-project RizvePortfolio.WebApi --output migration.sql

# View database information
dotnet ef dbcontext info --project RizvePortfolio.Infrastructure --startup-project RizvePortfolio.WebApi
```
