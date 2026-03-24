# 📋 TaskManagementProject > A production-ready **Task Management REST API** built with **ASP.NET Core 8**, following **Clean Architecture** principles. Supports full user authentication, project management, task tracking with priorities and statuses, member management, and analytics. --- ## 📑 Table of Contents - [Architecture Overview](#-architecture-overview) - [Project Structure](#-project-structure) - [Design Patterns](#-design-patterns) - [API Endpoints](#-api-endpoints) - [Data Models & DTOs](#-data-models--dtos) - [Database Design](#-database-design) - [Getting Started](#-getting-started) - [Technologies Used](#-technologies-used) --- ## 🏗️ Architecture Overview The solution follows **Clean Architecture** — dependencies only flow inward (API → Service → Core ← Repository). No inner layer knows anything about the outer layers.
┌─────────────────────────────────────────────────────────────┐
│                      API Layer                              │
│              TaskMangementProject  (Controllers)            │
│   AccountController · ProjectController · TaskController    │
│         UserController · AnalyticsController                │
└───────────────────────────┬─────────────────────────────────┘
                            │ calls
┌───────────────────────────▼─────────────────────────────────┐
│                    Service Layer                            │
│                 TaskProject.Service                         │
│   UserAccountService · TaskService · ProjectService         │
│      AnalyticsService · TokenService · EmailService         │
└───────────────────────────┬─────────────────────────────────┘
                            │ depends on interfaces from
┌───────────────────────────▼─────────────────────────────────┐
│                  Domain Layer (Core)                        │
│                  TaskProject.Core                           │
│   Entities · DTOs · Interfaces · Specifications             │
│            Enums · AutoMapper Profiles                      │
└───────────────────────────┬─────────────────────────────────┘
                            │ implemented by
┌───────────────────────────▼─────────────────────────────────┐
│                    Data Layer                               │
│               TaskProject.Repository                        │
│   TaskDbContext · IdentityDbContext · GenericRepository     │
│          UnitOfWork · Migrations · Seeding                  │
└───────────────────────────┬─────────────────────────────────┘
                            │
                  ┌─────────▼──────────┐
                  │   SQL Server DB    │
                  └────────────────────┘
**Why Clean Architecture?** - Each layer is independently testable and replaceable - The domain layer has **zero external dependencies** — pure C# with no framework coupling - Business logic lives in the Service layer, completely separate from HTTP concerns - Swapping the database or API framework touches only the outer layers --- ## 📁 Project Structure
TaskManagementProject/
│
├── TaskMangementProject/                  ← API Layer
│   ├── Controllers/
│   │   ├── AccountController.cs
│   │   ├── AnalyticsController.cs
│   │   ├── ProjectController.cs
│   │   ├── TaskController.cs
│   │   └── UserController.cs
│   ├── Helper/
│   │   └── ApplySeeding.cs
│   ├── appsettings.json
│   └── Program.cs
│
├── TaskProject.Core/                      ← Domain Layer
│   ├── Context/
│   │   └── TaskDbContext.cs
│   ├── Dtos/
│   │   ├── Analytics/   → TaskStatusAnalyticsDto.cs
│   │   ├── Auth/        → LoginDto, RegisterDto, ForgetPasswordDto, ResetPasswordDto, UserDto
│   │   ├── Project/     → ProjectDto, ProjectCreateDto
│   │   ├── Task/        → TaskDto, TaskCreateDto
│   │   └── User/        → UserDto, UserCreateDto
│   ├── Entities/
│   │   ├── Identity/    → AppUser.cs
│   │   ├── BaseEntity.cs
│   │   ├── Comment.cs
│   │   ├── Project.cs
│   │   ├── TaskItem.cs
│   │   └── User.cs
│   ├── Enums/
│   │   ├── Role.cs
│   │   ├── TaskPriority.cs
│   │   └── TaskStatus.cs
│   ├── Mapping/
│   │   ├── ProjectProfile.cs
│   │   ├── TaskProfile.cs
│   │   └── UserProfile.cs
│   ├── Migrations/
│   │   ├── 20260303025229_AddTables.cs
│   │   └── TaskDbContextModelSnapshot.cs
│   ├── PaginationResponses/
│   │   └── PaginationResponse.cs
│   ├── Repository.Contract/
│   │   ├── IGenericRepository.cs
│   │   └── IUnitOfWork.cs
│   ├── Services.Contract/
│   │   ├── IAnalyticsService.cs
│   │   ├── IEmailService.cs
│   │   ├── IProjectService.cs
│   │   ├── ITaskService.cs
│   │   ├── ITokenService.cs
│   │   ├── IUserAccountService.cs
│   │   └── IUserService.cs
│   └── Specification/
│       ├── ISpecification.cs
│       ├── ProjectSpecParams.cs
│       └── TaskSpecParams.cs
│
├── TaskProject.Repository/                ← Data Layer
│   ├── Identity/
│   │   ├── Configuration/
│   │   ├── Context/
│   │   │   └── TaskIdentityDbContext.cs
│   │   ├── DataSeed/
│   │   │   └── TaskIdentityContextSeed.cs
│   │   └── Migrations/
│   │       ├── 20260312132657_IdentityInitial.cs
│   │       └── TaskIdentityDbContextModelSnapshot.cs
│   ├── Repositories/
│   │   └── GenericRepository.cs
│   ├── SeedData/
│   │   ├── Comments.json
│   │   ├── Projects.json
│   │   ├── Tasks.json
│   │   └── Users.json
│   ├── Seeding/
│   │   └── TaskContextSeed.cs
│   ├── Specification/
│   │   ├── BaseSpecification.cs
│   │   ├── ProjectSpecification.cs
│   │   ├── SpecificationEvaluator.cs
│   │   ├── TaskCountSpecification.cs
│   │   └── TaskSpecification.cs
│   └── UnitOfWork/
│       └── UnitOfWork.cs
│
└── TaskProject.Service/                   ← Service Layer
    ├── Email/
    │   └── EmailService.cs
    └── Services/
        ├── Users/
        │   └── UserAccountService.cs
        ├── AnalyticsService.cs
        ├── ProjectService.cs
        ├── TaskService.cs
        ├── TokenService.cs
        └── UserService.cs
