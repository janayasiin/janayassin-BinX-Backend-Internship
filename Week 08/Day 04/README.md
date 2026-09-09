# Day 4 — Database Indexing & Performance Profiling

## Overview

Day 4 focused on **database indexing and query performance profiling**.

The main objectives were to identify columns and column combinations that genuinely benefit from indexing, add appropriate indexes using **EF Core Fluent API**, apply the required migrations, and analyze query performance before and after indexing using **SQL Server execution plans and timing measurements**.

The indexes were selected based on actual query patterns rather than indexing every column indiscriminately.

---

## 1. Identifying Index Candidates

Based on the application's query patterns, the following candidates were identified as frequently filtered, joined, or looked-up fields:

* `Patients.UserId`
* `Appointments (PatientId, AppointmentDate)`
* `VitalSigns (PatientId, RecordedAt)`

### 1.1 Patients — UserId

`Patient.UserId` is used to associate a patient with the corresponding Identity user and is a frequent lookup field.

![Patients UserId Index Candidate](images/index-candidate-patient-userid.png)

The index is configured using EF Core Fluent API:

```csharp
modelBuilder.Entity<Patient>()
    .HasIndex(p => p.UserId);
```

The `Patients.UserId` index was already present in the initial database migration as a unique index, so no additional migration was required for it.

---

### 1.2 Appointments — PatientId + AppointmentDate

Appointment queries may filter by both the patient and the appointment date. Therefore, these two columns were selected as a **composite index** candidate.

![Appointments Composite Index Candidate](images/index-candidate-appointments-patient-date.png)

The composite index was configured as:

```csharp
modelBuilder.Entity<Appointment>()
    .HasIndex(a => new { a.PatientId, a.AppointmentDate });
```

This creates the following index:

```text
IX_Appointments_PatientId_AppointmentDate
```

Using a composite index allows the database to optimize queries that use this column combination together.

---

### 1.3 VitalSigns — PatientId + RecordedAt

Vital signs are associated with a patient and are time-based. Queries may need to retrieve vital signs for a specific patient according to their recording time.

Therefore, `(PatientId, RecordedAt)` was selected as another composite index candidate.

![VitalSigns Composite Index Candidate](images/index-candidate-vitals-patient-recordedat.png)

The index was configured as:

```csharp
modelBuilder.Entity<VitalSign>()
    .HasIndex(v => new { v.PatientId, v.RecordedAt });
```

This creates:

```text
IX_VitalSigns_PatientId_RecordedAt
```

---

# 2. Adding Indexes with EF Core Migrations

After configuring the indexes in `ApplicationDbContext`, EF Core migrations were used to apply the database schema changes.

## 2.1 Appointments Migration

The existing single-column `PatientId` index was replaced with the composite index:

```csharp
migrationBuilder.DropIndex(
    name: "IX_Appointments_PatientId",
    table: "Appointments");

migrationBuilder.CreateIndex(
    name: "IX_Appointments_PatientId_AppointmentDate",
    table: "Appointments",
    columns: new[] { "PatientId", "AppointmentDate" });
```

![Appointments Composite Index Migration](images/appointment-composite-index-migration.png)

The resulting index covers:

```text
PatientId + AppointmentDate
```

which matches the query pattern being evaluated.

---

## 2.2 VitalSigns Migration

The existing single-column `PatientId` index was replaced with the composite index:

```csharp
migrationBuilder.DropIndex(
    name: "IX_VitalSigns_PatientId",
    table: "VitalSigns");

migrationBuilder.CreateIndex(
    name: "IX_VitalSigns_PatientId_RecordedAt",
    table: "VitalSigns",
    columns: new[] { "PatientId", "RecordedAt" });
```

![VitalSigns Composite Index Migration](images/vitals-composite-index-migration.png)

The resulting index covers:

```text
PatientId + RecordedAt
```

---

## 2.3 Patients UserId Index

The `Patients.UserId` index was already created in the initial migration:

```csharp
migrationBuilder.CreateIndex(
    name: "IX_Patients_UserId",
    table: "Patients",
    column: "UserId",
    unique: true);
```

Therefore, when an additional migration was generated for this index, EF Core produced an empty migration because the index was already present in the model snapshot and initial migration.

No duplicate migration was needed.

---

# 3. Profiling Query Performance

Query performance was evaluated using two types of evidence:

1. **Timing measurements** recorded before indexing.
2. **SQL Server execution plans** inspected after indexing.

Execution plans provide concrete evidence of how SQL Server executes a query, such as whether it performs an `Index Seek` or a `Clustered Index Scan`.

---

# 4. Baseline Performance — Before Indexing

The following baseline measurements were recorded before applying the new composite indexes:

| Query                                        | SQL Time | Request Time |
| -------------------------------------------- | -------: | -----------: |
| `Patients.UserId`                            |     7 ms |        43 ms |
| `VitalSigns (PatientId + RecordedAt)`        |     9 ms |        72 ms |
| `Appointments (PatientId + AppointmentDate)` |    10 ms |        91 ms |

These measurements were used as the baseline for the subsequent analysis.

---

# 5. VitalSigns — After Indexing

After applying the `VitalSigns` composite index, the query execution plan was inspected.

The plan showed:

**`Index Seek (NonClustered)`**

on the `VitalSigns` table using the newly created composite index.

