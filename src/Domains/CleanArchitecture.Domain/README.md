## Domain Project

In Clean Architecture, the central focus should be on Entities and business rules.

In Domain-Driven Design (DDD), this is called the Domain Model.

This project is the heart of your application and should contain all of your:

- Entities 
- Value Objects 
- Interfaces for repositories 
- Domain Services 
- And all other core business logic.

Key Concepts:
- Aggregates: Entities that are closely related and should be treated as a single unit for data changes are grouped into an Aggregate.
- Encapsulation: Entities should leverage encapsulation and should minimize public setters to protect their internal state and enforce business invariants.
- Domain Events: Entities can leverage Domain Events to clearly communicate changes and side effects to other parts of the system (typically to the Application Layer).
- Specifications: Entities can define Specifications (a design pattern) that encapsulate query logic, which can then be used to retrieve them.
- Repository Pattern: For mutable access (creating, updating, or deleting entities), they should be accessed through an Abstract Repository interface defined in this project.
- Query Services: Read-only ad hoc queries often bypass the full Domain Model and use separate, simpler Query Services (defined in the Application Layer) for efficiency.