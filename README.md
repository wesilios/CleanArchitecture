# CleanArchitecture
Clean Architecture Solution Template with Domain-Driven Design (DDD)

This project provides a foundational template for building a robust and maintainable application using Clean Architecture principles. The architecture is specifically designed to align with Domain-Driven Design (DDD), where the application's core logic is centered around a rich domain model. This structure ensures that the business rules are isolated, testable, and independent of external frameworks or databases.

## Architecture

Domain-Centric Development: The Domains layer is the heart of the application. It contains the core business rules and logic, defined by entities and value objects that model the real-world domain.

Separation of Concerns: The project is strictly separated into layers, ensuring that each component has a single, well-defined responsibility. This prevents coupling and allows for independent development and testing of each layer.

Dependency Rule: Dependencies always point inward. The Infrastructure and Application layers depend on the Domain layer, but the Domain layer has no dependencies on any external components. This "inward-pointing" rule keeps the core business logic from being polluted by technology choices.

### Project Structure
    .
    ├── samples/                                                # Sample projects and documentation
    ├── src/                                                    # Source code
    │   ├── Domains/
    │   │   └── CleanArchitecture.Domain/                       # Domain entities, value objects, domain services
    │   ├── Applications/
    │   │   └── CleanArchitecture.Application/                  # Use cases, interfaces, DTOs, business logic
    │   ├── Infrastructures/
    │   │   ├── CleanArchitecture.Infrastructure/               # Core infrastructure implementations
    │   │   └── CleanArchitecture.Infrastructure.Azure/         # Azure-specific infrastructure
    │   ├── Presentations/
    │   │   ├── CleanArchitecture.Api/                          # RESTful API (ASP.NET Core)
    │   │   ├── CleanArchitecture.Grpc/                         # gRPC services
    │   │   ├── CleanArchitecture.Web/                          # Web UI (Razor Pages)
    │   │   └── CleanArchitecture.Jobs/                         # Background jobs and workers (to be added)
    ├── tests/
    │   └── CleanArchitecture.Domain.UnitTests/                 # Unit tests for Domain layer
    ├── CleanArchitecture.sln                                   # Solution file
    ├── global.json                                             # .NET SDK version configuration
    └── README.md

### Layer Dependencies

- **Domain Layer** (`CleanArchitecture.Domain`): Contains enterprise business rules, entities, value objects, and domain services. Has no dependencies on other layers.
- **Application Layer** (`CleanArchitecture.Application`): Contains application business rules, use cases, and interfaces. Depends only on the Domain layer.
- **Infrastructure Layer**: Contains implementations for external concerns:
  - `CleanArchitecture.Infrastructure`: Core infrastructure implementations (data access, etc.)
  - `CleanArchitecture.Infrastructure.Azure`: Azure-specific implementations
  - Depends on Application and Domain layers.
- **Presentation Layer**: Contains user interfaces and entry points. Depends on Application and Domain layers.
  - `CleanArchitecture.Api`: RESTful API (ASP.NET Core Web API)
  - `CleanArchitecture.Grpc`: gRPC services for high-performance communication
  - `CleanArchitecture.Web`: Web UI using Razor Pages
- **Jobs Layer** (planned): Background jobs and workers for asynchronous processing.

### Domain Layer Implementation

The Domain layer includes:

- **Value Objects**: Rich domain objects with business logic (e.g., Color with RGB and opacity)
- **Common**: Base classes like `ValueObject` for implementing value object equality
- **Constants**: Domain-specific constants
- **Exceptions**: Custom domain exceptions

## Features

- ✅ Clean Architecture structure with proper layer separation
- ✅ .NET 8.0 support
- ✅ Multiple presentation options (Web API, gRPC, Razor Pages)
- ✅ C# 12.0 features with global using statements
- ✅ Domain-Driven Design (DDD) implementation
- ✅ Value Objects with business logic (e.g., Color with opacity management)
- ✅ Unit testing setup with xUnit, FluentAssertions, and Shouldly
- ✅ Azure infrastructure support
- ✅ Dependency Injection configuration per layer
- ✅ Structured logging with Serilog
- ✅ Multiple environment configurations

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Your favorite IDE (Visual Studio, VS Code, JetBrains Rider)

## License

This project is licensed with the [MIT license](LICENSE).