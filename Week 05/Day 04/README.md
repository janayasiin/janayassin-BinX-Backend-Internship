# Day 4 — Centralized Error Handling & Global Exception Middleware

## Overview

Implemented centralized exception handling for the ASP.NET Core API using custom middleware.

## What Was Implemented

* Created `ExceptionHandlingMiddleware` to catch unhandled exceptions globally.
* Used `ILogger` for structured server-side error logging.
* Returned standardized `ProblemDetails` responses with HTTP `500`.
* Prevented internal exception messages and stack traces from being exposed to clients.
* Added a `TestController` endpoint to deliberately trigger an exception.
* Added an integration test to verify the middleware behavior.

## Error Handling Flow

```text
Request → Middleware → Controller → Exception
                         ↓
              Exception Middleware
                         ↓
              Log Error + ProblemDetails
                         ↓
                    HTTP 500
```

## Testing

Verified that:

* The test endpoint returns `500 Internal Server Error`.
* The actual exception message is not returned to the client.
* Exception details such as `System.Exception` are not exposed.

## Tools Used

* ASP.NET Core
* C#
* `ILogger`
* `ProblemDetails`
* xUnit
* WebApplicationFactory
