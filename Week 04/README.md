# Week 4 — Authentication, Identity & Security

## Overview

Week 4 focused on securing the Library Catalog API by adding authentication, authorization, input validation, and API hardening.

## Completed

* ASP.NET Core Identity with user registration and password hashing.
* JWT authentication and token expiration.
* Protected routes using `[Authorize]`.
* Role-based authorization with `User` and `Admin` roles.
* Policy-based authorization using custom permission claims.
* FluentValidation for Create and Update requests.
* Structured `400 Bad Request` validation responses.
* Rate limiting with stricter protection for login.
* Named CORS policy with configurable allowed origins.
* HTTPS redirection, HSTS, and Content Security Policy.
* SQL injection review confirming the use of EF Core/LINQ without unsafe raw SQL.

## Week 4 Security Flow

```text
Authentication
      ↓
Authorization
      ↓
Validation
      ↓
Rate Limiting / CORS
      ↓
Secure Data Access
```

## Result

The Library Catalog API was secured with authentication, authorization, validation, and production-oriented API hardening, completing the main security requirements of Week 4.
