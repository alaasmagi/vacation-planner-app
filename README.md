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
BACKEND_PORT=<your-backend-port>
DEFAULT_VACATION_LENGTH=<legal-max-vacation-days>
```
SQLite DB is located in the backend root folder `/vacation-planner-backend`  
The example of `.env` has been provided in `/vacation-planner-backend/.env.example`  

If You want to use default values, you can just use the command `cp .env.example .env` in the backend root folder `/vacation-planner-backend`  

Frontend should also have .env file in the frontend root folder `/vacation-planner-client` which has following content:
```bash
VITE_FRONTEND_PORT=<your-frontend-port>
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
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseEntity"/> class with a new identifier value.
    /// </summary>
    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }
}

public abstract class BaseEntity<TKey> : IBaseEntity<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Gets or sets the unique identifier of the entity.
    /// </summary>
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
IVacationRequestService inherits from IBaseService interface and IVacationRequesRepository inherits from IBaseRepository. Both base interfaces are a part of NuGet packages **alaasmagi.Base.Contracts.Application** and **alaasmagi.Base.Contracts.DataAccess**. For more details, click [here](#alaasmagibase-nuget-packages).

#### DTO layer
* **VacationRequestEntity:** Database entity which keeps both Domain data and meta data
VacationRequestEntity inherits from **BaseEntityWithMeta** which is part of the NuGet package **alaasmagi.Base.Domain**. For more details, click [here](#alaasmagibase-nuget-packages).
* **VacationRequestDto** & **VactionRequestWebDto:** DTOs which hide the unnecessary datafields from UI and API
* **VacationRequestError:** Static class which holds standardised error code and message for duplicate vacation request entry
* Mappers for each of the DTOs: **VacationRequestMapper**, **VacationRequestDtoMapper** & **VacationRequestWebDtoMapper**
Both mappers inherit from **IMapper** which is part of the NuGet package **alaasmagi.Base.Contracts.DTO**. For more details, click [here](#alaasmagibase-nuget-packages).

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

## Testing

### Unit tests
Unit tests in this project focus on the backend’s core logic, repository layer, domain rules, and mapper behavior rather than the web layer. The service layer is covered through vacation request creation, updates, deletion, and retrieval flows, with specific checks that overlapping date ranges for the same employee are detected and duplicate requests are rejected. Repository tests validate CRUD operations and overlap detection against the real data access layer using an isolated in-memory SQLite database, which keeps the test suite fast and stable. Mapper tests verify correct translation between domain, database, and DTO models, including default values, computed fields, and collection mapping. There are also direct domain tests for date validity, vacation duration calculation, and overtime vacation logic. Overall, the suite provides solid coverage of the application’s business rules and data transformations without adding the overhead of HTTP-layer testing.

### Testing improvement
Additional tests could still improve confidence in edge cases and integration behavior. Useful next additions would include HTTP endpoint tests for request validation and response codes, service and repository tests for boundary date cases such as adjacent non-overlapping ranges, update scenarios that should preserve existing data correctly, and failure-path tests for invalid DTO input or missing entities. It would also be valuable to add broader integration or application startup tests to verify configuration, dependency wiring, and database initialization in a production-like setup.

## Visuals

  
