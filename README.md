## 📚 Course Overview: ASP.NET Core MVC (Lab 01 - Lab 06)

This repository documents the progression through 6 comprehensive labs, building a fundamental foundation with an ASP.NET Core MVC application. Moreover, we can understand an overview of how the backend side works, showing the UI interface through views.  

### Lab 01 & Lab 02: Foundations & Project Architecture
* **Core Focus:** Understanding the ASP.NET Core framework and MVC architecture.
* **Key Activities:**
  * Setting up the project structure (`Program.cs`, middleware pipeline).
  * Creating model, service, controller.
  * Understanding Routing and passing data from Controller to View using `ViewBag` and `ViewData`.
  * Setting up version control (Git/GitHub) for the project.
  * Configuring appsettings.json and appsettings.Development.json.
  * Getting familiar with Razor syntax (@model, @Model, @foreach, @if, @{ }).
    
### Lab 03: UI Construction & Data Binding
* **Core Focus:** Building dynamic interfaces and handling user input.
* **Key Activities:**
  * Designing reusable UI components using Razor Layouts and Partial Views.
  * Implementing Tag Helpers for form generation.
  * Using ViewModels to securely transfer data between the View and Controller.

### Lab 04 & Lab 05: DI, Service/Repository, Options Pattern & EF Core Data Layer & CRUD & State Management
* **Core Focus:** Implementing robust data manipulation and handling concurrency.
* **Key Activities:**
  * Connecting PostgreSQL database.
  * Adding search functionality using LINQ and parameterized queries.
  * Building the full Create, Read, Update, and Delete (CRUD) flows.
  * Implementing a "Soft Delete" mechanism (Trash/Restore functionality).
  * Handling database concurrency using `RowVersion` (preventing "Last Save Wins" conflicts).

### Lab 06 (Final Project): Authentication & Authorization
* **Core Focus:**  Securing the application with user identities and access control - upgrading the app for production with advanced security, tracking, and Git workflows.
* **Key Activities:**
  * Integrating ASP.NET Core Identity (`ApplicationUser`).
  * Building Register, Login, and Logout flows using Cookie Authentication.
  * Seeding role-based accounts (Admin, Customer).
  * Applying `[Authorize]` and creating custom Policies (e.g., `CanManageStationery`,...) to restrict access to specific routes.
  * Integrating third-party OAuth (Google Authentication).
  * Securing forms against CSRF attacks with `[ValidateAntiForgeryToken]`.
  * Creating an Audit Log system to track sensitive user actions.
  * Configuring Observability via Health Checks (`/health/live`, `/health/ready`).
  * Standardizing error handling using `ProblemDetails` and safe production logging.
  * Finalizing Git branch management using feature branches and pull requests.
