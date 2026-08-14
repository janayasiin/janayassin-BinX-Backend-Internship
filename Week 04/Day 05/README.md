# Day 5 — Securing the API: Rate Limiting, CORS & Security Headers

## Overview

Day 5 focused on hardening the Library Catalog API against common security risks by implementing rate limiting, configuring CORS, adding security headers, and reviewing the API for SQL injection vulnerabilities.

## 1. Rate Limiting

ASP.NET Core's built-in rate limiting middleware was configured using fixed-window policies.

Two policies were created:

```text
GeneralPolicy → 100 requests per minute
LoginPolicy   → 5 requests per minute
```

The login endpoint uses a stricter limit because repeated login attempts can be used for brute-force attacks.

Requests that exceed the configured limit return:

```text
429 Too Many Requests
```

The middleware is enabled with:

```csharp
app.UseRateLimiter();
```

### Rate Limiting Flow

```text
Client
  ↓
Rate Limiter
  ↓
Within limit? ── No ──→ 429 Too Many Requests
  ↓ Yes
Authentication / Authorization
  ↓
Controller
```

## 2. CORS Configuration

A named CORS policy called `AllowFrontend` was configured.

Allowed frontend origins are stored in `appsettings.json`:

```json
"AllowedOrigins": [
  "https://localhost:3000"
]
```

The policy reads the allowed origins from configuration:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
```

The policy is applied through:

```csharp
app.UseCors("AllowFrontend");
```

A permissive `AllowAnyOrigin()` policy was avoided because production APIs should restrict browser-based clients to known origins.

## 3. Security Headers

### HTTPS Redirection

The API already uses HTTPS redirection:

```csharp
app.UseHttpsRedirection();
```

This redirects HTTP requests to HTTPS.

### HSTS

HSTS was enabled outside the Development environment:

```csharp
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}
```

HSTS instructs browsers to use HTTPS for the application.

### Content Security Policy

A Content Security Policy header was added:

```text
Content-Security-Policy: default-src 'self'
```

The header was verified through the API response headers.

## 4. SQL Injection Prevention

The Library API was reviewed for raw SQL queries.

No `FromSql` usage or unparameterized raw SQL queries were found.

The project primarily uses Entity Framework Core and LINQ, for example:

```csharp
var book = await _context.Books
    .FirstOrDefaultAsync(b => b.Id == id);
```

EF Core parameterizes values used in LINQ queries, which helps prevent SQL injection.

Raw SQL should only be used with proper parameterization. User input should never be concatenated directly into a SQL string.

## 5. Final Security Configuration

The API now includes:

* Rate limiting
* Stricter rate limiting for login
* Named CORS policy
* HTTPS redirection
* HSTS for non-development environments
* Content Security Policy
* SQL injection review

## Key Takeaways

Rate limiting controls **how many requests** a client can make.

CORS controls **which browser origins** are allowed to call the API.

Security headers provide additional browser-level protections.

EF Core and LINQ provide parameterized queries by default, while unsafe raw SQL can bypass that protection.

## Result

The Library Catalog API was hardened against common request-abuse, browser-based, and database-related security risks while keeping security concerns centralized in the API configuration and middleware pipeline.
