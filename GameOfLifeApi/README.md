# Conway's Game of Life API

A production-ready API for Conway's Game of Life implemented in C# using .NET 7. The API provides endpoints for creating a board, simulating generations, and retrieving board states. MongoDB is used as the persistence layer, and Docker Compose is provided to easily run MongoDB.

## Technologies Used

- **.NET 7 / ASP.NET Core**
- **MongoDB** 
- **Docker** (for running a local instance of MongoDB)
- **FluentResults** (for implementing the Result Pattern on the service layer)
- **xUnit & Moq**
- **ILogger**


## Getting Started

### Prerequisites

- .NET 7 SDK
- Docker

### Setup

1. Run MongoDB via Docker Compose:
Use the provided `docker-compose.yml` file to spin up MongoDB:

```bash
docker-compose up
```

2. Build and Run the API:

```bash
dotnet build
dotnet run
```

## API Endpoints

`POST /api/boards` - Create a new board using a JSON payload representing a 2D boolean array.

#### Payload Example:

```json
[
  [true, false, true],
  [false, true, false],
  [true, false, true]
]
```

`PUT /api/boards/{id}/next` - Advance the board by one generation.

`PUT /api/boards/{id}/advance/{steps}` - Advance the board a specified number of steps.

`PUT /api/boards/{id}/final` - Advance the board until it reaches a stable state (or until a maximum number of attempts is reached).

## Testing
The solution includes separate test projects for unit and integration tests, available at the `GameOfLifeTests` test project..

### Run Tests:

```bash
dotnet test
```