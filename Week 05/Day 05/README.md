# Day 5 — Testing & Week 5 Synthesis

## Overview

Applied testing practices to the selected Phase 3 project, focusing on high-risk business logic, integration testing, centralized error handling, and full test-suite execution.

## What Was Completed

- Identified the highest-risk areas of the project that require testing priority.
- Implemented unit tests for critical business logic using **xUnit** and **Moq**.
- Added integration tests for important API endpoints.
- Implemented and verified centralized error handling.
- Ran the complete test suite using `dotnet test`.
- Verified that the implemented tests pass successfully.
- Prepared the project foundation for **Phase 3 Sprint 1**.

## Testing Strategy

Testing was prioritized based on **risk and complexity**, with emphasis on:

- Business logic containing branching and validation.
- Authentication and authorization.
- Operations involving important data or transactions.
- Previously identified bug-prone functionality.

Simple pass-through code and trivial properties were not prioritized for extensive testing.

## Tools Used

- **xUnit** — Unit and integration testing
- **Moq** — Mocking dependencies
- **ASP.NET Core** — API development and testing
- **Notion** — Week 5 documentation and synthesis

## Full Test Suite

The complete test suite was executed using:

```bash
dotnet test
