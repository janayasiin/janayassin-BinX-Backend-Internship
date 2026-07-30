# Week 2 Synthesis — C# Advanced, LINQ & ASP.NET Core Basics

## 1. Advanced C# & Language Features (Days 1–3)
- **Generics & Collections:** Built reusable and type-safe data structures, practicing efficient data manipulation.
- **LINQ (Language Integrated Query):** Applied declarative querying methods (`Where`, `Select`, `FirstOrDefault`, etc.) to filter and transform collections cleanly.
- **Async/Await & Concurrency:** 
  - Explored asynchronous programming fundamentals.
  - Compared sequential execution with concurrent execution using `Task.WhenAll` to optimize performance.
  - Implemented operation cancellation handling using `CancellationTokenSource`.

## 2. ASP.NET Core Fundamentals & Web APIs (Days 4–5)
- **Project Scaffolding & Routing:** Initialized a Web API project using `dotnet new webapi`, setting up routing, route parameters, and HTTP verbs.
- **Controllers vs. Minimal APIs:** Compared structured, attribute-routed Controller classes with direct lambda-mapped Minimal APIs in `Program.cs`.
- **Middleware Pipeline:** Built a custom request-logging middleware (`RequestLoggingMiddleware`) and analyzed pipeline ordering constraints.
- **Dependency Injection (DI):** 
  - Designed services using interfaces (`IProductService`).
  - Configured service lifetimes (`AddScoped`) and applied constructor injection inside controllers.

