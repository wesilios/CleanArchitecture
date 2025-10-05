# CleanArchitecture

Welcome to your new project generated with the Clean Architecture template!

## Getting Started

- Build the solution:
  ```sh
  dotnet build
  ```
- Run the application:
  ```sh
  dotnet run --project src/Presentations/CleanArchitecture.Presentation.Web/
  ```
- Adding migrations (if using EF Core):
  ```sh
  dotnet ef migrations add InitialCreate \
    -p src/CleanArchitecture.Infrastructure/ \
    -s src/CleanArchitecture.Api/ \
    -o DataAccess/Migrations/ \
    -v
  ```
- Update the database (if using EF Core):
  ```sh
  dotnet ef database update \
    -c CleanArchitectureDbContext \
    -p src/CleanArchitecture.Infrastructure/ \
    -s src/CleanArchitecture.Api/ \
    -v
  ```

## Solution Structure

- **Domains**: Domain entities, value objects, interfaces
- **Applications**: Application logic, CQRS handlers
- **Infrastructures**: Data access, external services
- **Presentations**: API endpoints, Console APP, gRPC services, Razor Pages, SPA Blazor Pages

## Contact

For any questions or concerns, please contact [Que Nguyen] at [quenguyen10190@gmail.com].
