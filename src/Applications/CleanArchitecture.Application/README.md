## Application Layer Project

In Clean Architecture, the Application Layer (or Application Services) project is a relatively thin layer that wraps the
domain model.

Application services are typically organized by feature. These may be simple CRUD operations or much more complex
activities.

Application services should not depend directly on infrastructure concerns, making them simple to unit test in most
cases.

Application services are often grouped into Commands and Queries, following CQRS (Command Query Responsibility
Segregation).

Having the Application Layer as a separate project can reduce the amount of logic in UI and Infrastructure projects.

For simpler projects, the Application Layer can be omitted, and its behavior moved into the UI project, either as
separate services or CQRS handlers, or by simply putting the logic into the API endpoints.