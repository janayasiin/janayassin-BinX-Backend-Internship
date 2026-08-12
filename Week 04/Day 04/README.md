# Week 04 - Day 04

## Input Validation with FluentValidation

### Topics Covered
- DataAnnotations vs. FluentValidation
- Writing FluentValidation validators
- Integrating validation into the ASP.NET Core pipeline
- Structured validation errors

### Implementation
- Installed FluentValidation and ASP.NET Core integration.
- Created validators for:
  - `CreateBookRequest`
  - `UpdateBookRequest`
- Added business validation rules for Title, ISBN, Price, AuthorId, and CategoryId.
- Registered validators to run automatically during request validation.

### Testing
Tested each validation rule individually using Postman and verified that invalid requests return structured `400 Bad Request` responses with meaningful field-specific error messages.

### Tools
- FluentValidation
- Postman
