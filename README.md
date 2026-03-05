# HackerNewsDataCompiler

API that retrieves and caches the best stories from [Hacker News]

# What I would improve

- Split the project into separate layers, such as Domain and Infrastructure, to better support growth
- Replace the current cache with Redis or another distributed cache solution
- Add pagination to improve performance at scale

# How I built it

- I built this API with a decoupled architecture, inspired by Clean Architecture and DDD principles
- Following the first point above, the project is structured to scale easily
- The Repository pattern also makes it straightforward to swap the data source with minimal effort
- I was unsure whether "comments" referred to direct replies (kids list) or all descendants, so I went with descendants based on the API documentation
- The ResponseModel centralizes error handling and avoids relying on exceptions for flow control
- The test coverage could be broader, but the most critical cases are included

## Requirements

| [.NET SDK](https://dotnet.microsoft.com/download) | 8.0+ |
| [Docker Desktop](https://www.docker.com/products/docker-desktop/) | Any recent version |

## Running with Docker (recommended)

### 1. Navigate to the correct folder

The `docker-compose.yml` is located in the `HackerNewsDataCompiler/` folder:

```bash
cd HackerNewsDataCompiler/HackerNewsDataCompiler
```

### 2. Start the container

```bash
docker compose up -d --build
```

The API will be available at **http://localhost:8080**.

### 4. Stop the container

```bash
docker compose down
```

## Running with dotnet run

### 1. Navigate to the API project

```bash
cd HackerNewsDataCompiler/HackerNewsDataCompiler/HackerNewsDataCompiler.API
```

### 2. Restore dependencies and run

```bash
dotnet run
```

The API will start at **http://localhost:5156** (HTTP) or **https://localhost:7249** (HTTPS).

The Swagger UI will open automatically at `http://localhost:5156/swagger`.

---

### If in production. Configure the environment file

Modify a `.env` file in that same folder (next to `docker-compose.yml`):

```env
ASPNETCORE_ENVIRONMENT=Production
```

> The API already has sensible defaults in `appsettings.json`. The `.env` file is required by the compose file — add any overrides there if needed.

## Available endpoint

GET | `/api/Stories/BestStoriesDetails` | Returns the top Hacker News stories with details

### Example request

```bash
curl http://localhost:8080/api/Stories/BestStoriesDetails
```

### Example response

```json
[
  {
    "title": "Story title",
    "uri": "https://example.com",
    "postedBy": "username",
    "time": "2026-03-05T10:00:00Z",
    "score": 342,
    "commentCount": 87
  }
]
```

## Running tests

```bash
cd HackerNewsDataCompiler
dotnet test
```