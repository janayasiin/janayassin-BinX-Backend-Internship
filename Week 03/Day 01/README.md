# Day 1 — REST API Design Principles & Resource Modeling

## 1. Domain and Core Resources

### Domain:
Library Catalog

### Core Resources:
- Books
- Authors
- Members
- Loans
- Categories


## 2. REST Endpoints for Books

| HTTP Method | Endpoint | Description |
|------------|----------|-------------|
| GET | /api/v1/books | Get all books |
| GET | /api/v1/books/{id} | Get a specific book |
| POST | /api/v1/books | Create a new book |
| PUT | /api/v1/books/{id} | Update a book |
| DELETE | /api/v1/books/{id} | Delete a book |


## 3. Nested Resource

### Author's Books

| HTTP Method | Endpoint | Description |
|------------|----------|-------------|
| GET | /api/v1/authors/{id}/books | Get all books written by a specific author |


## 4. HTTP Status Codes

| Endpoint | Success Status Code | Error Status Code |
|----------|---------------------|-------------------|
| GET /api/v1/books | 200 OK | 500 Internal Server Error - Unexpected server failure |
| GET /api/v1/books/{id} | 200 OK | 404 Not Found - Book does not exist |
| POST /api/v1/books | 201 Created | 400 Bad Request - Invalid book data |
| PUT /api/v1/books/{id} | 200 OK | 404 Not Found - Book does not exist |
| DELETE /api/v1/books/{id} | 204 No Content | 404 Not Found - Book does not exist |


## 5. API Versioning

The API will use URL versioning.

Examples:

- Version 1: /api/v1/books

- Future Version: /api/v2/books

Using version numbers in the URL allows future changes to be introduced without breaking existing clients.


## 6. REST Principles Applied

- Resources are represented using plural nouns such as `/books` and `/authors`.
- HTTP methods define actions on resources instead of using verbs in URLs.
- Each request contains all required information, following the stateless REST principle.
- API versioning is used to support future changes without breaking existing clients.
