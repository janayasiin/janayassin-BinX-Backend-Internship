# Day 01 — ASP.NET Core Identity & User Registration

## Overview

This day focused on setting up **ASP.NET Core Identity** with Entity Framework Core and implementing a user registration endpoint.

## Learning Objectives

* Understand what ASP.NET Core Identity provides.
* Configure Identity with Entity Framework Core.
* Implement user registration using `UserManager`.
* Understand how Identity handles password hashing and validation.

## Technologies

* ASP.NET Core Web API
* ASP.NET Core Identity
* Entity Framework Core
* SQL Server
* Postman
* .NET 9

## Implementation

### 1. Identity Setup

The existing `AppDbContext` was updated to inherit from:

```csharp
IdentityDbContext<IdentityUser>
```

This allows ASP.NET Core Identity to manage its own database tables alongside the application's existing entities.

### 2. Identity Configuration

Identity was registered in `Program.cs` using:

```csharp
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();
```

Authentication middleware was also added:

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

### 3. Database Migration

A new EF Core migration was created and applied to the database.

This added Identity tables such as:

* `AspNetUsers`
* `AspNetRoles`
* `AspNetUserRoles`
* `AspNetUserClaims`
* `AspNetUserLogins`
* `AspNetUserTokens`
* `AspNetRoleClaims`

### 4. User Registration

A `RegisterRequest` DTO was created with:

* Email
* Password

A registration endpoint was implemented:

```text
POST /api/auth/register
```

The endpoint uses:

```csharp
UserManager<IdentityUser>
```

and:

```csharp
CreateAsync()
```

to create the user.

### 5. Password Security

Password hashing is handled automatically by ASP.NET Core Identity.

No custom password hashing logic was implemented.

Identity also validates passwords according to its configured password policy.

## Testing

Registration was tested using Postman.

### Successful Registration

A valid email and password returned a successful response:

```text
200 OK
```

### Weak Password

A deliberately weak password was tested.

Identity rejected the request and returned validation errors such as:

* `PasswordTooShort`
* `PasswordRequiresNonAlphanumeric`
* `PasswordRequiresLower`

This confirmed that Identity's built-in password validation is working correctly.

## Result

Day 01 successfully implemented:

* ASP.NET Core Identity
* EF Core Identity integration
* Identity database schema
* User registration
* Password validation
* Secure password hashing through Identity
* Successful and failed registration testing with Postman
