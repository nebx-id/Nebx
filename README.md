# 📦 nebx

`nebx` is a lightweight utility package that simplifies common **C# API development ceremonies**.  
It reduces boilerplate and provides consistent building blocks for modern API projects using patterns like **FluentValidation**, **MediatR**, and **Result Pattern**.

---

## ✨ Features

- ✅ **FluentValidation** – Automatically registers validators and integrates them into your pipeline.
- ✅ **MediatR** – Simple configuration for command/query handlers and domain event dispatching.
- ✅ **Domain-Driven Design (DDD)** – Models and aggregates with built-in support for MediatR `INotification`.
- ✅ **Minimal API Endpoints** – Register and organize endpoints with minimal ceremony.
- ✅ **Result Pattern** – Strongly typed `Result` and `ErrorResponse` objects for consistent API responses.
- ✅ **Global Error Handling** – Centralized exception-to-HTTP translation (including domain and infrastructure errors).
- ✅ **EF Core DbContext** – Simplified configuration with `AddScopedDbContext<TDbContext>()`, interceptors, and `IUnitOfWork`.
- ✅ **Pagination Utilities** – Easy helpers for paginated queries and results.

---

## 🚀 Getting Started

### Installation

```bash
dotnet add package nebx
