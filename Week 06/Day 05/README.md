# \## Sprint 1 Review

# 

# \### Sprint Goal

# 

# Implement and validate the core backend functionality of the Cardiac Patient Monitoring System, including authentication, patient management, vital signs, medications, appointments, and appointment notes.

# 

# \### Completed Work

# 

# \* Implemented JWT authentication and authorization.

# \* Implemented patient profile management.

# \* Implemented Vital Signs management.

# \* Implemented Medication CRUD operations and patient filtering.

# \* Implemented Appointment CRUD operations.

# \* Added appointment business rules:

# 

# &#x20; \* Appointments must be scheduled in the future.

# &#x20; \* Duplicate appointments at the same date and time are prevented.

# \* Added `AppointmentNote` related to an appointment.

# \* Implemented a database transaction for creating an appointment and its related note.

# \* Tested transaction rollback by intentionally exceeding the database note length limit and verifying that both the appointment and note were rolled back.

# \* Configured entity relationships using EF Core Fluent API.

# \* Added seed data for medical conditions.

# \* Tested the implemented API endpoints through Swagger and verified successful and error responses.

# 

# \### Sprint Review Outcome

# 

# The core Sprint 1 functionality was implemented and manually validated through Swagger.

# 

# Incomplete items were moved to the Sprint 2 backlog:

# 

# \* Expand automated unit and integration test coverage.

# \* Complete the pull request review and merge process.

# \* Address mentor feedback and any remaining code-review issues.

# \* Continue improving API validation and error handling where required.

# 

# \### Sprint 1 Documentation

# 

# The Sprint 1 documentation includes:

# 

# \* ERD of the implemented database schema.

# \* EF Core migration history.

# \* Implemented API features.

# \* Transaction and rollback validation.

# \* Sprint retrospective and Sprint 2 action item.

# 

\## Sprint 1 Retrospective



\### What Went Well



\* The core backend features were implemented successfully.

\* EF Core relationships were configured using Fluent API.

\* Business logic was added beyond simple CRUD operations.

\* Appointment creation was implemented as a multi-step database operation.

\* Transaction rollback was tested successfully by forcing the appointment note operation to fail.

\* API endpoints were manually tested through Swagger with successful and error cases.

\* The project structure remained organized using Controllers, Services, DTOs, Models, and Data layers.



\### What Could Be Improved



\* Automated tests should be written earlier instead of relying mainly on manual Swagger testing.

\* More unit and integration tests are needed for business logic and transaction behavior.

\* Pull request review and feedback should be incorporated earlier in the sprint.



\### Action for Sprint 2



\*\*Write automated tests for new business logic and error cases before considering each feature complete.\*\*



