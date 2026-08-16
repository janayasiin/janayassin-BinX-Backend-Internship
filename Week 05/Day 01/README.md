## Day 1 — Phase 3 Project Selection & Unit Testing

### Project Scope

The **Cardiac Patient Monitoring System** is an ASP.NET Core REST API for managing patients, vital signs, medications, and appointments.

The project includes JWT authentication, input validation, centralized error handling, Entity Framework Core with SQL Server, and API documentation through Swagger and Postman.

The project also includes automated unit and integration tests using xUnit and Moq to verify critical API functionality.

### Completed Work

* Selected **Cardiac Patient Monitoring System** as the Phase 3 project.
* Created a separate **xUnit test project** and referenced the main API project.
* Created `VitalSignService` for unit testing.
* Applied the **Arrange-Act-Assert (AAA)** pattern.
* Added 3 `[Fact]` tests for heart-rate validation.
* Added a `[Theory]` test with multiple `[InlineData]` cases.
* Ran all tests successfully: **6 passed, 0 failed**.
