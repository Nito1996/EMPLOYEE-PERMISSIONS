EmployeePermissions API
EmployeePermissions is a robust and scalable API developed in ASP.NET Core that enables organizations to manage employee permissions efficiently. 
This application provides a comprehensive set of CRUD (Create, Read, Update, Delete) operations for handling employee permissions, 
integrating a relational database (SQL Server) to ensure data integrity and performance. 
Designed with maintainability and ease of use in mind, the API includes Swagger documentation for seamless testing and integration.

Key Features
Permission Management: Create, read, update, and delete employee permissions with ease.
Permission Types: Define and manage different types of permissions (e.g., vacations, medical leave, etc.).
Relational Database: Utilizes SQL Server for structured and efficient data storage.
RESTful Design: Follows REST principles, making it compatible with frontend applications and other services.
Automatic Documentation: Integrated Swagger (OpenAPI) for interactive API documentation and testing.
Validation and Error Handling: Robust validation and error handling to ensure data integrity and a smooth user experience.
Scalable Architecture: Built with scalability in mind, allowing for future enhancements and integrations.

Technologies Used
Backend Framework: ASP.NET Core
Database: SQL Server
API Documentation: Swagger (OpenAPI)
Version Control: Git
Dependency Management: NuGet
Logging: Microsoft.Extensions.Logging

Getting Started
Follow these steps to set up and run the EmployeePermissions API on your local machine.

Prerequisites
Before you begin, ensure you have the following installed:
.NET SDK (version 3.1 or later)
SQL Server (or a compatible database)
Git (optional, for version control)

Installation
1- Clone the Repository: Open your terminal and run the following command to clone the repository: git remote add origin https://github.com/Nito1996/EMPLOYEE-PERMISSIONS.git
2- Navigate to the Project Directory: Move into the project folder: cd EmployeePermissions
3- Configure the Database: Open the appsettings.json file and update the connection string to match your SQL Server configuration:
"ConnectionStrings": {
    "Connection": "Server=your-server;Database=EmployeePermissionsDB;User Id=your-user;Password=your-password;"
}
If the database does not exist, it will be created automatically when you run the application for the first time.
4- Run the Application: Start the API by running the following command: dotnet run.
5- Access the Swagger UI: Open your browser and navigate to http://localhost:5000 to access the Swagger UI. Here, you can explore and test all available endpoints.

API Endpoints
The API provides the following endpoints for managing employee permissions:
Permissions
GET /api/EmployeePermissions: Retrieve a list of all permissions.
GET /api/EmployeePermissions/{id}: Retrieve a specific permission by its unique ID.
POST /api/EmployeePermissions: Create a new permission.
PUT /api/EmployeePermissions/{id}: Update an existing permission.
DELETE /api/EmployeePermissions/{id}: Delete a permission.

Permission Types: Permission types are managed automatically when creating or updating permissions.
