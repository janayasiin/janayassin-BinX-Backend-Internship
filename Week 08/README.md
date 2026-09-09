# Week 08 — Sprint 3: Advanced Queries & Performance

## Overview

Week 08 focused on improving the performance of the Cardiac Patient Monitoring System through EF Core query optimization, Redis caching, database indexing, and performance profiling.

## Day 1 — Sprint Planning & N+1 Diagnosis

* Reviewed Sprint 3 performance goals.
* Enabled EF Core SQL logging and request timing.
* Identified a genuine N+1 problem in the patient performance endpoint.
* Baseline: **58 patients → 59 SQL queries → 285 ms**.

## Day 2 — Query Optimization

* Fixed the N+1 problem using eager loading with `Include()`.
* Reduced database queries from **59 to 1**.
* Validated DTO projection using `Select()`.
* Tested `AsSplitQuery()` for multiple collection navigations.

## Day 3 — Redis Caching

* Integrated Redis using `IDistributedCache`.
* Implemented the Cache-Aside pattern.
* Added 5-minute cache expiration and cache invalidation.
* Tested cache hit/miss behavior.
* Benchmark: **~695 ms cache miss → ~24 ms cache hit**.

## Day 4 — Indexing & Profiling

* Added composite indexes for:

  * `VitalSigns (PatientId, RecordedAt)`
  * `Appointments (PatientId, AppointmentDate)`
* Inspected SQL Server execution plans.
* Confirmed `Index Seek` usage for the VitalSigns query.
* Documented the Appointments optimizer behavior.

## Day 5 — Sprint Review & Retrospective

* Reviewed Sprint 3 performance results.
* Presented before/after evidence.
* Documented remaining performance opportunities for Sprint 4.
* Completed the Sprint Retrospective.
* Defined a concrete Sprint 4 action: **add automated regression tests to prevent N+1 issues from returning**.

## Sprint 3 Key Results

| Area             | Result                   |
| ---------------- | ------------------------ |
| N+1 Query        | **59 → 1 SQL query**     |
| Redis Cache      | **~695 ms → ~24 ms**     |
| VitalSigns Index | **Index Seek confirmed** |

## Sprint 4 Focus

* Automated regression and integration testing.
* Performance testing with larger datasets.
* API documentation and deployment preparation.

## Conclusion

Sprint 3 established an evidence-based approach to improving backend performance through query optimization, caching, indexing, and profiling.
