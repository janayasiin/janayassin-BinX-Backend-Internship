# Day 5 — Applying Testing to the Project

## Overview

Applied unit and integration testing to the Cardiac Patient Monitoring System, focusing on high-risk logic and important API behavior.

## Testing Implemented

- Unit tests for `VitalSignService`.
- Unit tests for `VitalSignAnalysisService` using Moq.
- Validation tests for `CreateAppointmentRequestValidator`.
- Integration tests for the `Appointments` API.
- In-Memory Database configuration for integration testing.

## Integration Test Scenarios

- Existing patient → `201 Created`
- Non-existing patient → `404 Not Found`

## Test Results

```bash
dotnet test
