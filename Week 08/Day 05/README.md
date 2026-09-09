# Day 5 — Sprint Review, Benchmark Demo & Retrospective

## Overview

Day 5 focused on closing Sprint 3 by reviewing the implemented performance improvements, presenting measurable before/after evidence, documenting remaining opportunities, and defining a concrete action for Sprint 4.

---

## 1. Sprint 3 Performance Benchmarks

### N+1 Query Optimization

The endpoint `GET /api/Patients/performance/n-plus-one` was tested with 58 patients.

**Before:**

* 58 patients
* 59 SQL queries
* 285 ms

**After using eager loading with `Include()`:**

* 58 patients
* 1 SQL query
* 372 ms

The main improvement was reducing database round-trips:

> **59 queries → 1 query**

![N+1 Before](../Day%2001/images/n-plus-one-queries.png)

![N+1 After](../Day%2002/images/n-plus-one-after-include.png)

---

### Redis Caching

The endpoint `GET /api/Patients/statistics/count` was implemented using the **Cache-Aside pattern**.

* Cache key: `patients:count`
* Expiration: 5 minutes
* Cache invalidation tested after patient writes

| Scenario   | Response Time |
| ---------- | ------------: |
| Cache Miss |       ~695 ms |
| Cache Hit  |        ~24 ms |

The cache hit avoided the SQL `COUNT(*)` query.

![Cache Miss](../Day%2003/images/cache-miss.png)

![Cache Hit](../Day%2003/images/cache-hit.png)

---

### Database Indexing

Composite indexes were added for:

```text
VitalSigns: (PatientId, RecordedAt)
Appointments: (PatientId, AppointmentDate)
```

The VitalSigns execution plan confirmed that the composite index was used through an **Index Seek**.

For Appointments, the index was created but SQL Server selected a **Clustered Index Scan** for the tested query. Therefore, no performance improvement was claimed without sufficient evidence.

---

## 2. Sprint Review

| Task                           | Status      |
| ------------------------------ | ----------- |
| N+1 diagnosis and optimization | ✅ Completed |
| Projection                     | ✅ Completed |
| Split query evaluation         | ✅ Completed |
| Redis caching                  | ✅ Completed |
| Cache invalidation             | ✅ Tested    |
| Database indexing              | ✅ Completed |
| Performance profiling          | ✅ Completed |

All completed tasks were reviewed using the collected performance evidence.

---

## 3. Sprint 4 Backlog

The following performance opportunities will be carried into Sprint 4:

* Add automated regression tests for N+1 query issues.
* Test the Appointments index with a larger dataset.
* Automate repeatable performance checks.
* Add integration tests for caching and cache invalidation.

---

## 4. Sprint 3 Retrospective

### What Went Well

* Successfully identified and fixed the N+1 query problem.
* Reduced database queries from **59 to 1**.
* Redis caching showed a measurable improvement from **~695 ms to ~24 ms** on cache hit.
* Execution plans were used to verify index behavior.

### What Could Be Improved

* Use larger and more realistic datasets for performance testing.
* Repeat benchmarks under more consistent conditions.
* Protect performance improvements with automated tests.

### Concrete Action for Sprint 4

> **Add an automated regression test to prevent the N+1 problem from being reintroduced.**

---

## 5. Sprint 3 Outcome

Sprint 3 established an evidence-based approach to backend performance optimization through:

> **Query Optimization → Redis Caching → Database Indexing → Performance Profiling**

Key results:

* **N+1:** 59 → 1 SQL query
* **Redis:** ~695 ms → ~24 ms on cache hit
* **VitalSigns:** Composite index verified through Index Seek
