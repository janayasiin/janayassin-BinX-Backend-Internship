# Day 3 — Entity Framework Core Setup & Code-First Migrations

## 1. Overview & Objectives
- Installed and configured **Entity Framework Core** with **SQL Server (LocalDB)**.
- Defined C# entity classes and navigation properties matching the Day 2 relational schema.
- Created and configured `AppDbContext` with proper `DbSet` properties.
- Registered the database context in `Program.cs` using Dependency Injection.
- Generated and applied Code-First migrations (`InitialCreate`).

---

## 2. Project Structure
The project is organized under `Week 03/Day 03/MyFirstApi` with the following key components:
- **Models/**: Entity classes (`Author`, `Category`, `Book`, `Member`, `Loan`).
- **Data/**: `AppDbContext.cs` class.
- **appsettings.json**: Contains the database connection string.

---

## 3. Connection String Configuration
Configured in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=LibraryCatalogDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
