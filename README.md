# EducationPlatform

EducationPlatform is an ASP.NET Core API for an online learning platform. The project covers course management, lessons, subscriptions, protected lesson access, authentication, database migrations, and file-backed media uploads.

## Features

- Course CRUD with creator ownership checks.
- Course preview image upload and retrieval.
- Lesson creation with summary, description, duration, media type, and lesson text.
- Lesson video upload and protected video streaming.
- Student subscriptions to published courses.
- Access control for lesson content: only admins, course creators, or users with an active subscription can view lessons and videos.
- JWT/Keycloak authentication with development authentication support.
- PostgreSQL persistence with EF Core migrations.
- Swagger UI and Postman collection for API testing.

## Project Structure

- `AspNetCore` - HTTP API, authentication, endpoint mapping, file storage.
- `Application` - request/handler business logic and response models.
- `Data` - EF Core database context and entities.
- `PostgreSql` - EF Core migrations and PostgreSQL registration.
- `Common` - shared enums and common models.
- `AspNetCore.Tests` - automated tests for lesson creation and subscription-based access.

## Requirements

- .NET SDK 10
- Docker Desktop
- PostgreSQL and Keycloak via `docker-compose.yml`
- Postman, optional but useful for manual API testing

## Run With Docker

```powershell
docker compose up --build
```

The compose setup starts:

- API: `http://localhost:5045`
- Keycloak: `http://localhost:8080`
- PostgreSQL: `localhost:5432`

The API applies EF Core migrations automatically during startup.

For a clean demo reset:

```powershell
docker compose down -v
docker compose up --build
```

Use `down -v` only when you intentionally want to delete local database data.

## Swagger

After the API starts, open:

```text
http://localhost:5045/swagger
```

Swagger is useful for quickly reviewing available endpoints. For authenticated requests, use a valid JWT access token from Keycloak.

## Postman

Import:

```text
EducationPlatform.postman_collection.json
```

Useful collection variables:

- `baseUrl`: `http://localhost:5045`
- `keycloakUrl`: `http://localhost:8080`
- `keycloakRealm`: `Education platform`
- `courseId`: created course id
- `lessonId`: created lesson id
- `coursePreviewImageFile`: local image path, for example `C:\temp\course-preview.png`
- `lessonVideoFile`: local video path, for example `C:\temp\lesson-video.mp4`

File testing endpoints:

- `POST /courses/{courseId}/preview-image` with `multipart/form-data`, field name `file`
- `GET /courses/{courseId}/preview-image`
- `POST /lessons/{lessonId}/video` with `multipart/form-data`, field name `file`
- `GET /lessons/{lessonId}/video`

## Access Rules

Lesson data and lesson videos are protected. A user can access them when at least one condition is true:

- the user is an admin;
- the user is the creator of the course;
- the user has an active subscription to the course that owns the lesson.

Pending subscriptions do not grant access.

## Tests

Run:

```powershell
dotnet test LearningPlatform.sln
```

Current tests cover:

- lesson creation persists course relation, lesson text, and media type;
- lesson access is forbidden without an active subscription;
- lesson access succeeds with an active subscription.

## File Storage

Uploaded files are stored under the API content root:

```text
storage/
```

The `storage/` directory is ignored by Git because it contains runtime uploads.
