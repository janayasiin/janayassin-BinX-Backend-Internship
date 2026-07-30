# Day 4 & Day 5 — ASP.NET Core API, Middleware & Dependency Injection

In this lab and subsequent extensions, I built an ASP.NET Core Web API project, exploring both Controllers and Minimal APIs, custom middleware pipelines, and dependency injection service lifetimes.

## Implemented Features & Endpoints
- **Controller-based Endpoints:**
  - `GET /api/products` — Returns all products via `IProductService`.
  - `GET /api/products/{id}` — Returns a single product by its ID.
- **Minimal APIs (Program.cs):**
  - `GET /products`
  - `GET /products/{id}`
- **Dependency Injection:**
  - Created `IProductService` and `ProductService` to handle data isolation.
  - Registered the service with `AddScoped` and injected it into `ProductsController` via constructor injection.
- **Custom Middleware:**
  - Implemented `RequestLoggingMiddleware` to log incoming HTTP methods and request paths to the console.

All endpoints were tested successfully using Swagger UI and Postman.
