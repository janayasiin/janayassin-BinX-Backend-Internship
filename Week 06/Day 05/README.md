# Sprint 1 Review

## 🎯 Sprint Goal

Implement and validate the core backend functionality of the **Cardiac Patient Monitoring System**, including:

* Authentication & Authorization
* Patient Management
* Vital Signs Management
* Medication Management
* Appointment Management
* Appointment Notes

---

## ✅ Completed Work

### 🔐 Authentication & Authorization

* Implemented **JWT Authentication and Authorization**.
* Configured role-based access where required.

### 👤 Patient Management

* Implemented patient profile management.
* Added API endpoints for managing patient information.

### ❤️ Vital Signs

* Implemented **Vital Signs management**.
* Added endpoints for creating, retrieving, updating, and managing patient vital signs.

### 💊 Medications

* Implemented **Medication CRUD operations**.
* Added patient-based medication filtering.

### 📅 Appointments

* Implemented **Appointment CRUD operations**.
* Added appointment business rules:

  * Appointments must be scheduled in the future.
  * Duplicate appointments at the same date and time are prevented.

### 📝 Appointment Notes

* Added the `AppointmentNote` entity and its relationship with appointments.
* Implemented appointment creation with its related note as a **single database transaction**.
* Tested transaction rollback by intentionally exceeding the database note length limit.
* Verified that when the note operation failed, **both the appointment and note were rolled back successfully**.

### 🗄️ Database & EF Core

* Configured entity relationships using **EF Core Fluent API**.
* Added seed data for medical conditions.
* Created and maintained EF Core migrations.

### 🧪 API Validation

* Tested the implemented API endpoints using **Swagger**.
* Verified both successful responses and expected error responses.

---

## 📊 Sprint Review Outcome

The core **Sprint 1 functionality was successfully implemented and manually validated through Swagger**.

The following incomplete items were moved to the **Sprint 2 backlog**:

* Expand automated unit and integration test coverage.
* Complete the Pull Request review and merge process.
* Address mentor feedback and remaining code-review issues.
* Continue improving API validation and error handling where required.

---

# 📚 Sprint 1 Documentation

The Sprint 1 documentation includes:

* 📌 ERD of the implemented database schema.
* 📌 EF Core migration history.
* 📌 Implemented API features.
* 📌 Transaction and rollback validation.
* 📌 Sprint retrospective.
* 📌 Sprint 2 action items.

---

# 🔄 Sprint 1 Retrospective

## ✅ What Went Well

* The core backend features were implemented successfully.
* EF Core relationships were configured using **Fluent API**.
* Business logic was implemented beyond basic CRUD operations.
* Appointment creation was implemented as a **multi-step database operation**.
* Transaction rollback was successfully tested by intentionally causing the appointment note operation to fail.
* API endpoints were manually tested through **Swagger** with both successful and error scenarios.
* The project structure remained organized using:

  * Controllers
  * Services
  * DTOs
  * Models
  * Data

---


