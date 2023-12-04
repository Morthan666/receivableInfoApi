# ReceivableInfoApi
## Features
Endpoints are described on the Swagger UI
- **https://localhost:5001/swagger/index.html**

## Prerequisites

Before running this API, ensure you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/download) (version 6.0)
- [Docker Desktop](https://docs.docker.com/desktop/install/windows-install/)
## Getting Started

To run this API on your local machine as a Docker container, follow these steps:

1. Clone this repository.
2. Navigate to the project directory.
3. Run `docker-compose up` to start the database container.
4. Navigate to the project directory/src/ReceivableInfoApi.WebApi
5. Run `dotnet restore` to restore the dependencies.
6. Run `dotnet run` to start the server.


Local database can be accessed using following connection string: `server=localhost;port=5433;database=receivableinfodb-local;uid=postgres;password=example`
