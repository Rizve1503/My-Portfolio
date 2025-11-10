# RizvePortfolio (Clean Architecture, .NET 8)

## Solution Structure
- **RizvePortfolio.Domain**: Pure domain entities (`Category`, `PortfolioItem`, base auditing).
- **RizvePortfolio.Application**: DTOs, service interfaces & implementations (`IPortfolioService`), repository abstractions (`IRepository`, `IUnitOfWork`), DI extension.
- **RizvePortfolio.Infrastructure**: EF Core `AppDbContext`, Identity `ApplicationUser`, generic `Repository<T>`, `UnitOfWork`, DI extension.
- **RizvePortfolio.WebApi**: ASP.NET Core entry point, controllers (`PortfolioItemsController`), configuration, DI composition.

## Layer Dependencies
```
Domain <- Application <- Infrastructure <- WebApi (composition)
Domain <- Infrastructure (via entities only) is referenced but domain remains persistence-agnostic.
```
WebApi references Application & Infrastructure; Application references Domain only; Infrastructure references Application & Domain.

## Getting Started
1. Ensure .NET 8 SDK installed.
2. Update connection string in `RizvePortfolio.WebApi/appsettings.json` if needed.
3. Run database migrations (add them first – not yet included):
```powershell
# Example (after adding migrations):
dotnet ef migrations add Initial --project RizvePortfolio.Infrastructure --startup-project RizvePortfolio.WebApi
 dotnet ef database update --project RizvePortfolio.Infrastructure --startup-project RizvePortfolio.WebApi
```
4. Run the API:
```powershell
dotnet run --project RizvePortfolio.WebApi
```

## Example Endpoints
- `GET /api/PortfolioItems` – list items
- `GET /api/PortfolioItems/{id}` – get item
- `POST /api/PortfolioItems` – create item
- `PUT /api/PortfolioItems/{id}` – update item
- `DELETE /api/PortfolioItems/{id}` – delete item

Body for create/update:
```json
{
  "title": "My Work",
  "summary": "Short summary",
  "content": "Detailed content...",
  "categoryId": "<guid>"
}
```

## Next Improvements
- Add migration & database initialization.
- Introduce FluentValidation for request models.
- Add Unit tests for Application services.
- Implement caching and pagination.
- Add JWT authentication and authorization policies.

## Notes
- Identity is registered; `ApplicationUser` ready for future auth flows.
- OpenAPI (Swashbuckle-style) placeholder via minimal `Microsoft.AspNetCore.OpenApi` package.
