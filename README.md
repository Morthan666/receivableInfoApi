# ReceivableInfoApi
## Features
Endpoints are described on the Swagger UI (available when running the API without Docker)
- **http://localhost:5000/swagger/index.html**

## Prerequisites

Before running this API, ensure you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/download) (version 6.0)
- [Docker Desktop](https://docs.docker.com/desktop/install/windows-install/) (optional)
## Getting Started

To run this API on your local machine as a Docker container, follow these steps:

1. Clone this repository.
2. Navigate to the project directory.
3. Run `docker-compose up` to start the container.

Alternatively to run this API without Docker:
2. Navigate to the project directory/src/ReceivableInfoApi.WebApi
3. Run `dotnet restore` to restore the dependencies.
4. Run `dotnet run` to start the server.


### Currently only in-memory database is available
