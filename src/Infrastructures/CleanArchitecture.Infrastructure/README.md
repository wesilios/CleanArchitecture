## Infrastructure Project

In Clean Architecture, infrastructure concerns are kept separate from the core business rules (or Domain Model in DDD).

The only project that should have code concerned with EF Core, Files, Email, Web Services, Azure/AWS/GCP, etc. is the
Infrastructure project.

The Infrastructure project should depend on the Domain project (and, optionally, the Application Layer) where
abstractions (interfaces) exist.

Infrastructure classes implement interfaces found in the Domain (or Application Layer) project(s).

These implementations are wired up at startup using Dependency Injection (DI). In this case, using
`Microsoft.Extensions.DependencyInjection` and extension methods defined in each project.