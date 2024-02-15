# UsersAPI

## Description
This project is an API that allows the CRUD operations on the users table.

## Configuración
The appsettings.json file contains the database connection string. Before running the code, be sure to modify the connection string corresponding to the "Production" tag to match your database environment.

```txt
{
  "ConnectionStrings": {
    "Production": "HERE_YOU SHOULD_INSERT_YOUR_CONNECTION_STRING"
  }
```

### Database de Datos
The code for this project requires a table called `tbUsers` in a SQL Server database. Make sure you create this table before running the code. You can use the following SQL script to create the table:

```sql
CREATE TABLE tbUsers (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(50),
    UserID INT PRIMARY KEY,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    Email NVARCHAR(100)
    DateOfBirth date,
    RetirementDate date
);
