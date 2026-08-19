# Day 3 — Integration Testing

## Overview

Implemented integration testing for the ASP.NET Core API using xUnit and `WebApplicationFactory`.

## What Was Implemented

- Configured `WebApplicationFactory` for API integration testing.
- Added an isolated EF Core InMemory database for tests.
- Configured test JWT authentication.
- Added authenticated API endpoint tests using `HttpClient`.
- Tested successful patient retrieval.
- Tested `404 Not Found` when a patient does not exist.

## Technologies

- .NET 9
- ASP.NET Core
- xUnit
- WebApplicationFactory
- Entity Framework Core InMemory
- JWT Authentication

## Test Results

```text
Total: 11
Passed: 11
Failed: 0
Skipped: 0
