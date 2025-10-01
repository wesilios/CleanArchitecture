# Chroma API Backend Project Plan & Requirements (Final Version with Implementation Details)

The Chroma project utilizes Clean Architecture for the backend (ASP.NET Core Web API) to manage Colors and
ColorPalettes. It employs a custom CQRS pattern and Repository pattern for maximum control and separation of concerns.

## Phase 1: Core API & Domain Implementation (MVP)

This phase establishes the architectural foundation, implements the strict Domain Model, and delivers the minimal CRUD
API.

1.1. Architectural & Development Requirements

| Aspect       | Principle/Technology                      | Implementation Detail                                                                         |
|--------------|-------------------------------------------|-----------------------------------------------------------------------------------------------|
| Architecture | Clean Architecture (ASP.NET Core Web API) | Strict separation of Domain, Application, Infrastructure, and Presentation layers.            |
| Data Flow    | CQRS (Custom Implementation)              | No external library dependency (e.g., MediatR). Defined by ICommandHandler and IQueryHandler. |
| Mapping      | Manual Mapping                            | Explicit conversion between DTOs, Commands, and Domain Entities. No AutoMapper dependency.    |
| Data Access  | Repository Pattern                        | IColorRepository and IPaletteRepository interfaces defined in the Domain layer.               |
| Scope        | Minimal CRUD                              | Implement all C/R/U/D operations for both Color and ColorPalette.                             |

1.2. Domain Model & Business Rules

| Entity          | Role Key     | Constraints/Rules                                                                                |
|-----------------|--------------|--------------------------------------------------------------------------------------------------|
| Color           | Value Object | Defined by RGB and Opacity (A). The composite (R,G,B,A) must be globally unique.                 |
| ColorPalette    | Entity       | Manages a collection of Colors via a Many-to-Many relationship.                                  |
| Maximum Size    | Domain Rule  | A Palette must contain a maximum of 5 Color entities. Enforced by the ColorPalette entity logic. |
| Junction Entity | PaletteColor | Links Palette and Color. Must include OrderIndex to maintain display sequence.                   |

3. Implementation Details: Core Patterns
   3.1. Repository Pattern (Discussion and Example)
   The repository pattern dictates that the Domain Layer defines the contract (interface) while the Infrastructure Layer
   provides the concrete implementation. This shields business logic from database technology.

Domain Layer Contract (Example):

```C#

// Domain Layer (Application.Contracts/Repositories)
public interface IPaletteRepository
{
    Task<Palette> GetByIdAsync(int id);
    Task AddAsync(Palette palette);
    // Methods must deal ONLY with Domain Entities/Value Objects
}
```

Infrastructure Layer Implementation (Discussion):
The concrete class (e.g., PaletteRepository : IPaletteRepository) is where EF Core or Dapper code lives. It handles the
translation from database models (or DTOs) back into the Palette Domain Entity before returning it.

3.2. CQRS Manual Implementation (Discussion and Example)
To avoid external library dependencies like MediatR, the Application Layer will define and use simple handler interfaces
and rely on the built-in ASP.NET Core Dependency Injection (DI) container for registration and resolution.

Application Layer Interfaces:

```C#

// Application Layer (Use Cases/Interfaces)
public interface ICommandHandler<TCommand>
{
    Task HandleAsync(TCommand command); // Write operations
}

public interface IQueryHandler<TQuery, TResult>
{
    Task<TResult> HandleAsync(TQuery query); // Read operations
}
```

Application Layer Handler (Example):
The handler uses the repository defined in the Domain layer.

```C#

// Application Layer (Use Cases/Handlers)
public class CreatePaletteCommandHandler : ICommandHandler<CreatePaletteCommand>
{
private readonly IPaletteRepository _repository;
public CreatePaletteCommandHandler(IPaletteRepository repository) => _repository = repository;

    public async Task HandleAsync(CreatePaletteCommand command)
    {
        // 1. Manual Mapping (Command DTO to Domain Entity)
        var newPalette = new Palette(command.Name);

        // 2. Domain Rule Check: Max 5 colors checked inside the Palette entity
        // (The entity itself manages the list of colors)

        await _repository.AddAsync(newPalette);
    }
}
```

3.3. Manual Mapping (Discussion and Example)
Manual mapping provides compile-time safety and full control over data transfer, mitigating risks associated with
reflection-based mappers. Mapping occurs primarily at the boundaries of the Application Layer (Command/Query to Entity)
and the Presentation Layer (Request DTO to Command/Query, Entity to Response DTO).

Example: Mapping Domain Entity to Response DTO (Within a Query Handler):

The ColorPalette Domain Entity is mapped to a flatter PaletteResponseDto for API consumption.

```C#

// Mapping from Domain Entity to Presentation DTO
public static PaletteResponseDto MapToDto(Palette palette)
{
    return new PaletteResponseDto
    {
        Id = palette.Id,
        Name = palette.Name,
        // Explicitly map the collection and the Value Object properties
        Colors = palette.PaletteColors.Select(pc => new ColorDto
            {
                Id = pc.Color.Id,
                R = pc.Color.R,
                G = pc.Color.G,
                B = pc.Color.B,
                A = pc.Color.A,
                OrderIndex = pc.OrderIndex
            }).ToList()
        };
    }
```

## Phase 2: Client Application and Enhanced Presentation

This phase develops the client application, focusing on decoupled communication with the API and advanced data
visualization.

4.1. Client Application Requirements

| Requirement             | Detail                     | Rationale/Benefit                                                                                                                                                             |
|-------------------------|----------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Client Type             | Console Application (.NET) | A standalone Presentation Layer consumer.                                                                                                                                     |
| Data Consumption Method | Call the API (HTTP)        | Preferred Method. The Console App acts as an external client, communicating via HTTP requests. This ensures strict decoupling and adherence to Clean Architecture boundaries. |
| Presentation Tool       | Spectre.Console            | Used for rich, structured, and visually appealing console output (tables, styles, color blocks).                                                                              |
| Color Integration       | Color Block Rendering      | The client converts the Color DTO's R,G,B,A values into a Spectre.Console.Color object to draw visual color blocks.                                                           |

4.2. Console Client Mockup (Spectre.Console Implementation)
The client uses Spectre.Console tables and styling to display data clearly.

A. Global Color Collection Display (GET /api/colors)

| ID   | RGB Values	     | Color Block      | Opacity (A) |
|------|-----------------|------------------|-------------|
| C101 | rgb(255,0,0)    | 	███ (Red)       | 1.0         |
| C102 | rgb(0,0,0)      | 	███ (Black)	    | 0.1         |
| C105 | rgb(218,165,32) | 	███ (Goldenrod) | 1.0         |

------------------------------------------------------
B. Palette Detail Display (GET /api/palettes/P201)

```
======================================================
PALETTE DETAIL: 'Minimalist Sunrise' (ID: P201)
------------------------------------------------------
| Status: Max Colors: 5/5 (FULL) | Created: 2025-09-30 |
------------------------------------------------------
| ORDER | Color Block | RGBA Values              | Use/Role        |
|-------|-------------|--------------------------|-----------------|
| 1     | ███         | rgb(240, 248, 255, 0.95) | Neutral Base    |
| 2     | ███         | rgb(0, 0, 0, 0.1)        | Shadow/Text Dim |
| 3     | ███         | rgb(255, 0, 0, 1.0)      | Primary Accent  |
| 4     | ███         | rgb(255, 165, 0, 0.85)   | Secondary       |
| 5     | ███         | rgb(255, 255, 0, 1.0)    | High Contrast   |
------------------------------------------------------
```