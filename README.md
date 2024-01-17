# StaffScreening Application
## Introduction
The StaffScreening app is designed to facilitate the screening process for staff members. It provides functionalities for user registration, login, and a questionnaire to assess eligibility for work based on predefined criteria.

## Main Features
- **User Registration:** New users can create an account by providing their email and a password.
- **User Login:** Registered users can log in to access the questionnaire.
- **Questionnaire:** Users complete a series of questions to determine their screening outcome.
- **Screening Outcome:** Based on the questionnaire responses, the system determines if the user has passed or failed the screening.

## System Requirements
To run the StaffScreening app, the following requirements must be met:
- .NET 6.0 SDK or later
- An SQL Server instance accessible via a connection string
- Visual Studio (recommended for development and testing)

## Installation
Before running the application, ensure the connection string in the `appsettings.json` file points to your SQL Server instance:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=StaffScreening;Integrated Security=SSPI;Encrypt=True;TrustServerCertificate=True;"
}
```

Replace `YOUR_SERVER_NAME` with the name of your SQL Server instance.

## Package Details
The application relies on various NuGet packages, outlined in the tables below:

### StaffScreening App Packages

| Package                                 | Version |
|-----------------------------------------|---------|
| BCrypt.Net-Next                         | 4.0.3   |
| Microsoft.EntityFrameworkCore           | 6.0.26  |
| Microsoft.EntityFrameworkCore.Design    | 6.0.26  |
| Microsoft.EntityFrameworkCore.SqlServer | 6.0.26  |

### StaffScreening.Tests Packages

| Package                         | Version |
|---------------------------------|---------|
| coverlet.collector              | 3.1.2   |
| Microsoft.AspNetCore.Mvc.Testing | 6.0.26 |
| Microsoft.EntityFrameworkCore.InMemory | 6.0.26 |
| Microsoft.NET.Test.Sdk          | 17.1.0  |
| Moq                             | 4.20.70 |
| Moq.EntityFrameworkCore         | 6.0.1.4 |
| MSTest.TestAdapter              | 2.2.8   |
| MSTest.TestFramework            | 2.2.8   |

Running the Application
1.Clone the repository to your local machine.
2.Open the solution in Visual Studio.
3.Restore the NuGet packages.
4.Run the StaffScreeningScript to set up the database.
5.Ensure the SQL Server instance is running and accessible.
6.Build and run the application.
