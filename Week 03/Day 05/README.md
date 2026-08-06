# Day 5 — Testing & Documenting the API with Postman & Week 3 Synthesis

## 1. Overview & Objectives
- Built and organized a complete **Postman Collection** covering all API endpoints with structured folders.
- Tested both **Success (Happy Paths)** and **Error Paths** (such as `400 Bad Request` and `404 Not Found`).
- Added automated **Postman Test Scripts (JavaScript)** to assert expected HTTP status codes and response properties.
- Configured a Postman **Environment and Variables** (`baseUrl`) for smooth switching between local and remote environments.
- Completed the Week 3 integration wrap-up and documentation.

---

## 2. Postman Collection Structure
- **Collection Name:** `Library Catalog API - Week 3`
- **Folder:** `Books`
  - `Get All Books` (200 OK)
  - `Get Book By Id` (200 OK)
  - `Get Book By Id - Not Found` (404 Not Found)
  - `Create Book` (201 Created)
  - `Create Book - Validation Error` (400 Bad Request)
  - `Update Book` (204 No Content / 200 OK)
  - `Update Book - Not Found` (404 Not Found)
  - `Update Book - Invalid Input` (400 Bad Request)
  - `Delete Book` (204 No Content)
  - `Delete Book - Not Found` (404 Not Found)

---

## 3. Postman Environment Variables
- Created an environment named `Local Development`.
- Variable configured:
  - `baseUrl` = `https://localhost:7043`

---

## 4. Automated Test Scripts Example
Added JavaScript assertions to test responses automatically:
```javascript
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});

pm.test("Response has an id property", function () {
    pm.expect(pm.response.json()).to.have.property("id");
});
