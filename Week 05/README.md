# Week 5 — Testing, Error Handling & Project Kickoff

## Overview

Week 5 focused on testing, error handling, and applying production-ready practices to the Cardiac Patient Monitoring System API.

The main goal was to build a reliable test suite, isolate business logic using mocks, test real HTTP endpoints, and implement centralized error handling.

---

## Day 1 — Project Scope & Unit Testing with xUnit

- Defined the project scope for the Cardiac Patient Monitoring System.
- Created an xUnit test project.
- Applied the Arrange-Act-Assert (AAA) pattern.
- Used `[Fact]` for individual test scenarios.
- Used `[Theory]` with `[InlineData]` for multiple input cases.
- Tested vital sign business logic and validation rules.

---

## Day 2 — Mocking with Moq

- Used Moq to isolate services from external dependencies.
- Mocked `IPatientRepository`.
- Tested `VitalSignAnalysisService` independently from the database.
- Tested successful and failure scenarios.
- Used `Verify()` to confirm repository interactions.

---

## Day 3 — Integration Testing

- Implemented integration tests using `WebApplicationFactory`.
- Created a custom test application factory.
- Used an in-memory database for testing.
- Tested real HTTP endpoints.
- Tested:
  - Successful requests.
  - Not Found responses.
  - Authenticated endpoints.
  - Error responses.
- Generated a test JWT and attached it to authenticated requests.

---

## Day 4 — Centralized Error Handling

- Implemented `ExceptionHandlingMiddleware`.
- Centralized unhandled exception handling.
- Added structured logging using `ILogger`.
- Standardized error responses using `ProblemDetails`.
- Prevented internal exception messages and stack traces from being exposed to clients.
- Added an endpoint to deliberately trigger an exception and verify the middleware.

---

## Day 5 — Testing & Week 5 Synthesis

- Applied unit and integration testing to the project.
- Identified high-risk business logic and API paths.
- Tested both successful and error scenarios.
- Ran the complete test suite using:

```bash
dotnet test
