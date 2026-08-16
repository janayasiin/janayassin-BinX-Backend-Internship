# Cardiac Patient Monitoring System

A backend REST API built with ASP.NET Core for a cardiac patient monitoring system.  
The system provides a secure way to manage patient information, vital signs, medications, and appointments through authenticated API endpoints.

## Overview

The system is designed as a backend prototype for monitoring cardiac patients. It allows authorized users to manage patient profiles and track important health measurements such as heart rate, blood pressure, temperature, and oxygen saturation.

The API also provides medication and appointment management, with validation, authentication, and centralized error handling to keep the system secure and reliable.

## Main Features

- User registration and login using ASP.NET Core Identity and JWT.
- Protected API endpoints using JWT authentication.
- Patient management with full CRUD operations.
- Vital signs management including:
  - Heart rate
  - Blood pressure
  - Temperature
  - Oxygen saturation
- Medication management with patient-based filtering.
- Appointment management with patient and status filtering.
- DTOs for request and response data.
- Input validation using FluentValidation.
- Centralized exception handling middleware.
- Entity Framework Core with SQL Server.
- EF Core migrations and synthetic seed data.
- Unit testing using xUnit and Moq.
- API testing and documentation using Swagger and Postman.

## Technologies

- C#
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- JWT Authentication
- FluentValidation
- xUnit
- Moq
- Swagger / OpenAPI
- Postman

## Project Structure

```text
CardiacPatientMonitoringSystem/
├── Controllers/
├── Models/
├── DTOs/
├── Data/
├── Validators/
├── Middleware/
├── Migrations/
└── Program.cs

CardiacPatientMonitoringSystem.Tests/
├── AuthControllerTests.cs
└── PatientsControllerTests.cs
