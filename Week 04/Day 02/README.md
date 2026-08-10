# Week 4 — Day 2: JWT Authentication & Token Issuance

## Overview

Implemented JWT authentication on top of ASP.NET Core Identity.

The API now supports user login, JWT token generation, token validation, and protected endpoints.

## What Was Implemented

* Added JWT Bearer authentication.
* Implemented login using `SignInManager`.
* Generated JWT tokens after successful authentication.
* Added User ID and Email claims to the token.
* Configured JWT issuer, audience, signing key, and token lifetime.
* Protected API endpoints using `[Authorize]`.
* Configured JWT secrets using ASP.NET Core User Secrets.
* Set access token expiration to 15 minutes.

## Authentication Flow

```text
Login Request
     ↓
Find User
     ↓
Verify Password using Identity
     ↓
Create JWT Claims
     ↓
Generate Signed JWT
     ↓
Return Token
     ↓
Client sends Bearer Token
     ↓
JWT Bearer Middleware validates Token
     ↓
[Authorize] Endpoint
```

## JWT Claims

The issued token contains:

* `sub` — User ID
* `email` — User Email
* `exp` — Token expiration time

## API Endpoints

### Register

```http
POST /api/auth/register
```

### Login

```http
POST /api/auth/login
```

Returns a JWT access token when the credentials are valid.

### Protected Books Endpoint

```http
GET /api/books
```

Requires a valid JWT Bearer token.

## Testing

The following scenarios were tested using Postman:

| Scenario                            | Expected Result        |
| ----------------------------------- | ---------------------- |
| Valid login                         | `200 OK` + JWT         |
| Invalid credentials                 | `401 Unauthorized`     |
| Protected endpoint without token    | `401 Unauthorized`     |
| Protected endpoint with valid token | `200 OK`               |
| JWT claims                          | User ID, Email, Expiry |
| Token lifetime                      | 15 minutes             |

## Security

The JWT signing key is stored using **ASP.NET Core User Secrets** and is not stored in the repository.

Passwords are handled by **ASP.NET Core Identity**, which performs password hashing and validation.

## Tools Used

* ASP.NET Core
* ASP.NET Core Identity
* JWT Bearer Authentication
* Entity Framework Core
* SQL Server
* Postman
* Swagger
* C#
