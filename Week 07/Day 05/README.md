# Day 5 — Sprint Review, Postman Demo & Retrospective

## Overview

Day 5 focused on closing Sprint 2 by demonstrating the implemented Authentication and Authorization flow, reviewing completed sprint requirements, identifying remaining work for Sprint 3, and documenting the Sprint 2 retrospective.

---

## 1. Authentication & Authorization Demo

The implemented authentication and authorization flow was reviewed and validated using Postman.

### Demonstrated Scenarios

* Patient Registration
* Patient Login and JWT token generation
* Accessing the authenticated patient's own resources
* Attempting to access another patient's resources
* Attempting to access an Admin-only endpoint using a Patient token

### Authorization Results

| Scenario                                    | Expected Result |
| ------------------------------------------- | --------------- |
| Patient accesses own resource               | `200 OK`        |
| Patient accesses another patient's resource | `403 Forbidden` |
| Patient accesses Admin-only endpoint        | `403 Forbidden` |

These scenarios demonstrate that the system correctly handles authentication, role-based authorization, and resource ownership.

### Authentication Flow

**Registration → Login → JWT Token → Authenticated Request → Authorization Check → Response**

---

## 2. Sprint 2 Review

The Sprint 2 implementation was reviewed against the defined requirements and acceptance criteria.

### Completed Work

* JWT Authentication
* Patient Registration and Login
* Role-Based Authorization (RBAC)
* Patient Resource Ownership Checks
* Admin Role Protection
* Protected Patient Endpoints
* Custom Request Timing Middleware
* Request Logging
* Critical Vital Sign Email Notifications

### Acceptance Criteria Review

| Requirement                       | Status    |
| --------------------------------- | --------- |
| Authentication                    | Completed |
| JWT Authorization                 | Completed |
| Role-Based Access Control         | Completed |
| Resource Ownership                | Completed |
| Admin Endpoint Protection         | Completed |
| Custom Middleware                 | Completed |
| Request Timing & Logging          | Completed |
| Critical Vital Sign Notifications | Completed |

---

## 3. Sprint 3 Backlog

Any remaining improvements or authorization edge cases identified during the Sprint Review will be carried forward into Sprint 3.

Planned direction:

* Improve cardiac patient monitoring capabilities
* Expand authorization and edge-case testing
* Improve backend reliability and architecture
* Continue strengthening the security of patient-related resources

---

## 4. Sprint 2 Retrospective

### What Went Well

* Implemented JWT-based authentication successfully.
* Established role-based authorization between Patients and Admins.
* Added ownership checks to prevent unauthorized access to other patients' resources.
* Implemented custom middleware for request timing and logging.
* Validated important authorization scenarios through Postman.

### What Could Be Improved

* Expand automated authorization and ownership tests.
* Increase coverage of edge cases.
* Continue improving separation of cross-cutting concerns.
* Perform more comprehensive security testing for new endpoints.

### Action for Sprint 3

**Add an explicit ownership-check test for every new patient resource endpoint.**

This will help ensure that authenticated users can only access resources belonging to them.

---

## 5. Sprint 2 Summary

Sprint 2 established the core security and backend infrastructure of the Cardiac Patient Monitoring System:

**Authentication → JWT → RBAC → Ownership Checks → Custom Middleware → Critical Vital Sign Notifications**

The Sprint 2 work was validated through Postman demonstrations and reviewed against the defined acceptance criteria.

The project is now ready to move forward with the Sprint 3 backlog and the next stage of cardiac monitoring functionality.