![VitalSigns Execution Plan After Index](images/baseline-vital-signs-after-index.png)

The execution plan also contained:

* `Index Seek (NonClustered)` on `Patients`
* `Index Seek (NonClustered)` on `VitalSigns`
* `Key Lookup (Clustered)` on `VitalSigns`

The important part of the plan is:

```text
Index Seek (NonClustered)
        ↓
IX_VitalSigns_PatientId_RecordedAt
```

This confirms that SQL Server selected the new composite index for the tested query.

### Performance Result

Before indexing:

```text
9 ms SQL / 72 ms request
```

After indexing, the execution plan displayed approximately:

```text
0.000s
```

Because the current dataset is relatively small and the execution time is extremely short, there is not enough evidence to claim a reliable percentage-based runtime improvement.

Instead, the execution plan provides clear evidence that the composite index is being used through an `Index Seek`.

---

# 6. Appointments — After Indexing

The following query was used to evaluate the `Appointments` composite index:

```sql
SELECT *
FROM [Appointments]
WHERE [PatientId] = @1
  AND [AppointmentDate] = @2;
```

After applying the composite index, the execution plan was inspected.

![Appointments Execution Plan After Index](images/appointments-execution-plan-after-index.png)

The plan showed:

**`Clustered Index Scan`**

on the `Appointments` table.

This means that SQL Server did **not** select the newly created composite index for this particular query.

### Why did SQL Server use a Clustered Index Scan?

This does not necessarily mean that the index is incorrect.

SQL Server uses a **cost-based query optimizer**. It compares different execution strategies and chooses the one it estimates to be cheaper.

With a small `Appointments` table, scanning the clustered index can be cheaper than using a non-clustered index and performing additional lookups.

Therefore, the optimizer selected:

```text
Clustered Index Scan
```

instead of:

```text
Index Seek
    ↓
IX_Appointments_PatientId_AppointmentDate
```

### Performance Result

Before indexing:

```text
10 ms SQL / 91 ms request
```

After indexing, the execution plan displayed approximately:

```text
0.000s
```

Since the current dataset is small, the available measurements do not support claiming a numerical performance improvement.

The documented result is therefore:

> The composite index was created successfully, but SQL Server did not select it for the tested query under the current data size and execution conditions.

---

# 7. Patients — UserId

The `Patients.UserId` index was already present in the initial migration and was therefore not treated as a newly added index.

The baseline lookup measurement was:

```text
7 ms SQL / 43 ms request
```

The index was identified as an appropriate candidate because `UserId` is used for frequent patient lookups.

![Patients UserId Index Candidate](images/index-candidate-patient-userid.png)

No additional migration was created because the index already existed in the database schema.

---

# 8. Before vs After Summary

| Index Candidate                             | Before                    | After                       | Result                            |
| ------------------------------------------- | ------------------------- | --------------------------- | --------------------------------- |
| `Patients.UserId`                           | 7 ms SQL / 43 ms request  | Existing index              | ✅ Index already present           |
| `VitalSigns (PatientId, RecordedAt)`        | 9 ms SQL / 72 ms request  | `Index Seek (NonClustered)` | ✅ Composite index used            |
| `Appointments (PatientId, AppointmentDate)` | 10 ms SQL / 91 ms request | `Clustered Index Scan`      | ⚠️ Index created but not selected |

> **Note:** The execution plans displayed approximately `0.000s` for the tested queries. Because of the small dataset and extremely short execution times, numerical percentage improvements were not considered reliable. Execution-plan behavior was therefore used as the main evidence of index utilization.

---

# 9. Key Findings

### Indexes should target real query patterns

Indexes should be added to columns that are frequently used in:

* `WHERE` clauses
* `JOIN` conditions
* `ORDER BY`
* Common lookup operations

Adding indexes to every column is not recommended because indexes also introduce storage and write-maintenance overhead.

### Composite indexes are useful for multi-column query patterns

The following composite indexes were added based on actual query patterns:

```text
(PatientId, AppointmentDate)
(PatientId, RecordedAt)
```

The column order was selected according to the filtering patterns of the tested queries.

### Index creation does not guarantee index usage

The `Appointments` index was successfully created, but SQL Server chose a clustered scan for the tested query.

This demonstrates that index usage depends on the query optimizer and the estimated cost of each execution strategy.

### Execution plans provide concrete evidence

The `VitalSigns` execution plan showed a non-clustered `Index Seek` using:

```text
IX_VitalSigns_PatientId_RecordedAt
```

This provides concrete evidence that SQL Server is using the composite index.

---

# 10. Conclusion

Day 4 successfully covered the database indexing and performance profiling requirements.

The work included:

* Identifying 2–3 relevant index candidates.
* Evaluating when indexes are appropriate.
* Adding composite indexes for multi-column query patterns.
* Configuring indexes using EF Core Fluent API.
* Applying the changes through EF Core migrations.
* Measuring baseline query performance.
* Inspecting SQL Server execution plans after indexing.
* Confirming composite index usage for `VitalSigns`.
* Analyzing the optimizer's decision for `Appointments`.
* Documenting the results based on actual evidence.

The main takeaway is that **database optimization should be evidence-based**. Creating an index is not enough; its effectiveness should be evaluated using query timings and execution plans to determine how the database engine actually executes the query.
