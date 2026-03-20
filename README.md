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

* **No relationships**- only a single entity.
