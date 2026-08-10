# Day 3 — JWT Authorization & Role-Based Access Control

## Overview

In Day 3, JWT authentication was extended with authorization and role-based access control. Protected API endpoints were configured to allow or deny access based on the authenticated user's role and claims.

## What Was Implemented

### 1. Protected Routes

The `BooksController` was protected using the `[Authorize]` attribute.

Authenticated users must provide a valid JWT to access the Books endpoints.

```csharp
[Authorize]
public class BooksController : ControllerBase
```

Requests without a valid token return:

```text
401 Unauthorized
```

### 2. Role-Based Authorization

Two Identity roles were created:

* `User`
* `Admin`

The Delete endpoint was restricted to administrators:

```csharp
[Authorize(Roles = "Admin")]
[HttpDelete("{id}")]
```

An authenticated User can access the API but cannot delete books.

Expected result:

```text
Admin → Allowed
User  → 403 Forbidden
```

### 3. Claims-Based Authorization

The JWT contains claims for the authenticated user's:

* User ID
* Email
* Role

Admin users also receive:

```text
Permission = ManageBooks
```

This permission is used by the authorization policy.

### 4. Authorization Policy

A named policy called `CanManageBooks` was configured in `Program.cs`.

```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanManageBooks", policy =>
        policy.RequireClaim("Permission", "ManageBooks"));
});
```

The Update endpoint uses this policy:

```csharp
[Authorize(Policy = "CanManageBooks")]
[HttpPut("{id}")]
```

This means that the user must have the required `ManageBooks` permission.

### 5. JWT Role and Permission Claims

During login, the user's roles are retrieved from ASP.NET Core Identity.

Admin users receive both the Admin role claim and the `ManageBooks` permission claim.

```text
Admin
 ├── Role = Admin
 └── Permission = ManageBooks

User
 └── Role = User
```

### 6. Postman Testing

Protected endpoints were tested using Postman.

The following authorization scenarios were verified:

| Scenario                                            | Expected Result          |
| --------------------------------------------------- | ------------------------ |
| Request without JWT                                 | `401 Unauthorized`       |
| Valid User accessing protected endpoint             | `200 OK` where permitted |
| User attempting Admin-only action                   | `403 Forbidden`          |
| Admin accessing Admin endpoint                      | `200 OK`                 |
| Admin updating a book with `ManageBooks` permission | `200 OK`                 |

### 7. Postman Environment

A Postman environment was configured with an `accessToken` variable.

After a successful login, the returned JWT is stored automatically and reused by protected requests:

```text
{{accessToken}}
```

This avoids manually copying the JWT for every request.

## Key Concepts Demonstrated

* `[Authorize]`
* JWT Bearer Authentication
* Role-Based Access Control (RBAC)
* Claims-Based Authorization
* Policy-Based Authorization
* ASP.NET Core Identity Roles
* JWT Claims
* HTTP `401 Unauthorized`
* HTTP `403 Forbidden`
* Postman Bearer Token authentication
* Postman Environment Variables

## Day 3 Result

The API now supports authenticated and authorized access using JWTs, Identity roles, claims, and authorization policies. Protected endpoints were verified through Postman using different user permissions.
