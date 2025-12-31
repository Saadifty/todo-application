# ToDo Application

A full-stack ToDo management system built with **Angular**, **.NET 8 Web API**, and **PostgreSQL**.  
This project demonstrates clean architecture, CRUD operations, RESTful API integration, and a responsive UI.

---

##  Features
- User-friendly task management interface
- CRUD operations for ToDos
- Backend REST API using ASP.NET Core
- PostgreSQL database integration via Entity Framework Core
- Separation of concerns (frontend, backend, database)
- Environment-based configuration

---

##  Tech Stack
| Layer | Technology |
|-------|-------------|
| Frontend | Angular 17 |
| Backend | ASP.NET Core 8 |
| Database | PostgreSQL |
| ORM | Entity Framework Core |
| Tools | Visual Studio / VS Code, Postman, Git |

---

##  Project Structure
todo-application/
│
├── ToDosBackend/ # C# .NET backend
│ ├── ToDosAdminSystem.API/ # API controllers, startup
│ ├── ToDosAdminSystem.Core/ # Entities, interfaces
│ ├── ToDosAdminSystem.Infrastructure/ # EF Core, DB context
│ └── ToDosAdminSystem.sln # Visual Studio solution
│
└── ToDosSystemAngular/ # Angular frontend
├── src/
├── package.json
└── angular.json
