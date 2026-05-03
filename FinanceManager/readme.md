# Personal Finance Manager - Design Document

Console application for managing expenses, income, and budgets with relational database persistence. This document satisfies the **Design Document**, **Database Design**, and high-level **Setup / Test** expectations for the Clean Code Learning Assignment.

---

## 1. Assignment Alignment


| Deliverable        | Location / Notes                                                        |
| ------------------ | ----------------------------------------------------------------------- |
| Source Code        | Application project (to be added alongside this folder).                |
| Design Document    | This file + UML artifacts in this folder.                               |
| Database Design    | [Section 6](#6-database-design) and `ERDiagram.png` / `ERDiagram.puml`. |
| Setup Instructions | [Section 8](#8-setup-instructions).                                     |
| Test Cases         | [Section 9](#9-test-cases-mandatory).                                   |


### 1.1 Objective

Demonstrate clean coding practices, structured problem-solving, and scalable system design through a **Personal Finance Manager** with a console UI and persistent storage.

### 1.2 Problem Statement

Build a system to manage **expenses**, **income**, and **budgets** via the console, with data surviving application restarts using a **relational database**.

---

## 2. Functional Requirements


| #   | Requirement                                                       | Design response                                                                                                       |
| --- | ----------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------- |
| 1   | **Expense management**: add, view, filter (category/date), delete | `ExpenseService`, `IExpenseRepository`, `IExpenseFilter` strategies, commands for each menu action.                   |
| 2   | **Income management**: add and view                               | `IIncomeRepository`, income command(s), shared `Transaction` model with `TransactionType`.                            |
| 3   | **Budget management**: set and track                              | `IBudgetRepository` / `BudgetService` (see activity diagram), `Category.BudgetAmount`, `ExpenseService.IsOverBudget`. |
| 4   | **Summary**: totals, balance, category-wise spending              | `SummaryService` (class diagram), aggregations over repositories or dedicated queries.                                |


### 2.1 Non-functional Requirements

- **Persistence**: SQLite (or equivalent) via infrastructure layer; connection configuration externalized (no hardcoded secrets).
- **Interface**: Clear prompts, validation before calling services where appropriate, actionable error messages.
- **Design**: Modular structure, separation of concerns, loose coupling, extensibility (new commands/filters/repositories without rewriting core menu logic).

---

## 3. Architecture Overview

The solution uses **layered architecture** aligned with the class and sequence diagrams:

```text
┌─────────────────────────────────────────┐
│  Presentation (Console UI)              │  ConsoleMenu, ICommand, validators
├─────────────────────────────────────────┤
│  Application (Services + interfaces)    │  ExpenseService, BudgetService, SummaryService,
│                                         │  IExpenseRepository, IIncomeRepository, IExpenseFilter
├─────────────────────────────────────────┤
│  Domain                                 │  Transaction, Category, TransactionType
├─────────────────────────────────────────┤
│  Infrastructure                         │  *RepositoryImpl, SqliteDbContext
└─────────────────────────────────────────┘
```

**Dependency rule**: UI and services depend on **abstractions** (interfaces), not concrete database types. Infrastructure implements those interfaces.

### 3.1 UML Artifacts (Visual Design)


| Artifact                             | File                                          | Purpose                                                                   |
| ------------------------------------ | --------------------------------------------- | ------------------------------------------------------------------------- |
| Class diagram                        | `ClassDiagram.png`, `ClassDiagram.puml`       | Types, layers, relationships, patterns.                                   |
| Sequence diagram (Add Expense)       | `SequenceDiagram.png`, `SequenceDiagram.puml` | End-to-end flow: menu → command → validation → service → repository → DB. |
| Activity diagram (Set/Update Budget) | `ActivityDiagram.png`, `ActivityDiagram.puml` | Validation loops and layer boundaries for budget updates.                 |
| ER diagram                           | `ERDiagram.png`, `ERDiagram.puml`             | Tables, keys, cardinality, constraints.                                   |


---

## 4. Recommended Design Patterns

These patterns match the diagrams and assignment goals (modularity, testability, clean boundaries).

### 4.1 Command Pattern (Presentation)

- `**ICommand`** with `Execute()`; `**ConsoleMenu**` holds a map of menu keys to commands.
- **Why**: Decouples the menu from actions; adding a feature means adding a command class, not growing a single `switch` monolith.
- **Examples**: `AddExpenseCommand`, `ViewSummaryCommand`, plus further commands for list/delete/filter/income/budget.

### 4.2 Repository Pattern (Application + Infrastructure)

- `**IExpenseRepository`**, `**IIncomeRepository**` (and budget/category persistence as needed) live in the application boundary; `**ExpenseRepositoryImpl**` / `**IncomeRepositoryImpl**` implement them using `SqliteDbContext`.
- **Why**: Business logic stays free of SQL details; persistence can be swapped or mocked in tests.

### 4.3 Strategy Pattern (Filtering)

- `**IExpenseFilter`** with `Apply(...)`; concrete strategies `**DateRangeFilter**`, `**CategoryFilter**` (and more if needed).
- **Why**: Filter rules are composable and open for extension without modifying repository internals.

### 4.4 Service Layer (Application)

- `**ExpenseService`**, `**BudgetService**`, `**SummaryService**` orchestrate use cases: validation of business rules (e.g. **check budget before saving expense** per sequence diagram), aggregation for summaries.
- **Why**: Keeps commands thin and puts rules in one testable place per bounded operation.

### 4.5 Dependency Injection (Constructor Injection)

- Services receive `IExpenseRepository`, `IBudgetRepository`, etc.; commands receive services via constructors.
- **Why**: Satisfies loose coupling and enables unit tests with fakes/stubs.

### 4.6 Optional Patterns (Use When Complexity Justifies)


| Pattern             | When to use                                                                                                                  |
| ------------------- | ---------------------------------------------------------------------------------------------------------------------------- |
| **Factory**         | Building the command dictionary or DbContext from configuration in one place (`CompositionRoot`).                            |
| **Specification**   | If query rules grow beyond simple strategy composition.                                                                      |
| **Result / Either** | Uniform handling of success/failure from validation or persistence without exceptions for control flow (language-dependent). |


---

## 5. Key Behavioral Flows

### 5.1 Add Expense (Sequence)

1. User selects Add Expense → `**ConsoleMenu`** invokes `**AddExpenseCommand.Execute()**`.
2. Command collects input → `**InputValidator**` returns parsed amount and date.
3. `**ExpenseService.AddNewExpense**` runs business rules (`**CheckBudget**` / `IsOverBudget` as applicable).
4. Service calls `**IExpenseRepository.AddExpense**`; infrastructure executes SQL **INSERT** into **Transactions**.
5. User sees success or a clear error message.

See `SequenceDiagram.png` for the full message flow.

### 5.2 Set / Update Budget (Activity)

1. User chooses Set/Update Budget.
2. **UI**: List categories → validate category ID (exists and **expense** type) → loop on error.
3. **UI**: Prompt budget amount → validate numeric and **> 0** → loop on error.
4. **Application**: `BudgetService.Update(categoryId, amount)`.
5. **Infrastructure**: Update `**Categories`** row (`budget_amount`).
6. **UI**: Confirm success.

See `ActivityDiagram.png`.

---

## 6. Database Design

### 6.1 ER Model Summary

- `**Categories`** (1) — classifies — (**0..*** ) `**Transactions`**.

### 6.2 Table: `Categories`


| Column          | Type         | Notes                                            |
| --------------- | ------------ | ------------------------------------------------ |
| `category_id`   | INTEGER PK   | Surrogate key.                                   |
| `name`          | TEXT         | e.g. Food, Salary, Rent.                         |
| `type`          | TEXT         | Constrained to `**Income**` or `**Expense**`.    |
| `budget_amount` | DECIMAL NULL | Optional; meaningful for **expense** categories. |


### 6.3 Table: `Transactions`


| Column           | Type       | Notes                                                 |
| ---------------- | ---------- | ----------------------------------------------------- |
| `transaction_id` | INTEGER PK | Surrogate key.                                        |
| `type`           | TEXT       | Income / Expense; must align with category semantics. |
| `date`           | DATE       | Transaction date.                                     |
| `amount`         | DECIMAL    | Positive amount; sign policy documented in code.      |
| `description`    | TEXT       | Optional detail.                                      |
| `category_id`    | INTEGER FK | References `Categories.category_id`.                  |


### 6.4 Domain Mapping

- `**Category`** ↔ `Categories`; `**Transaction**` ↔ `Transactions`; `**TransactionType**` enum ↔ `type` columns.
- Enforce category type vs transaction type in **service** or **DB CHECK** constraints as appropriate.

DDL scripts or migrations should live with the source project (e.g. `scripts/` or ORM migrations).

---

## 7. Clean Code & Project Constraints

- **Naming**: Intention-revealing names for commands, services, and repositories.
- **Functions**: Small, single-purpose; validate at UI where input is captured; enforce invariants in services.
- **DRY**: Shared validation helpers (e.g. parsers, numeric checks) without copy-paste across commands.
- **No hardcoding**: Connection strings, file paths, and magic menu keys from configuration or constants in one module.
- **Error handling**: Distinguish user input errors from persistence failures; avoid silent catches; log or surface messages suitable for console.
- **Edge cases**: Empty lists, unknown category IDs, zero/negative amounts, invalid dates, DB locked/unavailable.

---

## 8. Setup Instructions

*(Adjust paths and commands to your chosen stack, e.g. .NET / Java / Node.)*

1. **Prerequisites**: Runtime/SDK for your language; SQLite (or bundled provider).
2. **Clone** the repository and open the solution/project containing the console app.
3. **Configure**: Set connection string or database file path via environment variable, `appsettings.json`, or `.env` (no secrets in source).
4. **Initialize database**: Run provided migration/DDL so `Categories` and `Transactions` exist with constraints.
5. **Seed data** *(optional)*: Default categories for demo.
6. **Run**: Execute the console project from the IDE or CLI.
7. **Tests**: Run the test project (see Section 9).

---

## 9. Test Cases (Mandatory)

Participants must define and automate tests where feasible. Suggested coverage matrix:

### 9.1 Expense Management


| Scenario                                              | Expected                                 |
| ----------------------------------------------------- | ---------------------------------------- |
| Valid add (valid category, date, amount, description) | Persisted; success path.                 |
| Invalid amount (non-numeric, negative, zero)          | Rejected at validation; no DB write.     |
| Invalid date                                          | Rejected; no DB write.                   |
| Unknown category ID                                   | Rejected with clear message.             |
| Delete existing / non-existing expense                | Removed or “not found” behavior defined. |


### 9.2 Income Management


| Scenario                     | Expected                     |
| ---------------------------- | ---------------------------- |
| Valid add to income category | Persisted.                   |
| Add to expense-only category | Rejected per business rules. |
| View when empty / with rows  | Correct listing.             |


### 9.3 Budget Management


| Scenario                          | Expected                                |
| --------------------------------- | --------------------------------------- |
| Valid update on expense category  | `budget_amount` updated.                |
| Update on income category         | Rejected (per activity diagram intent). |
| Invalid category ID               | Error loop / message.                   |
| Invalid amount (non-numeric, ≤ 0) | Error per activity diagram.             |


### 9.4 Filtering & Queries


| Scenario             | Expected                          |
| -------------------- | --------------------------------- |
| Filter by date range | Only transactions in range.       |
| Filter by category   | Only matching category.           |
| Combined filters     | Intersection (document behavior). |


### 9.5 Summary Calculations


| Scenario       | Expected                                                       |
| -------------- | -------------------------------------------------------------- |
| Known fixtures | Totals, balance, category totals match hand-calculated values. |
| No data        | Zero / empty-state handling.                                   |


### 9.6 Persistence & Failure Handling


| Scenario                | Expected                                                             |
| ----------------------- | -------------------------------------------------------------------- |
| Insert then restart app | Data still present.                                                  |
| Simulated DB failure    | Graceful error; no partial inconsistent state if using transactions. |


### 9.7 Console Input Validation


| Scenario                   | Expected                         |
| -------------------------- | -------------------------------- |
| Empty input, garbage input | Retries or errors without crash. |


---

## 10. Evaluation Criteria Mapping


| Criterion              | How this design supports it                                        |
| ---------------------- | ------------------------------------------------------------------ |
| Code readability       | Commands + small services + clear domain types.                    |
| Design quality         | Layering, patterns above, UML consistency.                         |
| Separation of concerns | UI ≠ application rules ≠ SQL.                                      |
| Database integration   | Repositories, ER-driven schema, transactions where needed.         |
| Maintainability        | New menu option = new command + wiring; new filter = new strategy. |
| Error handling         | Validation layers + repository/service error propagation.          |


---

## 11. Implementation Checklist

- Domain entities and enums aligned with ER diagram.
- Repositories implement application interfaces; `SqliteDbContext` isolated.
- All menu actions implemented as `ICommand` implementations.
- `ExpenseService` budget check before save (per sequence diagram).
- `BudgetService.Update` path matches activity diagram validation rules.
- Automated tests for Sections 9.1–9.7.
- Setup and run documented; configuration externalized.

---

## 12. References

- Assignment brief: Clean Code Learning Assignment — Personal Finance Manager.
- Local UML sources: `*.puml` files in this directory (editable in PlantUML-compatible tools).

