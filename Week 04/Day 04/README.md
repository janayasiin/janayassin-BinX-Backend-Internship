# Day 04 — Input Validation with FluentValidation

## Overview

This day focused on implementing input validation in ASP.NET Core using **FluentValidation**.

The goal was to move validation rules out of the DTOs and define them in separate validator classes, including business rules that go beyond simple required-field checks.

---

## Learning Objectives

- Understand the difference between DataAnnotations and FluentValidation.
- Create validators using `AbstractValidator<T>`.
- Define meaningful validation and business rules.
- Integrate FluentValidation into the ASP.NET Core request pipeline.
- Return structured validation errors.
- Test validation behavior using Postman.

---

## DataAnnotations vs FluentValidation

Initially, the request DTOs used DataAnnotations such as:

- `[Required]`
- `[StringLength]`
- `[Range]`

For this exercise, these validation attributes were removed from the DTOs and the validation logic was moved into dedicated FluentValidation classes.

This keeps the DTOs focused on representing request data while validation rules are maintained separately.

---

## Validators

Two validators were implemented:

```text
Validators/
├── CreateBookRequestValidator.cs
└── UpdateBookRequestValidator.cs
