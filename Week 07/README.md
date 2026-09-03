# Week 7 — Sprint 2: Authentication & Role-Based Access

## Overview

Week 7 focused on securing the **Cardiac Patient Monitoring System** by implementing authentication, authorization, role-based access control, resource ownership, and custom middleware.

## Sprint Goal

Implement a secure authentication and authorization layer for the existing capstone API.

## Completed Work

### Day 1 — Identity Integration

* Integrated ASP.NET Core Identity.
* Linked `Patient` with `ApplicationUser`.
* Added and applied Identity migration.
* Defined `Patient` and `Admin` roles.

### Day 2 — JWT Authentication

* Implemented Patient registration and login.
* Added JWT authentication.
* Added User ID, Patient ID, Email, and Role claims.
* Tested the registration and login flow with Postman.

### Day 3 — RBAC & Ownership

* Implemented Patient/Admin role-based authorization.
* Added resource ownership checks.
* Protected Patient-specific resources.
* Seeded the Admin account.
* Tested successful and rejected authorization scenarios.

### Day 4 — Custom Middleware

* Implemented Request Timing & Logging Middleware.
* Registered it in the request pipeline.
* Tested middleware across successful and unauthorized requests.
* Added critical vital-sign email notifications.

### Day 5 — Sprint Review

* Demonstrated Authentication and RBAC using Postman.
* Validated `200 OK` and `403 Forbidden` scenarios.
* Reviewed Sprint 2 requirements.
* Completed the Sprint Retrospective.
* Defined an action item for Sprint 3.

## Key Technologies

* C#
* ASP.NET Core
* ASP.NET Core Identity
* JWT
* Entity Framework Core
* SQL Server
* Postman
* Git & GitHub

## Sprint Outcome

Sprint 2 established a secure foundation for the Cardiac Patient Monitoring System:

**Identity → JWT → RBAC → Ownership Checks → Custom Middleware**

The project is ready to move into Sprint 3 with a focus on performance, caching, and further monitoring capabilities.
