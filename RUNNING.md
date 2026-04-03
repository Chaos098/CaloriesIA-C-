# Running the Project

## Prerequisites

- Docker Desktop (recommended path)
- Or local setup:
  - .NET SDK/runtime compatible with `net9.0`
  - PostgreSQL (for `UserService`)

---

## Option 1: Run Everything with Docker (Recommended)

From repository root:

```bash
docker compose up --build
```

This starts:

- `postgres` on `localhost:5432`
- `user-service` on `localhost:5238`
- `nutrition-service` on `localhost:5164`
- `image-analysis-service` on `localhost:5108`
- `calorie-computation-service` on `localhost:5284`
- `push-notification-service` on `localhost:5146`
- `bff` on `localhost:5035`

Stop services:

```bash
docker compose down
```

Stop and remove DB volume (fresh DB next start):

```bash
docker compose down -v
```

---

## Option 2: Run Locally (Without Docker)

1) Start PostgreSQL and create/access:
- Database: `aicalorie`
- Username: `postgres`
- Password: `postgres`

2) Ensure `UserService` connection string is set in:
- `src/Services/UserService/UserService.Api/appsettings.Development.json`

3) Run each API in separate terminals from repo root:

```bash
dotnet run --project src/Services/UserService/UserService.Api/UserService.Api.csproj
dotnet run --project src/Services/NutritionService/NutritionService.Api/NutritionService.Api.csproj
dotnet run --project src/Services/ImageAnalysisService/ImageAnalysisService.Api/ImageAnalysisService.Api.csproj
dotnet run --project src/Services/CalorieComputationService/CalorieComputationService.Api/CalorieComputationService.Api.csproj
dotnet run --project src/Services/PushNotificationService/PushNotificationService.Api/PushNotificationService.Api.csproj
dotnet run --project src/Gateways/AICalorieCounter.Bff.Api/AICalorieCounter.Bff.Api.csproj
```

---

## Quick Health Checks

```bash
curl http://localhost:5035/health
curl http://localhost:5238/health
curl http://localhost:5164/health
curl http://localhost:5108/health
curl http://localhost:5284/health
curl http://localhost:5146/health
```

---

## Quick API Tests via BFF

Get service URL map:

```bash
curl http://localhost:5035/api/gateway/services
```

Get nutrition sample:

```bash
curl http://localhost:5035/api/gateway/nutrition/apple
```

Analyze image (stubbed response):

```bash
curl -X POST http://localhost:5035/api/gateway/image/analyze \
  -H "Content-Type: application/json" \
  -d '{"imageUrl":"https://example.com/meal.jpg"}'
```

Compute calories:

```bash
curl -X POST http://localhost:5035/api/gateway/calories/compute \
  -H "Content-Type: application/json" \
  -d '{
    "items":[
      {"name":"apple","calories":52,"protein":0.3,"carbohydrates":14,"fat":0.2},
      {"name":"banana","calories":89,"protein":1.1,"carbohydrates":23,"fat":0.3}
    ]
  }'
```

Send notification (stubbed response):

```bash
curl -X POST http://localhost:5035/api/gateway/notifications/send \
  -H "Content-Type: application/json" \
  -d '{
    "deviceToken":"demo-token",
    "title":"Hydration Reminder",
    "message":"Drink water now."
  }'
```

---

## Android Emulator Note

If MAUI Android app calls backend from emulator:

- Use `http://10.0.2.2:5035` for BFF (not `localhost`)

