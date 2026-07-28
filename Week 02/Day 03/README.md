# Day 3 — Async/Await Deep Dive & Concurrency Basics

This directory contains the practical lab work for Day 3, focusing on asynchronous programming, concurrency, and task cancellation in C# and .NET.

## Lab Objectives
* **Sequential Execution:** Understanding how synchronous or sequential `await` calls execute one after another, resulting in a total execution time equal to the sum of all individual delays.
* **Concurrent Execution (`Task.WhenAll`):** Learning how to start multiple independent tasks simultaneously and await them together, significantly reducing execution time to match the slowest task.
* **Task Cancellation (`CancellationToken`):** Implementing graceful operation cancellation using `CancellationTokenSource` and handling `OperationCanceledException`.

## Code Implementation (`Program.cs`)
The lab features realistic mock data loading scenarios (`GetUsersAsync`, `GetOrdersAsync`, `GetProductsAsync`) alongside a long-running process demonstrating cancellation tokens.
