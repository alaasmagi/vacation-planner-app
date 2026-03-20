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
The core and base logic of the application is modularized into separate NuGet packages **alaasmagi.Base.*** [GitHub link](https://github.com/alaasmagi/alaasmagi-base-nuget/tree/main/Base.Domain), containing reusable domain models, shared abstractions and foundational realisation components. This package is developed and maintained by myself and serves as a shared base across projects.

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

The **BaseEntity** class is not defined in this project itself, it comes from the NuGet package **alaasmagi.Base.Domain** package. For more details, click [here](#alaasmagibase-nuget-packages)

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
* **VacationRequestService** - Responsible for applying business rules and communicating with database via **IVacationRequestRepository** interface
VacationRequestService class inherits from BaseService class, which is part of the NuGet package **alaasmagi.Base.Application** ([NuGet link](https://www.nuget.org/packages/alaasmagi.Base.Application), [GitHub link](https://github.com/alaasmagi/alaasmagi-base-nuget/tree/main/Base.Application)), which is published and maintained by myself.

#### Contract layer
* **IVacationRequestService**
* **IVacationRequestRepository**
IVacationRequestService inherits from IBaseService interface and IVacationRequesRepository inherits from IBaseRepository. Both base interfaces come from NuGet packages **alaasmagi.Base.Contracts.Application** ([NuGet link](https://www.nuget.org/packages/alaasmagi.Base.Contracts.Application), [GitHub link](https://github.com/alaasmagi/alaasmagi-base-nuget/tree/main/Base.Contracts.Application)) and  **alaasmagi.Base.Contracts.DataAccess** ([NuGet link](https://www.nuget.org/packages/alaasmagi.Base.Contracts.DataAccess), [GitHub link](https://github.com/alaasmagi/alaasmagi-base-nuget/tree/main/Base.Contracts.DataAccess)), which are published and maintained by myself.

#### DTO layer
* **VacationRequestEntity** - Database entity which keeps both Domain data and meta data
VacationRequestEntity inherits from BaseEntityWithMeta which is part of
* **VacationRequestDto** & **VactionRequestWebDto** - DTOs which hide the unnecessary fields from UI and API  
* **VacationRequestError** - Static class which holds standardised error code and message for duplicate vacation request entry
* Mappers for each of the DTOs - **VacationRequestMapper**, **VacationRequestDtoMapper** & **VacationRequestWebDtoMapper**

#### Web layer
* **BookingService** - Responsible for validating external input, fetching data via IBookingRepository, mapping data into Data Transfer Objects(DTOs) via BookingMapper. 
* **TableService** - Responsible for validating external input, fetching data via ITableRepository, mapping data into Data Transfer Objects(DTOs) via TableMapper. 
* **Contracts** - IRepository, IBookingRepository, ITableRepository
* **DTOs** - BookingDto, CreateBookingDto, PositionDto, TableDto, VerifyPasswordDto
* **Exceptions** - ApiException, ConflictException, NotFoundException, ValidationException
* **Mappers** - BookingMapper, TableMapper

#### Helpers
* **BookingService** - Responsible for validating external input, fetching data via IBookingRepository, mapping data into Data Transfer Objects(DTOs) via BookingMapper. 
* **TableService** - Responsible for validating external input, fetching data via ITableRepository, mapping data into Data Transfer Objects(DTOs) via TableMapper. 
* **Contracts** - IRepository, IBookingRepository, ITableRepository
* **DTOs** - BookingDto, CreateBookingDto, PositionDto, TableDto, VerifyPasswordDto
* **Exceptions** - ApiException, ConflictException, NotFoundException, ValidationException
* **Mappers** - BookingMapper, TableMapper
  
