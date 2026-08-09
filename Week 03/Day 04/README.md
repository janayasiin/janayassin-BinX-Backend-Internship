# Day 4 — Implementing CRUD Operations with EF Core

## 1. Overview & Objectives
- Implemented full **CRUD (Create, Read, Update, Delete)** operations for the `Books` resource using **Entity Framework Core**.
- Created Request **DTOs** (`CreateBookRequest`, `UpdateBookRequest`) to handle incoming data securely.
- Applied data annotations validation (`[Required]`, `[StringLength]`, `[Range]`).
- Developed `BooksController` with proper HTTP status codes (`200 OK`, `201 Created`, `204 No Content`, `400 Bad Request`, `404 Not Found`).
- Tested all endpoints thoroughly using Postman.

---

## 2. Endpoints Implemented (`BooksController`)

| Method | Endpoint | Description | Status Codes |
|--------|----------|-------------|--------------|
| `POST` | `/api/books` | Create a new book | `201 Created`, `400 Bad Request` |
| `GET` | `/api/books` | Retrieve all books | `200 OK` |
| `GET` | `/api/books/{id}` | Retrieve a single book by ID | `200 OK`, `404 Not Found` |
| `PUT` | `/api/books/{id}` | Update an existing book | `200 OK` / `204 No Content`, `400 Bad Request`, `404 Not Found` |
| `DELETE` | `/api/books/{id}` | Delete a book by ID | `204 No Content`, `404 Not Found` |

---

## 3. Postman Testing Checklist
- [x] Create Book (`POST`)
- [x] Get All Books (`GET`)
- [x] Get Book By Id (`GET`)
- [x] Get Book By Id - Not Found (`404`)
- [x] Update Book (`PUT`)
- [x] Update Book - Not Found (`404`)
- [x] Validation Error Handling (`400 Bad Request`)
- [x] Delete Book (`DELETE`)
- [x] Delete Book - Not Found (`404`)
