# Docker Run Guide

This setup runs the platform as three containers:

- `education-api` - ASP.NET Core API
- `education-postgres` - PostgreSQL for the API and Keycloak
- `education-keycloak` - Keycloak auth server

## Ports

- API: `http://localhost:5045`
- Keycloak: `http://localhost:8080`
- PostgreSQL: `localhost:5432`

## First Run

If another Keycloak container is already using port `8080`, stop it first:

```powershell
docker stop keycloak
```

Then run the project:

```powershell
docker compose up --build
```

For detached mode:

```powershell
docker compose up --build -d
```

## Database

PostgreSQL creates two databases on first volume initialization:

- `education`
- `keycloak`

The API applies EF Core migrations automatically on startup.

## Keycloak Realm Import

To make the project portable, export your configured `education-platform` realm from Keycloak and put the JSON file here:

```text
docker/keycloak/import/education-platform-realm.json
```

The compose file starts Keycloak with:

```text
start-dev --import-realm
```

If the PostgreSQL Docker volume already exists, Keycloak will not re-import over an existing realm. For a clean demo reset:

```powershell
docker compose down -v
docker compose up --build
```

Only use `down -v` when you intentionally want to delete local container database data.

## Postman

Use the existing Postman collection with:

- `baseUrl`: `http://localhost:5045`
- `keycloakUrl`: `http://localhost:8080`
- `keycloakRealm`: `education-platform`
- `keycloakPostmanClientId`: `education-postman`

Use collection Authorization with OAuth 2.0 Authorization Code with PKCE.
