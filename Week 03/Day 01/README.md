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
| GET | /api/books | Get all books |
| GET | /api/books/{id} | Get a specific book |
| POST | /api/books | Create a new book |
| PUT | /api/books/{id} | Update a book |
| DELETE | /api/books/{id} | Delete a book |

## 3. Nested Resource

### Author's Books

| HTTP Method | Endpoint | Description |
|------------|----------|-------------|
| GET | /api/authors/{id}/books | Get all books written by a specific author |

## 4. HTTP Status Codes

| Endpoint | Success Status Code | Error Status Code |
|----------|---------------------|-------------------|
| GET /api/books | 200 OK | 401 Unauthorized |
| GET /api/books/{id} | 200 OK | 404 Not Found |
| POST /api/books | 201 Created | 400 Bad Request |
| PUT /api/books/{id} | 200 OK | 404 Not Found |
| DELETE /api/books/{id} | 204 No Content | 404 Not Found |

## 5. API Versioning

The API will use URL versioning.

Example:

/api/v1/books

Using version numbers in the URL allows future changes to be introduced without breaking existing clients.

Future versions can be added like:

/api/v2/books
