# Calculator (Onion) – .NET 8

A clean Onion Architecture solution for a Calculator API:
- **Domain** → business contracts/entities
- **Application** → use-cases (orchestrate domain)
- **Infrastructure** → domain implementations/adapters
- **API** → ASP.NET Core Web API (JWT auth, Swagger)
- **Tests** → unit + integration (WebApplicationFactory)

## Solution Layout
```
Calculator/
  Calculator.sln
  Calculator.Domain/
  Calculator.Application/
  Calculator.Infrastructure/
  Calculator.Api/
  Calculator.Tests/
  Dockerfile
  docker-compose.yml
  .gitignore
  .dockerignore
```
> Ports used by default: HTTP **5029** (adjust in `launchSettings.json` or compose).

---

## Prerequisites
- .NET SDK **8.x** (`dotnet --version` → `8.0.x`)
- (Optional) Docker Desktop (for containerized run)
- (Optional) cURL / Postman for testing
- Dev HTTPS (only if you choose HTTPS locally): `dotnet dev-certs https --trust`

---

## Build & Run (local)

### Restore & Build
```bash
dotnet restore
dotnet build
```

### Run the API
```bash
dotnet run --project Calculator.Api/Calculator.Api.csproj
```
- Swagger UI: `http://localhost:5029/swagger` (or the port printed in console)

### Get JWT
```bash
curl -X POST "http://localhost:5029/api/auth/token"
```
Response:
```json
{ "token": "<JWT>", "expiresAt": "2025-09-16T21:45:00Z" }
```

### Call Calculator
```bash
TOKEN=$(curl -s -X POST "http://localhost:5029/api/auth/token" | jq -r .token)

curl -X POST "http://localhost:5029/api/calculator" \
  -H "Authorization: Bearer $TOKEN" \
  -H "X-Operation: add" \
  -H "Content-Type: application/json" \
  -d '{ "number1": 10, "number2": 5 }'
# => { "result": 15 }
```

---

## Running Tests
```bash
dotnet test
```
Includes:
- **Unit tests** for calculation logic
- **Integration tests** using `WebApplicationFactory<Program>`

---

## Configuration
`Calculator.Api/appsettings.json` (or environment variables):

- `Jwt:Issuer` (env: `Jwt__Issuer`)
- `Jwt:Audience` (env: `Jwt__Audience`)
- `Jwt:Key` (env: `Jwt__Key`)

Example (launchSettings or compose):
```json
"environmentVariables": {
  "ASPNETCORE_ENVIRONMENT": "Development",
  "Jwt__Issuer": "CalculatorApi",
  "Jwt__Audience": "CalculatorApiUsers",
  "Jwt__Key": "change_me_please_very_secret_key"
}
```

---

## Swagger / OpenAPI
- In-code Swagger via Swashbuckle: `/swagger`
- **Design-first** YAML (SwaggerHub requirement): use the provided YAML (see below).  
  Import it to **SwaggerHub**, fix validation if needed, then **Generate Server → aspnetcore** to create a server stub.

> Header **`X-Operation`** is required. In controller you can bind with:
> ```csharp
> public IActionResult Calculate([FromBody] OperationRequest body, [FromHeader(Name="X-Operation")] string operation)
> ```
> or add a **Swagger OperationFilter** to inject the header globally.

---

## Docker

### Build Image
```bash
docker build -t calculator-api:latest .
```

### Run Container
```bash
docker run --rm -p 5029:5029 \
  -e ASPNETCORE_ENVIRONMENT=Development \
  -e ASPNETCORE_URLS=http://+:5029 \
  -e Jwt__Issuer=CalculatorApi \
  -e Jwt__Audience=CalculatorApiUsers \
  -e Jwt__Key=change_me_please_very_secret_key \
  --name calculatorapi \
  calculator-api:latest
```
- Swagger: `http://localhost:5029/swagger`

### Docker Compose
```bash
docker compose up --build
# logs:
docker compose logs -f
# stop:
docker compose down
```

---

## HTTP Scratch File (optional)
`Calculator.Api/Calculator.Api.http` – for quick local testing (VS/VSCode):
```http
@Calculator_Api_Host = http://localhost:5029

### Get token
POST {{Calculator_Api_Host}}/api/auth/token

### Add
POST {{Calculator_Api_Host}}/api/calculator
Authorization: Bearer {{JWT_TOKEN}}
X-Operation: add
Content-Type: application/json

{
  "number1": 10,
  "number2": 5
}
```

---

## SwaggerHub (as requested in assignment)

1) Go to **swaggerhub.com** → create a free account.
2) Create a new API → choose **OpenAPI 3.0**.
3) Paste the YAML you have (see your `OpenApi/calculator-api.yaml`).
4) Fix any validation warnings & save.
5) Click **Generate Server** → choose **aspnetcore** (target .NET 8 if available, or adjust csproj to net8.0).
6) Download the ZIP and compare to your current solution (you already have a working implementation).

**CLI alternative (OpenAPI Generator docker image):**
```bash
docker run --rm -v ${PWD}:/local openapitools/openapi-generator-cli \
  generate -i /local/OpenApi/calculator-api.yaml \
  -g aspnetcore \
  -o /local/GeneratedStub \
  --additional-properties=packageName=CalculatorApi,targetFramework=net8.0,nullableReferenceTypes=true
```

---

## Common NuGets

API:
```bash
dotnet add Calculator.Api package Swashbuckle.AspNetCore -v 6.*
dotnet add Calculator.Api package Microsoft.AspNetCore.Authentication.JwtBearer -v 8.*
dotnet add Calculator.Api package System.IdentityModel.Tokens.Jwt -v 6.*
```

Tests:
```bash
dotnet add Calculator.Tests package Microsoft.AspNetCore.Mvc.Testing -v 8.*
dotnet add Calculator.Tests package xunit
dotnet add Calculator.Tests package xunit.runner.visualstudio
dotnet add Calculator.Tests package FluentAssertions
dotnet add Calculator.Tests package Microsoft.NET.Test.Sdk -v 17.10.*
```

---

## Notes
- Keep your **.gitignore** and **.dockerignore** at the solution root.
- If you change ports, update: `launchSettings.json`, `.http` file, `servers` in YAML, and compose mappings.
- For HTTPS in Docker, you’ll need to mount dev certs (not required for this assignment).

---

## License
MIT
# matrix
