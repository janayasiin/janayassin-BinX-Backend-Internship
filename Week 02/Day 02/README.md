# Day 2 — Advanced LINQ & Deferred Execution

## 📌 Overview
Today's lab focuses on mastering advanced LINQ operators for data manipulation and understanding the underlying execution mechanics in C#. We explored how to reshape collections using grouping, joining, and flattening, as well as the implications of deferred execution.

---

## 🛠️ Key Concepts Learned
1. **Deferred vs. Immediate Execution:** Understanding that LINQ queries are not evaluated until they are enumerated (e.g., via `foreach`, `ToList()`, or `Count()`), and how this impacts dynamic data changes.
2. **Grouping (`GroupBy`):** Clustering elements based on a specific key and aggregating data within groups (e.g., calculating total amounts per customer).
3. **Joining (`Join`):** Combining two related collections based on matching foreign/primary keys, mirroring SQL inner joins.
4. **Flattening (`SelectMany`):** Unwrapping nested collections (collections of collections) into a single, flat sequence of items.

---

## 💻 Implementation Highlights
- Created related `Customer` and `Order` models with a shared foreign key relationship and nested collection items.
- Implemented a `GroupBy` query to calculate total order spending per customer.
- Implemented a `Join` query to merge customer names with their respective order amounts.
- Implemented a `SelectMany` query to flatten all nested order items into a unified sequence.
- Demonstrated deferred execution by modifying the source list after query definition and observing the results upon enumeration.
