# Starship Shop REST API

A complete REST API backend for an online starship shop (a video game store that sells starships), built with ASP.NET Core 10.0, PostgreSQL, and Entity Framework Core.

## Features

- **Layered Architecture**: Models, Data, Services, Routers, Middleware, Validators, and Schemas
- **TPT Inheritance**: Table-Per-Type mapping for Starship hierarchy (PrivateVessel, PublicTransportVessel, CargoVessel)
- **JWT Authentication**: Secure authentication with BCrypt password hashing
- **Validation**: FluentValidation for request validation
- **Global Exception Handling**: Middleware for consistent error responses
- **Pagination**: Built-in pagination support for listing starships
- **OpenAPI**: Auto-generated API documentation

## Technologies

- ASP.NET Core 10.0 (.NET 10)
- PostgreSQL
- Entity Framework Core with Npgsql provider
- JWT Bearer Authentication
- FluentValidation
- BCrypt.Net for password hashing

## Project Structure

```
StarshipShop/
├── StarshipShop.sln
├── StarshipShop.Api/
│   ├── Models/           # Domain entities
│   ├── Data/             # Database context
│   ├── Services/         # Business logic
│   ├── Routers/          # API endpoints
│   ├── Middleware/       # Exception handling
│   ├── Validators/       # Request validation
│   └── Schemas/          # Request/Response DTOs
```

## Prerequisites

- .NET 10 SDK
- PostgreSQL 12+

## Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/miroPasko/ASPNET-API-AI-Demo.git
   cd ASPNET-API-AI-Demo
   ```

2. **Configure Database**
   
   Update the connection string in `appsettings.json` or use environment variables:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=starship_shop;Username=postgres;Password=postgres"
     }
   }
   ```

3. **Apply Migrations**
   ```bash
   cd StarshipShop.Api
   dotnet ef database update
   ```

4. **Run the Application**
   ```bash
   dotnet run
   ```

   The API will be available at `https://localhost:5001` (or the port configured in `launchSettings.json`)

## API Endpoints

### Authentication

- **POST** `/api/auth/login` - Login and get JWT token (AllowAnonymous)
  ```json
  {
    "email": "user@example.com",
    "password": "password123"
  }
  ```

### Starships (All require authentication)

- **GET** `/api/starships?page=1&pageSize=10` - List all starships with pagination
- **GET** `/api/starships/{id}` - Get a starship by ID
- **POST** `/api/starships/create` - Create a new starship
- **PUT** `/api/starships/{id}` - Update a starship
- **DELETE** `/api/starships/{id}` - Delete a starship

### Example: Create Private Vessel

```json
{
  "name": "Millennium Falcon",
  "manufacturer": "Corellian Engineering Corporation",
  "price": 100000,
  "engineId": 1,
  "ftlCapable": true,
  "ftlDriveId": 1,
  "totalCrew": 4,
  "totalCapacity": 6,
  "starshipType": "PrivateVessel",
  "vesselType": "Frigate"
}
```

### Example: Create Public Transport Vessel

```json
{
  "name": "Galaxy Express",
  "manufacturer": "TransGalactic Corp",
  "price": 500000,
  "engineId": 2,
  "ftlCapable": true,
  "ftlDriveId": 2,
  "totalCrew": 20,
  "totalCapacity": 500,
  "starshipType": "PublicTransportVessel",
  "transportClass": "Luxury",
  "totalPassengers": 480
}
```

### Example: Create Cargo Vessel

```json
{
  "name": "Heavy Hauler",
  "manufacturer": "Industrial Shipyards",
  "price": 750000,
  "engineId": 3,
  "ftlCapable": false,
  "totalCrew": 10,
  "totalCapacity": 100,
  "starshipType": "CargoVessel",
  "cargoType": "RawMaterials",
  "totalCargoCapacity": 50000
}
```

## Database Schema

### Core Entities

- **Users**: User accounts with email and password
- **PaymentDetails**: Payment information for users
- **Engines**: Propulsion systems for starships
- **FtlDrives**: Faster-than-light drives for starships
- **Starships**: Base starship entity (TPT inheritance)
  - **PrivateVessels**: Personal starships (Fighter, Frigate, etc.)
  - **PublicTransportVessels**: Passenger transport ships (Economy, Standard, Business, Luxury)
  - **CargoVessels**: Cargo transport ships (Mixed, Liquid, RawMaterials, Vehicles, Hazardous)
- **Sales**: Purchase transactions

### Relationships

- Starship → Engine: Restrict (cannot delete Engine if used by Starships)
- Starship → FtlDrive: SetNull (FTL drive is optional)
- Sale → User: Cascade (delete sales when user is deleted)
- Sale → Starship: Restrict (cannot delete Starship if sold)
- Sale → PaymentDetails: Restrict (cannot delete payment details if used in sales)

## Development

### Building

```bash
dotnet build
```

### Running Tests

```bash
dotnet test
```

### Creating Migrations

```bash
dotnet ef migrations add MigrationName
```

### Applying Migrations

```bash
dotnet ef database update
```

## Configuration

### JWT Settings

Configure JWT authentication in `appsettings.json`:

```json
{
  "Jwt": {
    "Key": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "StarshipShop",
    "Audience": "StarshipShopClients"
  }
}
```

⚠️ **Security Note**: For production, use environment variables or secure configuration management for sensitive values.

## OpenAPI Documentation

When running in Development mode, the OpenAPI specification is available at `/openapi/v1.json`

## License

MIT
