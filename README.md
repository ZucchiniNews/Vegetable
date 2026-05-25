# 🥒 ZucchiniNews — Vegetable

A full-stack news platform built with ASP.NET Core MVC, Strapi CMS, ASP.NET Identity, Entity Framework Core, and Azure cloud services.

Live site: [zucchininews.azurewebsites.net](https://zucchininews.azurewebsites.net)

---

## Table of Contents

- [Overview](#overview)
- [Solution Structure](#solution-structure)
- [Tech Stack](#tech-stack)
- [Prerequisites](#prerequisites)
- [Local Setup](#local-setup)
- [Configuration Reference](#configuration-reference)
- [Database Setup](#database-setup)
- [Strapi CMS Setup](#strapi-cms-setup)
- [Azure Services Setup](#azure-services-setup)
- [Running the Application](#running-the-application)
- [Running Azure Functions Locally](#running-azure-functions-locally)
- [CI/CD](#cicd)
- [Project Notes](#project-notes)

---

## Overview

ZucchiniNews is a news aggregation and publishing platform. The MVC frontend fetches articles from a Strapi headless CMS, handles user accounts via ASP.NET Identity, tracks analytics via Azure Application Insights, and uses Azure Blob/Table Storage for media and historical data. Background jobs (e.g. subscription expiry notifications) run as Azure Functions.

---

## Solution Structure

```
Vegetable/
├── Zucchinimvc.sln
├── ZucchiniMVC/              # ASP.NET Core MVC web application
├── ZucchiniCore/             # Shared domain models, interfaces, DTOs
├── SharedLib/                # Shared utilities and helpers
├── ZucchiniMVC.E2ETests/     # End-to-end tests (Playwright)
├── zucchini-functions/       # Azure Functions (background jobs)
└── .github/workflows/        # GitHub Actions CI/CD pipelines
```

### Project Responsibilities

**ZucchiniMVC** — The main web app. Contains controllers, Razor views, Identity configuration, EF Core context, service registrations, and all application logic.

**ZucchiniCore** — Shared library consumed by both `ZucchiniMVC` and `zucchini-functions`. Houses domain entities, enums, DTOs, and repository/service interfaces.

**SharedLib** — Common utilities shared across the solution.

**zucchini-functions** — Azure Functions project. Currently implements a `SubscriptionExpiryNotifier` timer trigger that sends expiry reminder emails to subscribers.

**ZucchiniMVC.E2ETests** — End-to-end test suite.

---

## Tech Stack

| Concern | Technology |
|---|---|
| Web framework | ASP.NET Core MVC (.NET 8) |
| CMS / content source | Strapi (headless, REST API) |
| Authentication | ASP.NET Core Identity |
| ORM | Entity Framework Core |
| Database | Microsoft SQL Server (MSSQL) |
| File storage | Azure Blob Storage |
| Historical data | Azure Table Storage |
| Background jobs | Azure Functions (isolated worker) |
| Analytics | Azure Application Insights |
| Frontend charts | Chart.js |
| CI/CD | GitHub Actions |
| Hosting | Azure App Service |
| Security scanning | CodeQL (GitHub Advanced Security) |

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Azure Functions Core Tools v4](https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express / LocalDB for local dev)
- [Node.js 18+](https://nodejs.org/) — required for Strapi
- An [Azure account](https://azure.microsoft.com/free/) — for Blob Storage, Table Storage, and App Insights (or use Azurite locally)
- [Azurite](https://github.com/Azure/Azurite) (optional) — local Azure Storage emulator

---

## Local Setup

### 1. Clone the repository

```bash
git clone https://github.com/ZucchiniNews/Vegetable.git
cd Vegetable
git checkout development
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Configure user secrets

The project uses `appsettings.json` for non-sensitive defaults and either user secrets (local) or Azure App Service environment variables (production) for sensitive values.

For local development, add secrets using the .NET Secret Manager:

```bash
cd ZucchiniMVC
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=ZucchiniNews;Trusted_Connection=True;MultipleActiveResultSets=true"
dotnet user-secrets set "Strapi:BaseUrl" "http://localhost:1337"
dotnet user-secrets set "Strapi:ApiToken" "<your-strapi-api-token>"
dotnet user-secrets set "AzureStorage:ConnectionString" "UseDevelopmentStorage=true"
dotnet user-secrets set "ApplicationInsights:ConnectionString" "<your-appinsights-connection-string>"
dotnet user-secrets set "Email:SmtpHost" "<your-smtp-host>"
dotnet user-secrets set "Email:SmtpPort" "587"
dotnet user-secrets set "Email:Username" "<your-email>"
dotnet user-secrets set "Email:Password" "<your-email-password>"
```

> For fully local development without Azure, set `AzureStorage:ConnectionString` to `UseDevelopmentStorage=true` and run Azurite.

---

## Configuration Reference

Below is a summary of all configuration keys the application reads. In production these are set as App Service environment variables (or Key Vault references).

| Key | Description |
|---|---|
| `ConnectionStrings:DefaultConnection` | MSSQL connection string for the Identity and application database |
| `Strapi:BaseUrl` | Base URL of the Strapi CMS instance (e.g. `http://localhost:1337`) |
| `Strapi:ApiToken` | Strapi API token for authenticated requests |
| `AzureStorage:ConnectionString` | Azure Storage connection string (Blob + Table Storage). Use `UseDevelopmentStorage=true` for Azurite |
| `ApplicationInsights:ConnectionString` | Azure Application Insights connection string |
| `Email:SmtpHost` | SMTP host for outgoing email |
| `Email:SmtpPort` | SMTP port (typically `587` for TLS) |
| `Email:Username` | SMTP username / sender address |
| `Email:Password` | SMTP password |

---

## Database Setup

The application uses EF Core migrations with MSSQL. To create/update the database:

```bash
cd ZucchiniMVC
dotnet ef database update
```

To add a new migration:

```bash
dotnet ef migrations add <MigrationName>
```

> Make sure your `DefaultConnection` connection string is set before running migrations.

The database hosts ASP.NET Identity tables (users, roles, claims) as well as any application entities (subscriptions, analytics, etc.).

---

## Strapi CMS Setup

ZucchiniNews uses Strapi as its content source. Articles, categories, and media are managed in Strapi and consumed via REST API.

### Running Strapi locally

If you have a local Strapi instance:

```bash
cd <your-strapi-project>
npm install
npm run develop
```

Strapi will be available at `http://localhost:1337`. Set `Strapi:BaseUrl` accordingly.

### Strapi API token

Generate a read-only API token in Strapi under **Settings → API Tokens** and set it as `Strapi:ApiToken`.

### Expected content types

The application expects the following Strapi content types:

- `articles` — with fields: `Title`, `Content`, `Slug`, `PublishedAt`, `Cover` (with `url`/`formats`), `Category`
- `categories` — with fields: `Name`, `Slug`

---

## Azure Services Setup

### Azure Blob Storage

Used to store uploaded media/images. The application uses a container named `zucchini-media` (created automatically on first use if permissions allow, otherwise create it manually in the Azure Portal with **Blob** public access).

### Azure Table Storage

Used for time-series analytics history (e.g. daily page view counts). Tables are created automatically by the application on startup if they don't exist.

### Azure Application Insights

Used to track page views, requests, and custom analytics events. Create an Application Insights resource in the Azure Portal and copy the **Connection String** into your configuration.

### Azurite (local emulator)

To avoid needing a real Azure Storage account locally:

```bash
# Install
npm install -g azurite

# Run (data stored in current directory)
azurite --silent --location ./azurite-data --debug ./azurite-debug.log
```

Then set `AzureStorage:ConnectionString` to `UseDevelopmentStorage=true`.

---

## Running the Application

```bash
cd ZucchiniMVC
dotnet run
```

The app will be available at `https://localhost:5001` (or the port shown in the console).

### First-run checklist

1. Database has been created (`dotnet ef database update`)
2. Strapi is running and `Strapi:BaseUrl` / `Strapi:ApiToken` are set
3. Azure Storage (or Azurite) connection string is set
4. An admin user can be registered via the `/Account/Register` page, or seeded manually

---

## Running Azure Functions Locally

The `zucchini-functions` project contains background jobs. To run locally:

```bash
cd zucchini-functions
```

Create a `local.settings.json` file (this file is git-ignored):

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "AzureStorage:ConnectionString": "UseDevelopmentStorage=true",
    "ConnectionStrings:DefaultConnection": "<your-mssql-connection-string>",
    "Email:SmtpHost": "<smtp-host>",
    "Email:SmtpPort": "587",
    "Email:Username": "<email>",
    "Email:Password": "<password>"
  }
}
```

Then run:

```bash
func start
```

### Functions included

| Function | Trigger | Description |
|---|---|---|
| `SubscriptionExpiryNotifier` | Timer (daily) | Queries for subscriptions expiring within 7 days and sends reminder emails |

---

## CI/CD

The project uses GitHub Actions for CI/CD. Workflows are defined under `.github/workflows/`.

### Pipelines

| Workflow | Trigger | Description |
|---|---|---|
| Build & Deploy | Push to `main` | Builds the solution, runs CodeQL security scan, publishes and deploys `ZucchiniMVC` to Azure App Service |
| PR Check | Pull request to `main` / `development` | Builds the solution and runs CodeQL |

### Secrets required in GitHub

Configure these under **Repository → Settings → Secrets and variables → Actions**:

| Secret | Description |
|---|---|
| `AZURE_WEBAPP_PUBLISH_PROFILE` | Publish profile downloaded from the Azure App Service resource |
| `AZURE_WEBAPP_NAME` | Name of the Azure App Service (e.g. `zucchininews`) |

### Azure App Service startup command

The App Service must be configured with an explicit startup command to avoid ambiguity when multiple `.runtimeconfig.json` files are present in the publish output:

```
dotnet Zucchinimvc.dll
```

Set this under **App Service → Configuration → General settings → Startup Command**.

---

## Project Notes

- The solution targets **.NET 8**.
- `appsettings.json` is git-ignored for security. Use user secrets locally and App Service environment variables in production.
- The `development` branch is the main working branch. `main` triggers deployments.
- CodeQL is used for static security analysis — it scans for vulnerabilities, not build errors. A separate `dotnet build` step in CI covers build validation.
- The analytics dashboard (`/Admin/Analytics`) uses Chart.js to render time-series graphs of page views pulled from Azure Table Storage and Application Insights.
- User account management (email change, password change) is implemented as a single consolidated Razor Page with Bootstrap tabs and named `OnPost` handlers.
