# Day 1 — Generics & Advanced Collections

## 📌 Overview
Today's lab focuses on implementing **Generics** and **Advanced Collections** in C#. By leveraging generic constraints and appropriate collection interfaces, we build type-safe, reusable, and secure data access structures.

---

## 🛠️ Key Concepts Learned
1. **Generics (`<T>`):** Eliminating code duplication and ensuring compile-time type safety without relying on `object` casting.
2. **Generic Constraints (`where T : class`):** Restricting generic types to reference types to ensure safety and allow null checks.
3. **Collection Interfaces (`IReadOnlyList<T>` & `IEnumerable<T>`):** Encapsulating internal data storage and preventing external modification (API protection).
4. **Predicates (`Func<T, bool>`):** Implementing flexible, LINQ-powered filtering and searching mechanisms.

---

## 💻 Implementation Highlights
- Created a generic `Repository<T>` class constrained to reference types.
- Implemented `Add()`, `GetAll()` (returning `IReadOnlyList<T>`), and `Find()` methods.
- Tested the repository using multiple domain models (`Product` and `User`).
