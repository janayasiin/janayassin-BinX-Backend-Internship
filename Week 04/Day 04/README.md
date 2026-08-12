# Week 04 - Day 04

## Library Catalog API

### Topics Covered
- FluentValidation
- JWT Authentication
- Claims-based Authorization
- Custom Middleware
- API Testing with Swagger & Postman

### Validation
Implemented FluentValidation for:
- CreateBookRequest
- UpdateBookRequest

Validation includes:
- Required Title and ISBN
- Maximum length
- Price greater than 0
- Valid AuthorId
- Valid CategoryId

### Authentication & Authorization
- Configured ASP.NET Core Identity
- Added JWT Bearer Authentication
- Added `CanManageBooks` authorization policy
- Used `Permission = ManageBooks` claim

### Middleware
Added `RequestLoggingMiddleware` to log HTTP requests.

### Testing
Tested the API using Swagger and Postman, including:
- Valid requests
- Invalid requests
- Validation errors
- Authentication
- Authorization
- Book creation and update
