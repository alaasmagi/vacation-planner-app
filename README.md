# vacation-planner-app

## Short description
* UI language: Estonian
* Development year: **2026**
* Languages and technologies: **Backend: ASP.NET, EF Core, SQLite & Frontend: React, TypeScript**

## How to run
### Prerequisites

* .NET 10
* EF Core
* Node.js 
* Modern web browser

Backend should have .env file in the backend root folder `/vacation-planner-backend` which has following content:
```bash
DB_CONNECTION=<your-db-path-for-development-mode>#Default value = "Data Source=app.db"
DB_CONNECTION_DEVELOPMENT=<your-db-path-for-development-mode>#Default value = "Data Source=../app.db"
FRONTEND_URL=<your-frontend-url-for-cors>
BACKEND_URL=<your-backend-url>
DEFAULT_VACATION_LENGTH=<legal-max-vacation-days>
```
SQLite DB is located in the backend root folder `/vacation-planner-backend`  
The example of `.env` has been provided in `/vacation-planner-backend/.env.example`  

If You want to use default values, you can just use the command `cp .env.example .env` in the backend root folder `/vacation-planner-backend`  

Frontend should also have .env file in the frontend root folder `/vacation-planner-client` which has following content:
```bash
VITE_FRONTEND_URL=<your-frontend-url>
VITE_API_URL=>your-backend-url>
VITE_EMPLOYEE_ID=<hardcoded-employee-id-in-guid-format>#Default value: d2f1615f-4a3b-4e49-a8e2-75ecbf202d83
VITE_DEFAULT_VACATION_LENGTH=<legal-max-vacation-days>
```
The example has been provided in `/vacation-planner-client/.env.example`

If You want to use default values, you can just use the command `cp .env.example .env` in the backend root folder `/vacation-planner-backend`  

5080 is the default port on which the backend runs. 8080 is the default port on which the frontend runs.

### Running the app

After meeting all prerequisites above - 
* backend can be run via terminal/cmd open in the `/vacation-planner-backend/Web` folder by executing command:  
```bash
dotnet run
```
* frontend can be run via terminal/cmd open in the `/vacation-planner-client` folder by executing command:  
```bash
npm i; npm run start 
```

## Structure

### Data model(ERD)

<img width="305" height="308" alt="image" src="https://github.com/user-attachments/assets/950e5934-2499-4185-bbc4-29c94287bc47" />

**No relationships**- only a single entity.

### Backend structure

```
vacation-planner-backend
  ├── app.db
  ├── Application
  │   ├── Application.csproj
  │   └── VacationRequestService.cs
  ├── Contract
  │   ├── Application
  │   ├── Contract.csproj
  │   └── DataAccess
  ├── DataAccess
  │   ├── AppDbContext.cs
  │   ├── AppDbContextFactory.cs
  │   ├── DataAccess.csproj
  │   ├── DataAccessUow.cs
  │   ├── Migrations
  │   └── VacationRequestRepository.cs
  ├── Directory.Build.props
  ├── Domain
  │   ├── Domain.csproj
  │   ├── EVacationStatus.cs
  │   └── VacationRequest.cs
  ├── DTO
  │   ├── DataAccess
  │   ├── DTO.csproj
  │   ├── Error
  │   └── Presentation
  ├── Helpers
  │   ├── EnvInitializer.cs
  │   └── Helpers.csproj
  ├── Test
  │   ├── DomainTest.cs
  │   ├── MapperTest.cs
  │   ├── RepositoryTest.cs
  │   ├── ServiceTest.cs
  │   ├── Test.csproj
  │   └── TestHelpers.cs
  ├── vacation-planner-backend.sln
  └── Web
      ├── ApiControllers
      ├── appsettings.Development.json
      ├── appsettings.json
      ├── Controllers
      ├── Models
      ├── Program.cs
      ├── Properties
      ├── Views
      ├── Web.csproj
      └── wwwroot
```

