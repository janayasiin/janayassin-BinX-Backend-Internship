# Week 3 — ASP.NET Core Web API & Entity Framework Core

## 🚀 Overview
During Week 3 of the Backend Engineering Internship, I built and developed the foundational core of the **Library Catalog API** using **ASP.NET Core Web API**, **Entity Framework Core**, and **SQL Server**. The implementation progressed systematically from database design to robust CRUD operations and comprehensive API testing.

---

## 📅 Daily Progress & Implementation

### **Day 2: SQL Server Schema Design & Normalization**
- Designed a relational database schema for a **Library Catalog** system.
- Defined core entities: `Authors`, `Categories`, `Books`, `Members`, and `Loans`.
- Applied normalization rules (**1NF, 2NF, 3NF**) to eliminate data redundancy.
- Established primary keys, foreign keys, and relationships (1-to-Many).

### **Day 3: Entity Framework Core Setup & Code-First Migrations**
- Created the ASP.NET Core Web API project (`MyFirstApi`).
- Installed EF Core packages (`Microsoft.EntityFrameworkCore.SqlServer` & `Tools`).
- Implemented C# entity classes with correct navigation properties.
- Configured `AppDbContext` and registered it in `Program.cs` via Dependency Injection.
- Generated and applied Code-First migrations (`InitialCreate`) targeting SQL Server LocalDB.

### **Day 4: Implementing CRUD Operations**
- Developed `BooksController` handling full **CRUD** operations for books.
- Utilized asynchronous programming (`async/await`, `ToListAsync`, `FirstOrDefaultAsync`, `SaveChangesAsync`).
- Implemented request **DTOs** (`CreateBookRequest`, `UpdateBookRequest`).
- Applied robust data validation using **DataAnnotations** (`[Required]`, `[StringLength]`, `[Range]`).
- Handled proper HTTP status codes (`200 OK`, `201 Created`, `204 No Content`, `400 Bad Request`, `404 Not Found`).

### **Day 5: API Testing & Documentation with Postman**
- Built a structured Postman Collection with folders for the `Books` resource.
- Covered both happy paths and error paths (Validation errors, Not Found cases).
- Added automated Postman JavaScript test scripts for status code and property assertions.
- Configured a Postman Environment with a dynamic `baseUrl` variable.

---

## 🛠️ Tech Stack
- **Framework:** ASP.NET Core Web API (.NET)
- **ORM:** Entity Framework Core (Code-First)
- **Database:** SQL Server (LocalDB)
- **Testing Tool:** Postman
