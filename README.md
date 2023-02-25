## About

A RESTful API project that handles basic CRUD operations for students and teachers. 
Uses FluentValidation, MediatR and EF Core. Also demonstrates CQRS by separating the queries (read) from the commands (write).

## How to use
- Just run the API project which opens the Swagger UI for you to explore the APIs. You may also use some other tools such as cURL / Postman
- By default it uses an In-Memory Database Provider. If you wish to persist data in a local :
	- Update appsettings.json and change UseInMemoryDatabase to false
	- Run the following command:

		`dotnet ef migrations add "InitializeDatabase" --project Infrastructure --startup-project Api --output-dir Data\Migrations`
