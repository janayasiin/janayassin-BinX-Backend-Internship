
### Day 2 — Mocking Dependencies with Moq

#### Completed Work

- Added **Moq** to the xUnit test project.
- Created `IPatientRepository` as a dependency interface for testing.
- Created `VitalSignAnalysisService` using dependency injection.
- Created a mocked repository using `Mock<IPatientRepository>`.
- Used `Setup()` and `ReturnsAsync()` to configure mocked return values.
- Used `ThrowsAsync()` to simulate repository exceptions.
- Tested how the service handles dependency failures.
- Used `Verify()` with `Times.Once` to verify repository interactions.
- Continued applying the **Arrange-Act-Assert (AAA)** pattern.
- Verified all automated tests successfully.

**Test Result:** `9 passed | 0 failed`

