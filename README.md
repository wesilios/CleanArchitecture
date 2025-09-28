# CleanArchitecture
Clean Architecture Solution Template with Domain-Driven Design (DDD)

This project provides a foundational template for building a robust and maintainable application using Clean Architecture principles. The architecture is specifically designed to align with Domain-Driven Design (DDD), where the application's core logic is centered around a rich domain model. This structure ensures that the business rules are isolated, testable, and independent of external frameworks or databases.

## Architecture

Domain-Centric Development: The Domains layer is the heart of the application. It contains the core business rules and logic, defined by entities and value objects that model the real-world domain.


Separation of Concerns: The project is strictly separated into layers, ensuring that each component has a single, well-defined responsibility. This prevents coupling and allows for independent development and testing of each layer.

Dependency Rule: Dependencies always point inward. The Infrastructure and Application layers depend on the Domain layer, but the Domain layer has no dependencies on any external components. This "inward-pointing" rule keeps the core business logic from being polluted by technology choices.

### Project Structure
    .
    ├── .template.config/
    │   ├── template.json
    ├── src/ 
    │   ├── Domains/ 
    │   │   └── Domain/ # Domain entities, value objects, domain services 
    │   ├── Applications/
    │   │   └── Application/ # Use cases, interfaces, DTOs, business logic 
    │   └── Infrastructures/ 
    │       └── Infrastructure/ # External concerns (database, web API, etc.)
    ├── tests/
    │   └── Domains.UnitTests/ # Unit tests for domain layer
    ├── CleanArchitecture.sln # Solution file
    ├── global.json # .NET SDK version configuration
    └── README.md

### Layer Dependencies

- **Domain Layer**: Contains enterprise business rules and entities. Has no dependencies on other layers.
- **Application Layer**: Contains application business rules and use cases. Depends only on the Domain layer.
- **Infrastructure Layer**: Contains implementations for external concerns. Depends on Application and Domain layers.

## Features

- ✅ Clean Architecture structure
- ✅ .NET 8.0 support
- ✅ ASP.NET Core integration
- ✅ C# 12.0 features
- ✅ Razor Pages support
- ✅ Unit testing setup
- ✅ Solution template configuration

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Your favorite IDE (Visual Studio, VS Code, JetBrains Rider)

## License

This project is licensed with the [MIT license](LICENSE).