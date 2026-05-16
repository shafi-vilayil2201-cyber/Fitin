# Fitin Backend

Fitin is a fitness platform backend built using ASP.NET Core and Clean Architecture.

## Architecture

- API Layer
- Application Layer
- Domain Layer
- Infrastructure Layer

## Features

- Authentication (JWT)
- Product Management
- Cart Management
- Wishlist Management
- Cloudinary Image Upload

## Tech Stack

- ASP.NET Core 8
- Entity Framework Core
- SQL Server
- JWT Authentication
- Cloudinary

## Run Project

```bash
dotnet restore
dotnet run
```

## Local Secrets

This project already has a `UserSecretsId`, so for local development keep real secrets out of `appsettings.json` and set them with `dotnet user-secrets`.

Run these from the repository root:

```bash
dotnet user-secrets set "SeedAdmin:Email" "admin@yourdomain.com" --project Fitin.API/Fitin.API.csproj
dotnet user-secrets set "SeedAdmin:Password" "your-strong-admin-password" --project Fitin.API/Fitin.API.csproj
dotnet user-secrets set "Jwt:Key" "your-strong-jwt-secret-at-least-32-chars" --project Fitin.API/Fitin.API.csproj
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=...;Database=...;User Id=...;Password=...;TrustServerCertificate=True;" --project Fitin.API/Fitin.API.csproj
dotnet user-secrets set "Cloudinary:CloudName" "your-cloud-name" --project Fitin.API/Fitin.API.csproj
dotnet user-secrets set "Cloudinary:ApiKey" "your-cloudinary-api-key" --project Fitin.API/Fitin.API.csproj
dotnet user-secrets set "Cloudinary:ApiSecret" "your-cloudinary-api-secret" --project Fitin.API/Fitin.API.csproj
dotnet user-secrets set "Razorpay:KeyId" "your-razorpay-key-id" --project Fitin.API/Fitin.API.csproj
dotnet user-secrets set "Razorpay:KeySecret" "your-razorpay-key-secret" --project Fitin.API/Fitin.API.csproj
```

Useful commands:

```bash
dotnet user-secrets list --project Fitin.API/Fitin.API.csproj
dotnet user-secrets remove "SeedAdmin:Password" --project Fitin.API/Fitin.API.csproj
```

## Deploy Backend To Azure

This API can be deployed to Azure App Service with `.NET 8`.

### 1. Create Azure resources

- Create an `Azure SQL Database`
- Create an `App Service Plan`
- Create a `Web App` with runtime `.NET 8 (LTS)`

### 2. Configure App Service environment variables

In `App Service -> Settings -> Environment variables`, add:

- `ConnectionStrings__DefaultConnection`
- `Jwt__Key`
- `Jwt__Issuer`
- `Jwt__Audience`
- `Cloudinary__CloudName`
- `Cloudinary__ApiKey`
- `Cloudinary__ApiSecret`
- `Razorpay__KeyId`
- `Razorpay__KeySecret`
- `Cors__AllowedOrigins__0`
- `SeedAdmin__Email`
- `SeedAdmin__Password`

Example values:

```text
ConnectionStrings__DefaultConnection=Server=tcp:<server>.database.windows.net,1433;Initial Catalog=<db>;Persist Security Info=False;User ID=<user>;Password=<password>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
Jwt__Key=<strong-random-secret-at-least-32-chars>
Jwt__Issuer=Fitin
Jwt__Audience=FitinUsers
Cloudinary__CloudName=<cloudinary-name>
Cloudinary__ApiKey=<cloudinary-key>
Cloudinary__ApiSecret=<cloudinary-secret>
Razorpay__KeyId=<razorpay-key-id>
Razorpay__KeySecret=<razorpay-key-secret>
Cors__AllowedOrigins__0=https://your-frontend-domain.com
SeedAdmin__Email=admin@yourdomain.com
SeedAdmin__Password=<strong-admin-password>
```

Notes:

- EF Core migrations run automatically on startup.
- In non-development environments, no admin user is seeded unless `SeedAdmin__Email` and `SeedAdmin__Password` are configured.
- Use `/health` to verify the app is up after deployment.

### 3. Publish locally

```bash
dotnet publish Fitin.API/Fitin.API.csproj -c Release
```

### 4. Azure CLI zip deploy example

```bash
dotnet publish Fitin.API/Fitin.API.csproj -c Release -o ./publish
cd publish
zip -r ../fitin-api.zip .
az webapp deploy --resource-group <resource-group> --name <app-name> --src-path ../fitin-api.zip --type zip
```

### 5. Verify

- Open `https://<app-name>.azurewebsites.net/health`
- Confirm the Azure SQL firewall allows access
- Test your frontend against the deployed API base URL