#### alaasmagi.Base.* NuGet packages
The core and base logic of the application is modularized into separate NuGet packages **alaasmagi.Base.*** ([GitHub link](https://github.com/alaasmagi/alaasmagi-base-nuget/tree/main/Base.Domain)), containing reusable domain models, shared abstractions and foundational realisation components. This package is developed and maintained by myself and serves as a shared base across projects.

The project currently contains these NuGet packages:

- `alaasmagi.Base.Domain` - [NuGet link](https://www.nuget.org/packages/alaasmagi.Base.Domain)
- `alaasmagi.Base.Contracts.DTO` - [NuGet link](https://www.nuget.org/packages/alaasmagi.Base.Contracts.DTO)
- `alaasmagi.Base.DTO` - [NuGet link](https://www.nuget.org/packages/alaasmagi.Base.DTO)
- `alaasmagi.Base.Contracts.DataAccess` - [NuGet link](https://www.nuget.org/packages/alaasmagi.Base.Contracts.DataAccess)
- `alaasmagi.Base.DataAccess.EF` - [NuGet link](https://www.nuget.org/packages/alaasmagi.Base.DataAccess.EF)
- `alaasmagi.Base.Contracts.Application` - [NuGet link](https://www.nuget.org/packages/alaasmagi.Base.Contracts.Application)
- `alaasmagi.Base.Application` - [NuGet link](https://www.nuget.org/packages/alaasmagi.Base.Application)


#### Domain layer

* **VacationRequest:**

```csharp
public class VacationRequest(int defaultVacationLength) : BaseEntity
{ 
    public Guid EmployeeId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string? Comment { get; set; }
    public EVacationStatus Status { get; set; }
    
    public bool IsValid => EndDate > StartDate 
                           && StartDate >= DateOnly.FromDateTime(DateTime.Today);

    public int DurationDays => EndDate.DayNumber - StartDate.DayNumber;
    public bool IsOverTime => DurationDays > defaultVacationLength;
}
```

The **BaseEntity** class is not defined in this project itself, it comes from the NuGet package **alaasmagi.Base.Domain** package. For more details, click [here](#alaasmagibase-nuget-packages).

* **BaseEntity:**
```csharp
public abstract class BaseEntity : BaseEntity<Guid>
{
    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }
}

public abstract class BaseEntity<TKey> : IBaseEntity<TKey>
    where TKey : IEquatable<TKey>
{
    [Required]
    public virtual TKey Id { get; set; } = default!;
}
```

* **EVacationStatus:**

```csharp
public enum EVacationStatus
{
    Pending,
    Approved,
    Rejected,
}
```

#### Application layer
* **VacationRequestService:** Responsible for applying business rules and communicating with database via **IVacationRequestRepository** interface  
**VacationRequestService** class inherits from **BaseService** class, which is part of the NuGet package **alaasmagi.Base.Application**. For more details, click [here](#alaasmagibase-nuget-packages).

#### Contract layer
* **IVacationRequestService**
* **IVacationRequestRepository**
IVacationRequestService implements IBaseService interface and IVacationRequesRepository inherits from IBaseRepository. Both base interfaces are a part of NuGet packages **alaasmagi.Base.Contracts.Application** and **alaasmagi.Base.Contracts.DataAccess**. For more details, click [here](#alaasmagibase-nuget-packages).

#### DTO layer
* **VacationRequestEntity:** Database entity which keeps both Domain data and meta data
VacationRequestEntity inherits from **BaseEntityWithMeta** which is part of the NuGet package **alaasmagi.Base.Domain**. For more details, click [here](#alaasmagibase-nuget-packages).
* **VacationRequestDto** & **VactionRequestWebDto:** DTOs which hide the unnecessary datafields from UI and API
* **VacationRequestError:** Static class which holds standardised error code and message for duplicate vacation request entry
* Mappers for each of the DTOs: **VacationRequestMapper**, **VacationRequestDtoMapper** & **VacationRequestWebDtoMapper**  
All of the three mappers implement **IMapper** which is part of the NuGet package **alaasmagi.Base.Contracts.DTO**. For more details, click [here](#alaasmagibase-nuget-packages).

#### Helpers
* **EnvInitializer:** Responsible for providing all environment variables from `.env` file.

#### Web layer
* **Controllers:** MVC controller responsible for handling UI requests and returning views. This controller serves the MVC UI.
* **ApiControllers:** REST API controllers responsible for handling HTTP requests and returning JSON responses. This controller serves the React UI.

The project has two UIs:
* **MVC UI:** Server-side UI used by administrators or managers.
* **React UI:** Client-side application used by employees.

#### Endpoints
* **GET** - `/api/VacationRequest`: Fetches all VacationRequests.
* **GET** - `/api/VacationRequest/{ID}`: Fetches one VacationRequest by ID.
* **POST** - `/api/VacationRequest`: Creates one VacationRequest.
* **PUT** - `/api/VacationRequest/{ID}`: Updates one VacationRequest.
* **DELETE** - `/api/VacationRequest/{ID}`: Deletes one VacationRequest.

* Endpoints are documented with Swagger, which is available via `/api/swagger`:
<img width="1447" height="327" alt="image" src="https://github.com/user-attachments/assets/0c85c862-e43f-4090-a8e8-54d70135d163" />


### Frontend structure
```
src
├── api
│   └── index.ts
├── assets
│   ├── hero.png
│   ├── react.svg
│   └── vite.svg
├── components
│   ├── Entry.tsx
│   ├── Loading.tsx
│   └── VacationRequestForm.tsx
├── index.css
├── main.tsx
├── models
│   └── index.ts
├── routing
│   └── router.tsx
├── types
│   └── index.ts
├── utils
│   ├── index.ts
│   └── ui.ts
└── views
    ├── Details.tsx
    ├── Edit.tsx
    └── Home.tsx
```

* **Api** - Uses Axios methods to fetch necessary data from backend.
* **Assets** - Default assets for react.
* **Router** - Handles navigation between views.
* **Types** - Consists of DTOs for API usage.
* **Models** - Consist of helper data model for mapping the statuses.
* **Utils** - Consists of helper functions that are used for displaying error messages to UI, computing date overlapping etc.

#### Components
* **Entry** - Visual component for displaying one vacation request.
* **Loading** - Visual component for displaying loading state.
* **VacationRequestForm** - Visual and functional component for getting input for VacationRequest.

#### Views
* **Home** - View for employees to display all their VacationRequest.
* **Details** - View for employees to display one specific VacationRequest.
* **Edit** - View for employess to create or edit VacationRequest.

## Design choices

### Database  
I kept persistence lightweight with SQLite and a single-entity data model. That keeps the schema small, easy to reason about and fast to evolve. IDs use GUIDs (via the shared BaseEntity), which is a standard, low-collision approach. For request state I use an enum (Pending/Approved/Rejected) to avoid stringly-typed status bugs.

### Backend
The backend follows a clean, layered structure: Domain holds the core entity and rules, Application contains the business service, DataAccess isolates EF Core and repositories and Web exposes both MVC and REST endpoints. Contracts and DTOs sit between layers to keep dependencies explicit and map only the fields needed for each boundary. I also reuse my own alaasmagi.Base.* NuGet packages for shared abstractions and base classes, so the project stays consistent with other apps.

### Frontend
The client is a focused React + TypeScript SPA used by employees. It talks to the REST API and keeps the UI simple: submit a request, view existing requests and check status. Admins/managers use the separate MVC UI served by the backend, so the employee-facing SPA stays clean and lightweight.

## Features

Main application features include:

* Vacation request creation with start/end dates and optional comment
* Automatic validation of date ranges and duration
* Status tracking with Pending, Approved, Rejected
* Full CRUD REST API for vacation requests
* React SPA for employees
* MVC UI for managers/admins
* Environment-driven configuration (.env) for DB, ports and legal vacation length

## Testing

### Unit tests
Unit tests in this project are implemented using the NUnit framework and focus on the backend’s core logic, repository layer, domain rules and mapper behavior rather than the web layer. The service layer is covered through vacation request creation, updates, deletion and retrieval flows, with specific checks that overlapping date ranges for the same employee are detected and duplicate requests are rejected. Repository tests validate CRUD operations and overlap detection against the real data access layer using an isolated in-memory SQLite database, which keeps the test suite fast and stable. Mapper tests verify correct translation between domain, database and DTO models, including default values, computed fields and collection mapping. There are also direct domain tests for date validity, vacation duration calculation and overtime vacation logic. Overall, the suite provides solid coverage of the application’s business rules and data transformations without adding the overhead of HTTP-layer testing.

## Visuals

### React UI

* **Home view:**  
<img width="1728" height="1117" alt="image" src="https://github.com/user-attachments/assets/37dd8b04-dbe1-495c-acc0-7424358cc52f" />

* **Details view:**  
<img width="1728" height="1117" alt="image" src="https://github.com/user-attachments/assets/7b31f200-b57c-44ac-bb91-f02d29a318b6" />

* **Create/Edit view:**  
<img width="1728" height="1117" alt="image" src="https://github.com/user-attachments/assets/38252c5a-0cf2-4a90-82f1-924104d73667" />


### ASP.NET MVC UI

* **Index:**  
<img width="1728" height="1117" alt="image" src="https://github.com/user-attachments/assets/cc2b3538-ca8c-4e08-9d27-9896962e6a4e" />  

* **VacationRequest Index:**
<img width="1728" height="1117" alt="image" src="https://github.com/user-attachments/assets/79b2ed7d-38b6-4c35-9041-c4fdb5802c7f" />

* **VacationRequest Details:**
<img width="1728" height="1117" alt="image" src="https://github.com/user-attachments/assets/75fe2fd1-81f6-42b8-81f2-70a57773ea49" />

* **VacationRequest Create/Edit:**  
<img width="1728" height="1117" alt="image" src="https://github.com/user-attachments/assets/580f86e8-3691-4aaf-bfa6-67a3db816e60" />


## Improvements & scaling possibilities

### Multiple users and JWT authentication
For this project to be production-ready, it needs to the capability for multiple employees with users registration and login, JWT and refreshtoken generation, password recovery and email validation. It could also benefit from external authentication provider like Google or Microsoft which eliminates the reason for newcomers to make yet another account to start using the admin page of the application.

### Multi-tenancy
I like to think about every project as a potential SaaS candidate. In this case, it would be a good idea to introduce an abstraction layer and move one or levels higher in the database design to allow other companies and multiple users to use the solution as well. The current approach focuses on a single company with a single employee and their vacation requests, but it could be extended to support multiple employees and companies, each with their own employees with their vacation requests.

## Q&A

### 1. How would you plan to automatically test the application? What types of tests would you write, which tools would you use and what would be their purpose in the application?
I have already implemented automated tests for DTO mappers, the service layer and the data access layer. However, additional tests could further improve confidence, particularly around edge cases and integration behavior. Useful next additions include HTTP endpoint tests to validate request handling and response codes, as well as service and repository tests for boundary scenarios such as adjacent non-overlapping date ranges. Additional coverage could also include update scenarios to ensure existing data is preserved correctly. It would also be valuable to introduce broader integration tests (or application startup tests) to verify configuration, dependency injection wiring and database initialization in a production-like environment.

### 2. If a user submits changes to the same vacation request simultaneously from two different devices, how should the application detect this and how should it behave in such a situation?
I suggest adding a dedicated concurrency token to VacationEntity, for example RowVersion, to represent the current version of the record in the database. The value could be a hash derived from the previous version and the entity data. The API would send the latest version to the frontend and the frontend would include it in subsequent update requests. On update, the API would compare the submitted version with the current value in the database. If they differ, it means another device has already updated the same request and the API should return 409 Conflict. 


