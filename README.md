# Todo Application (Full Stack)

Full-stack Todo application built with **Angular** (frontend) and **ASP.NET Core (.NET 8)** (backend) with **PostgreSQL**.


## Architecture (Azure)

- **Frontend:** Angular → **Azure Static Web Apps**
- **Backend:** ASP.NET Core Web API → **Azure App Service (Linux)**
- **Database:** **Azure Database for PostgreSQL (Flexible Server)**
- **Infrastructure as Code:** **Bicep** (in `/infra`)
- **CI/CD:** **GitHub Actions**
  - Deploy infra (Bicep)
  - Deploy backend (zip deploy to App Service)
  - Deploy frontend (Static Web Apps workflow)

### High-level diagram

Frontend (SWA)  →  Backend API (App Service)  →  PostgreSQL (Flexible Server)


## Run Locally

### Backend (.NET API)
1. Open `ToDosBackend/` in your IDE
2. Update the connection string in `appsettings.json` (or use environment variables)
3. Run the API

### Frontend (Angular)
1. `cd ToDosSystemAngular`
2. `npm install`
3. `ng serve`
4. Open: `http://localhost:4200`

> Note: In production, services point to the Azure API URL.

## Deployment (Azure + IaC + CI/CD)

This project is deployed on Azure using **Infrastructure as Code (Bicep)** and **GitHub Actions CI/CD**.

### Infrastructure (Bicep)
- Folder: `/infra`
- Provisions:
  - App Service Plan + Web App (`todo-saad-api`)
  - Azure PostgreSQL Flexible Server + DB
  - Firewall rule allowing Azure services
  - (Frontend is deployed via Static Web Apps GitHub integration)

### CI/CD (GitHub Actions)
- Backend deployment pipeline builds & publishes .NET, then deploys to Azure App Service.
- Frontend pipeline builds Angular and deploys to Azure Static Web Apps.

### Key environment variables (Azure App Service)
- `ConnectionStrings__AppProgDb` → PostgreSQL connection string
- `AllowedOrigins__0` → Static Web App URL (CORS allow-list)

## Troubleshooting

### CORS errors
Ensure the backend allows the frontend origin:
- Set `AllowedOrigins__0` in Azure App Service to your Static Web App URL

### Database errors
If you see errors like `relation "todos" does not exist`, ensure the required tables exist in PostgreSQL.