--- ## 🧩 Design Patterns ### Repository Pattern + Generic Repository A single GenericRepository<T> handles all CRUD operations for any entity type — no repeated data access code across services.
csharp
// One generic repository serves all entity types
IGenericRepository<Project>
IGenericRepository<TaskItem>
IGenericRepository<User>
### Unit of Work Groups multiple repository operations into a single transaction. Either everything commits or nothing does.
csharp
await _unitOfWork.CompleteAsync(); // Commits all pending changes atomically
### Specification Pattern All query logic (filtering, sorting, pagination) is encapsulated in dedicated Specification classes — completely separate from the repository.
csharp
// TaskSpecification builds the full EF Core query:
// - Filter by ProjectId
// - Sort by Name / Priority / Status
// - Pagination via Skip/Take
var spec = new TaskSpecification(specParams);
var tasks = await _unitOfWork.Repository<TaskItem>().GetAllWithSpecAsync(spec);
### AutoMapper Handles all entity-to-DTO conversions via dedicated Profile classes — no manual mapping code in controllers or services.
TaskItem  ──► TaskDto
Project   ──► ProjectDto
User      ──► UserDto
--- ## 🚀 API Endpoints ### 🔐 Account — /api/Account | Method | Endpoint | Description | Auth | |--------|----------|-------------|:----:| | POST | /api/Account/register | Register a new user | ❌ | | POST | /api/Account/login | Login and receive JWT token | ❌ | | POST | /api/Account/forgot-password | Send password reset email | ❌ | | POST | /api/Account/reset-password | Reset password using email token | ❌ | <details> <summary><b>POST</b> <code>/api/Account/register</code></summary> **Request Body:**
json
{
  "email": "user@example.com",
  "displayName": "Ahmed Ali",
  "phoneNumber": "01012345678",
  "password": "P@ssw0rd123"
}
**Response:** 200 OK </details> <details> <summary><b>POST</b> <code>/api/Account/login</code></summary> **Request Body:**
json
{
  "email": "user@example.com",
  "password": "P@ssw0rd123"
}
**Response:**
json
{
  "email": "user@example.com",
  "displayName": "Ahmed Ali",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
</details> <details> <summary><b>POST</b> <code>/api/Account/forgot-password</code></summary> **Request Body:**
json
{ "email": "user@example.com" }
Sends a password reset token to the provided email address. **Response:** 200 OK </details> <details> <summary><b>POST</b> <code>/api/Account/reset-password</code></summary> **Request Body:**
json
{
  "email": "user@example.com",
  "token": "reset-token-received-via-email",
  "newPassword": "NewP@ssw0rd123"
}
**Response:** 200 OK </details> --- ### 📊 Analytics — /api/Analytics | Method | Endpoint | Description | Auth | |--------|----------|-------------|:----:| | GET | /api/Analytics/task-status | Task count grouped by status | ✅ | <details> <summary><b>GET</b> <code>/api/Analytics/task-status</code></summary> **Response:**
json
{
  "todo": 12,
  "inProgress": 5,
  "done": 34
}
</details> --- ### 📁 Project — /api/Project | Method | Endpoint | Description | Auth | |--------|----------|-------------|:----:| | GET | /api/Project | Get all projects (paginated) | ✅ | | POST | /api/Project | Create a new project | ✅ | | GET | /api/Project/{id} | Get project details by ID | ✅ | | POST | /api/Project/{projectId}/members/{userId} | Add a member to a project | ✅ | <details> <summary><b>GET</b> <code>/api/Project</code></summary> **Query Parameters:** | Parameter | Type | Description | |-----------|------|-------------| | PageIndex | int | Page number (default: 1) | | PageSize | int | Items per page (default: 10) | | Sort | string | Sort field (e.g. name) | </details> <details> <summary><b>POST</b> <code>/api/Project</code></summary> **Request Body:**
json
{
  "name": "Website Redesign",
  "description": "Full redesign of the company website",
  "ownerId": 1
}
</details> <details> <summary><b>GET</b> <code>/api/Project/{id}</code></summary> **Response:**
json
{
  "id": 1,
  "name": "Website Redesign",
  "description": "Full redesign of the company website",
  "ownerId": 1,
  "ownerUsername": "ahmed.ali",
  "membersCount": 4,
  "tasksCount": 12
}
</details> <details> <summary><b>POST</b> <code>/api/Project/{projectId}/members/{userId}</code></summary> Adds the user with userId to the project with projectId.
POST /api/Project/1/members/5
**Response:** 200 OK </details> --- ### ✅ Task — /api/Task | Method | Endpoint | Description | Auth | |--------|----------|-------------|:----:| | GET | /api/Task | Get all tasks (paginated, filterable) | ✅ | | POST | /api/Task | Create a new task | ✅ | | GET | /api/Task/{id} | Get task details by ID | ✅ | | PUT | /api/Task/{id}/status | Update task status | ✅ | <details> <summary><b>GET</b> <code>/api/Task</code></summary> **Query Parameters:** | Parameter | Type | Description | |-----------|------|-------------| | PageIndex | int | Page number | | PageSize | int | Items per page | | ProjectId | int | Filter tasks by project | | Sort | string | Sort field | </details> <details> <summary><b>POST</b> <code>/api/Task</code></summary> **Request Body:**
json
{
  "title": "Design landing page",
  "description": "Create wireframes and mockups for the homepage",
  "projectId": 1,
  "assignedToId": 3,
  "status": 0,
  "priority": 1
}
**Response:**
json
{
  "id": 7,
  "title": "Design landing page",
  "description": "Create wireframes and mockups for the homepage",
  "status": "Todo",
  "priority": "Medium",
  "projectId": 1,
  "assignedToId": 3,
  "projectName": "Website Redesign",
  "assignedToUsername": "sara.khaled"
}
</details> <details> <summary><b>PUT</b> <code>/api/Task/{id}/status</code></summary> **Query Parameter:** status (integer)
PUT /api/Task/7/status?status=1
**Status Values:** | Value | Name | |-------|------| | 0 | Todo | | 1 | InProgress | | 2 | Done | | 3 | Cancelled | **Response:** 200 OK </details> --- ### 👤 User — /api/User | Method | Endpoint | Description | Auth | |--------|----------|-------------|:----:| | GET | /api/User | Get all users | ✅ | | POST | /api/User | Create a new user (admin) | ✅ | <details> <summary><b>GET</b> <code>/api/User</code></summary> **Response:**
json
[
  {
    "id": 1,
    "username": "ahmed.ali",
    "email": "ahmed@example.com",
    "role": "Admin"
  },
  {
    "id": 2,
    "username": "sara.khaled",
    "email": "sara@example.com",
    "role": "Member"
  }
]
</details> <details> <summary><b>POST</b> <code>/api/User</code></summary> **Request Body:**
json
{
  "username": "omar.hassan",
  "email": "omar@example.com",
  "password": "P@ssw0rd123",
  "role": 1
}
**Role Values:** 0 = Admin, 1 = Member **Response:**
json
{
  "id": 3,
  "username": "omar.hassan",
  "email": "omar@example.com",
  "role": "Member"
}
</details> --- ## 📦 Data Models & DTOs ### Core Entities
BaseEntity
├── Project    → Id, Name, Description, OwnerId, Members[], Tasks[]
├── TaskItem   → Id, Title, Description, Status, Priority, ProjectId, AssignedToId
├── User       → Id, Username, Email, Role
└── Comment    → Id, Content, TaskId, AuthorId, CreatedAt

AppUser (ASP.NET Identity) → Extends IdentityUser with DisplayName, PhoneNumber
### Enums
csharp
enum TaskStatus   { Todo = 0, InProgress = 1, Done = 2, Cancelled = 3 }
enum TaskPriority { Low = 0, Medium = 1, High = 2, Critical = 3 }
enum Role         { Admin = 0, Member = 1 }
--- ## 🗄️ Database Design The application uses **two separate SQL Server databases**: ### TaskDbContext — Main Application Database Stores all application data: Projects, Tasks, Users, Comments.
Projects  ──<  Tasks        (one project has many tasks)
Users     ──<  Tasks        (one user is assigned to many tasks)
Projects  ──<  Members      (many-to-many: projects ↔ users)
Tasks     ──<  Comments     (one task has many comments)
### TaskIdentityDbContext — Identity Database Handles ASP.NET Core Identity separately: user accounts, password hashing, roles, and password reset tokens. ### Seed Data On first run, the app auto-loads initial data from JSON files in SeedData/: | File | Contents | |------|----------| | Users.json | Default users with assigned roles | | Projects.json | Sample projects | | Tasks.json | Sample tasks linked to projects and users | | Comments.json | Sample comments on tasks | --- ## ⚙️ Getting Started ### Prerequisites - [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) - SQL Server (LocalDB / Express / Full) - SMTP credentials (for the password reset email feature) ### 1. Clone the Repository
bash
git clone https://github.com/your-username/TaskManagementProject.git
cd TaskManagementProject
### 2. Configure appsettings.json
json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=TaskManagementDb;Trusted_Connection=True;TrustServerCertificate=True",
    "IdentityConnection": "Server=.;Database=TaskIdentityDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "JWT": {
    "Key": "your-super-secret-key-minimum-32-characters",
    "Issuer": "TaskManagementProject",
    "Audience": "TaskManagementClient",
    "DurationInDays": 7
  },
  "EmailSettings": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "SenderEmail": "your-email@gmail.com",
    "SenderPassword": "your-app-password",
    "SenderName": "TaskManagement"
  }
}
### 3. Apply Database Migrations
bash
# Main application database
dotnet ef database update \
  --project TaskProject.Core \
  --startup-project TaskMangementProject

# Identity database
dotnet ef database update \
  --project TaskProject.Repository \
  --startup-project TaskMangementProject \
  --context TaskIdentityDbContext
### 4. Run the Application
bash
dotnet run --project TaskMangementProject
### 5. Open Swagger UI
https://localhost:{port}/swagger
> Seed data is applied automatically on first run via ApplySeeding.cs. --- ## 🛠️ Technologies Used | Technology | Purpose | |------------|---------| | **ASP.NET Core 8** | Web API framework | | **Entity Framework Core** | ORM for database access | | **ASP.NET Core Identity** | Authentication & user management | | **JWT Bearer Tokens** | Stateless authentication | | **AutoMapper** | Entity-to-DTO mapping | | **Swagger / Swashbuckle** | API documentation & interactive testing | | **SQL Server** | Relational database | | **Clean Architecture** | Solution structure & separation of concerns | | **Repository Pattern** | Data access abstraction | | **Unit of Work** | Atomic transaction management | | **Specification Pattern** | Reusable, composable query building |
