# QuantityMeasurementApp 📏

A comprehensive measurement application demonstrating object-oriented design, generics, and unit conversions across multiple categories: Length, Weight, Volume, and Temperature.

## Use Cases Implemented

### UC1 – Feet Measurement Equality ⚖️
**Goal:** Implement equality comparison between two feet values.

**What you implemented:**
- A simple `Feet` class with:
  - `private readonly double value;`
  - Constructor to set the value.
  - Overrode `Equals(object obj)` to compare:
    - If references are the same → true.
    - If obj is null → false.
    - If obj is not Feet → false.
    - Cast to Feet and compare value using `CompareTo` instead of `==`.
  - Basic `GetHashCode()` implementation based on value.
- A main/console entry that:
  - Creates example Feet objects.
  - Calls Equals to demonstrate true/false outputs.
- Tests (MSTest):
  - Same value equality (1.0 ft vs 1.0 ft → true).
  - Different value inequality (1.0 ft vs 2.0 ft → false).
  - Null comparison (feet vs null → false).
  - Different class comparison (feet vs "1.0" → false).
  - Same reference comparison (object equals itself → true).

**Concepts:**
- Properly overriding Equals (equality contract).
- Encapsulation and immutability (readonly field).
- Floating-point comparison using CompareTo.
- Basic unit testing patterns.

### UC2 – Feet and Inches Measurement Equality ⚖️
**Goal:** Add Inches as another length unit and support equality for it like Feet.

**What you implemented:**
- Introduced an `Inches` class with the same pattern as Feet:
  - Private readonly value.
  - Constructor.
  - Overridden Equals using the same rules.
- Unit tests similar to UC1 but for inches:
  - Same value.
  - Different value.
  - Null.
  - Different type.
  - Same reference.
- **Important note:** At this point, Feet and Inches were still separate classes; there was no cross-unit comparison (no feet vs inches equality). You began to see duplicate code in constructors, Equals, tests, etc.

**Concepts:**
- Applying the same equality logic to another value type.
- Recognizing duplication and the need for a more generic design.

### UC3 – Generic Length Quantity (QuantityLength + LengthUnit) 📦
**Goal:** Remove duplication between Feet and Inches by creating a generic length quantity type.

**What you implemented:**
- `LengthUnit` enum:
  - Values: Feet, Inch.
  - Each associated with a conversion factor relative to a base unit: Feet.
- `QuantityLength` class:
  - Fields: `double value; LengthUnit unit;`
  - Constructor takes value and LengthUnit.
  - Equals:
    - Converts both this and other to feet.
    - Compares base-unit values for equality.
- Updated tests:
  - 1.0 ft == 12.0 in (using QuantityLength with appropriate units).
  - Same-unit equality still works.

**Concepts:**
- DRY principle: Replaced separate Feet and Inches logic with a single class plus LengthUnit.
- Base-unit normalization: Convert both values to feet before comparison.
- Enum-based unit representation.

### UC4 – Extending Length with Yards and Centimeters 📏
**Goal:** Extend the length system with more units and prove the design is scalable.

**What you implemented:**
- Extended `LengthUnit` enum:
  - New values: Yard, Centimeter.
  - Extended conversion logic:
    - Yard: 1 yard = 3 feet.
    - Centimeter: 1 cm = 0.393701 inches → then converted to feet.
- `QuantityLength`:
  - Updated `ToBaseUnitInFeet()` (or equivalent) to handle the new units.
- Tests:
  - 1 yard == 3 feet.
  - 1 yard == 36 inches.
  - 1 cm == 0.393701 inches (or equivalent checks).
  - Cross-unit equality and transitivity (e.g., yard = feet = inch).

**Concepts:**
- Scalability of the enum + base-unit model.
- Multiple conversion paths all converging in the base unit.
- Maintaining equality correctness with more units.

### UC5 – Unit-to-Unit Conversion API (Same Category) 🔄
**Goal:** Expose a clear API to convert between units (e.g., feet → inches) instead of only checking equality.

**What you implemented:**
- In `QuantityLength`:
  - Static method: `Convert(double value, LengthUnit source, LengthUnit target)`
    - Validates input & units.
    - Converts via base feet: baseInFeet = sourceUnit.ConvertToBase(value). targetValue = baseInFeet / targetUnitFactor.
  - Instance method: `ConvertTo(LengthUnit targetUnit)`
    - Uses the same internal logic.
- Tests:
  - 1.0 ft → 12.0 in.
  - 24.0 in → 2.0 ft.
  - Same-unit conversion returns same value.
  - Round-trip: value → unitB → unitA returns approximately the original value.

**Concepts:**
- Clear separation: equality vs conversion.
- Reusing base-unit normalization for generic conversion.
- Input validation (finite numbers, defined enums).

### UC6 – Addition of Two Length Units ➕
**Goal:** Add ability to add two length measurements.

**What you implemented:**
- In `QuantityLength`:
  - `Add(QuantityLength other)`:
    - Converts both to feet.
    - Adds base values.
    - Returns result in the unit of the first operand by default.
  - `Add(QuantityLength other, LengthUnit resultUnit)`:
    - Same base calculation, but result converted to explicit result unit.
- Service/methods & menu:
  - Controller/menu logic to:
    - Read two values and units from user.
    - Call service to add.
    - Present result.
- Tests:
  - Same-unit addition (1 ft + 2 ft = 3 ft).
  - Cross-unit addition (1 ft + 12 in = 2 ft).
  - Zero and negative values.

**Concepts:**
- Arithmetic on value objects.
- Immutability: addition returns new quantity.
- Identity element (adding zero) and correctness.

### UC7 – Addition with Explicit Target Unit ➕
**Goal:** Provide flexibility to express the result in a specific unit, independent of operands.

**What you implemented:**
- Clarified overloading:
  - `Add(other)` → result in first operand's unit.
  - `Add(other, resultUnit)` → result in explicitly chosen unit.
- Menu updated to ask:
  - Result unit (e.g., ft/in/yd/cm).
- Tests:
  - Same inputs but different result units:
    - 1 ft + 12 in = 2 ft (result ft).
    - 1 ft + 12 in = 24 in (result in).
  - Commutativity for addition when using the same target unit.

**Concepts:**
- Method overloading for usability.
- Explicit vs implicit representation concerns.
- Maintaining consistent arithmetic logic while allowing flexible result units.

### UC8 – Refactor LengthUnit to Own Conversion Responsibility 🏗️
**Goal:** Improve cohesion and SOLID by moving conversion logic out of QuantityLength and into LengthUnit.

**What you implemented:**
- In `LengthUnit` (or extension):
  - `ConvertToBaseUnit(this LengthUnit unit, double value) → feet.`
  - `ConvertFromBaseUnit(this LengthUnit unit, double baseFeet) → target unit.`
- In `QuantityLength`:
  - Replaced inline conversion factors with calls to these methods.
  - Equals, Convert, Add, etc., now depend solely on LengthUnit conversion APIs.

**Concepts:**
- Single Responsibility Principle: Units handle conversions. Quantities handle arithmetic and equality.
- Cleaner separation and easier extension.

### UC9 – Weight Measurement (Kilogram, Gram, Pound) ⚖️
**Goal:** Add a new measurement category (weight) with similar functionality to length.

**What you implemented:**
- `WeightUnit` enum:
  - Kilogram (base), Gram, Pound.
- Extensions:
  - `ConvertToBaseUnit(this WeightUnit, double) → kg.`
  - `ConvertFromBaseUnit(...) → target weight unit.`
- `QuantityWeight` (initially) or direct `Quantity<WeightUnit>` (later):
  - Equality, conversion, addition.
- Service & menu:
  - New options for weight equality, conversion, and addition.
- Tests:
  - 1 kg == 1000 g.
  - 1 kg == ~2.20462 lb.
  - Cross-unit conversions and addition.

**Concepts:**
- Multi-category support using the same patterns.
- Domain separation: weight logic independent from length.

### UC10 – Generic Quantity<TUnit> (for Length + Weight) 📦
**Goal:** Eliminate duplicated quantity logic across categories with a generic quantity class.

**What you implemented:**
- `Quantity<TUnit>` generic class:
  - Works with any enum: LengthUnit, WeightUnit, etc.
  - Operations: Equals, GetHashCode. ConvertTo. Add with optional resultUnit.
  - Uses unit-specific ConvertToBaseUnit / ConvertFromBaseUnit based on TUnit.
- Started using `Quantity<LengthUnit>` and `Quantity<WeightUnit>` in services/tests.

**Concepts:**
- Generic programming to share logic across categories.
- `where TUnit : struct, Enum` ensures TUnit is an enum.
- DRY across multiple domains (length and weight).

### UC11 – Volume Measurement (Litre, Millilitre, Gallon) 🥛
**Goal:** Prove that the generic design scales to a new category: Volume.

**What you implemented:**
- `VolumeUnit` enum:
  - Litre (base), Millilitre, Gallon.
- Conversions:
  - 1 L = 1000 mL.
  - 1 gallon ≈ 3.78541 L.
- Extended `Quantity<TUnit>`:
  - Added unit-handling for VolumeUnit in ToBaseUnit and FromBaseUnit.
- Tests:
  - 1 L == 1000 mL.
  - 3.78541 L == 1 gallon.
  - Cross-unit conversions and addition.
- Service/menu:
  - Volume equality, conversion, and addition now available.

**Concepts:**
- Scalability of generic design: no new quantity class required.
- Only new enum + conversion rules were needed.

### UC12 – Subtraction and Division ➖➗
**Goal:** Add subtraction and division on quantities for all appropriate categories (length, weight, volume).

**What you implemented:**
- In `Quantity<TUnit>`:
  - `Subtract(Quantity<TUnit> other)` and `Subtract(Quantity<TUnit> other, TUnit resultUnit)`:
    - Convert both to base.
    - Subtract base values.
    - Convert result to chosen unit.
  - `double Divide(Quantity<TUnit> other)`:
    - Convert both to base.
    - baseThis / baseOther → dimensionless ratio.
    - Division by zero throws exception.
- Service and menu:
  - New menu options:
    - Subtract for length/weight/volume.
    - Divide for length/weight/volume.
- Tests:
  - Same-unit subtraction and cross-unit subtraction.
  - Results negative/zero where appropriate.
  - Division > 1, < 1, = 1.
  - Division-by-zero exception.

**Concepts:**
- Extending arithmetic while preserving immutability and correctness.
- Non-commutative operations.
- Centralized rule that division returns a scalar ratio.

### UC13 – Centralized Arithmetic Logic (Add/Subtract/Divide) 🧮
**Goal:** Remove duplicated arithmetic logic inside Quantity<TUnit> and enforce DRY at the method-body level.

**What you implemented:**
- Introduced:
  ```csharp
  private enum ArithmeticOperation { Add, Subtract, Divide }
  private double PerformBaseArithmetic(Quantity<TUnit> other, ArithmeticOperation operation)
  {
      // validation + base-unit normalization + operation
  }
  ```
- Add, Subtract, Divide now:
  - Call `PerformBaseArithmetic(other, operationType)`.
  - For add/subtract, convert base result back to resultUnit.
  - For division, return base ratio directly.

**Concepts:**
- Centralizing validation and arithmetic into one helper.
- Shorter, clearer public methods.
- Reduced maintenance cost and bug risk.

### UC14 – Temperature Measurement with Selective Arithmetic Support 🌡️
**Goal:** Add temperature as a measurement category and handle its unique constraints (equality and conversion only, no arithmetic).

**What you implemented:**
- `TemperatureUnit` enum:
  - Celsius, Fahrenheit, Kelvin.
- Extensions:
  - `ConvertToBaseUnit` (base: Celsius):
    - °C → °C, °F → °C, K → °C.
  - `ConvertFromBaseUnit`:
    - °C → target unit.
- Extended `Quantity<TUnit>`:
  - ToBaseUnit and FromBaseUnit support TemperatureUnit.
  - `ValidateOperationSupport` (conceptually):
    - For TUnit == TemperatureUnit, arithmetic operations are not supported.
    - When attempting: Add, Subtract, Divide on `Quantity<TemperatureUnit>`:
      - Throw `NotSupportedException` with clear message.
- Service & menu:
  - Temperature equality.
  - Temperature conversion.
  - No menu options for temperature arithmetic.
- Tests:
  - Celsius/Fahrenheit/Kelvin equality and conversion:
    - 0°C ↔ 32°F ↔ 273.15 K.
    - 100°C ↔ 212°F ↔ 373.15 K.
    - −40°C ↔ −40°F.
  - Exceptions for:
    - Add/Subtract/Divide on temperature.
    - Cross-category comparisons (temperature vs length/weight/volume) → false.

**Concepts:**
- Handling non-linear conversions (offset + scale).
- Category-specific constraints (temperature does not support arithmetic like length/weight/volume).
- Maintaining generic `Quantity<TUnit>` while selectively disabling certain operations for specific unit types.

### UC15 – DTO Architecture + JSON-Backed Cache (No DB, No ADO.NET, No EF) 📊
**Goal:** Introduce DTOs and a clean layered architecture with in-memory + JSON file persistence.

**What you implemented:**
- **DTOs:**
  - `QuantityMeasurementModel/QuantityDto.cs`:
    - Fields: `MeasurementCategory Category`, `string Unit`, `double Value`.
- **Service Interface (DTO-based):**
  - `QuantityMeasurementBusinessLayer/IQuantityMeasurementService.cs`:
    - Methods: `bool CompareQuantities(QuantityDto first, QuantityDto second)`, `QuantityDto ConvertQuantity(QuantityDto quantity, string targetUnit)`, `QuantityDto AddQuantities(QuantityDto first, QuantityDto second, string resultUnit)`, `QuantityDto SubtractQuantities(...)`, `double DivideQuantities(...)`.
- **Service Implementation:**
  - `QuantityMeasurementService` uses existing model classes (`Quantity`, `QuantityLength`, etc.) and logs via `IQuantityMeasurementRepository`.
- **Console UI (DTO-based):**
  - `QuantityMeasurementController/QuantityMenu.cs`:
    - Reads user input, creates `QuantityDto` objects, calls DTO methods on `IQuantityMeasurementService`.
- **Cache Repository + JSON Persistence:**
  - `QuantityMeasurementRepository/QuantityMeasurementCacheRepository.cs`:
    - Holds `List<QuantityMeasurementEntity>` in memory.
    - On startup: loads from `measurements_cache.json` if present.
    - On Save: appends to list and writes updated list to `measurements_cache.json` using `System.Text.Json`.
  - `QuantityMeasurementModel/QuantityMeasurementEntity.cs`:
    - Entity for logging operations (JSON-friendly with public setters + parameterless constructor).

**Concepts:**
- Clean layered architecture with DTOs separating UI from business logic.
- In-memory caching with JSON file persistence (no database).
- Repository pattern for data access abstraction.

### UC16 – ADO.NET Integration (SQL Server) for Logs/History 🗄️
**Goal:** Add raw ADO.NET to persist `QuantityMeasurementEntity` logs to SQL Server, maintaining DTO/service architecture.

**What you implemented:**
- **ADO.NET Repository:**
  - `QuantityMeasurementRepository/QuantityMeasurementDatabaseRepository.cs`:
    - Implements `IQuantityMeasurementRepository` using ADO.NET (`Microsoft.Data.SqlClient`).
    - Uses `SqlConnection`, `SqlCommand`, `SqlDataReader`.
    - Typical pattern:
      ```csharp
      using (var connection = new SqlConnection(connectionString))
      {
          connection.Open();
          using (var command = connection.CreateCommand())
          {
              command.CommandText = "... INSERT INTO QuantityMeasurementLogs ...";
              command.Parameters.AddWithValue("@Id", entity.Id);
              // ...
              command.ExecuteNonQuery();
          }
      }
      ```
    - All CRUD for `QuantityMeasurementEntity` via manual SQL + parameterized queries.
- **Configuration:**
  - Connection string & config via `appsettings.json` or config class.
  - Repository type selection (cache vs DB).
- **Program/App Wiring:**
  - `QuantityMeasurementController/Program.cs` or `AppConfig` helper:
    ```csharp
    IQuantityMeasurementRepository repository = useDatabase
        ? new QuantityMeasurementDatabaseRepository(connectionString)
        : QuantityMeasurementCacheRepository.Instance;
    IQuantityMeasurementService service = new QuantityMeasurementService(repository);
    var menu = new QuantityMenu(service);
    menu.ShowMainMenu();
    ```
  - Can run with cache+JSON (UC15) or switch to ADO.NET for DB logging.

**Concepts:**
- Raw ADO.NET for database operations (no ORM).
- Parameterized queries for security.
- Configurable persistence layers.

### UC17 – Web API + EF Core + Redis + Disconnected Architecture 🌐
**Goal:** Build ASP.NET Core Web API with EF Core ORM, Redis caching, and disconnected architecture for operations history.

**What you implemented:**
- **ASP.NET Core Web API:**
  - New project: `QuantityMeasurementApi`.
  - `Program.cs`: `AddControllers`, `AddSwaggerGen`, registers `QuantityMeasurementDbContext`, Redis connection, repositories.
  - `QuantityMeasurementApi/Controllers/QuantityApiController.cs`:
    - REST endpoints: `POST /api/v1/quantities/compare`, `POST /api/v1/quantities/convert`, `POST /api/v1/quantities/add`, `POST /api/v1/quantities/subtract`, `POST /api/v1/quantities/divide`, `GET /api/v1/quantities/ping`.
    - Uses `IQuantityMeasurementService` for calculations (same DTO service from UC15).
    - Uses `IQuantityOperationRepository` to record operations.
  - Swagger: `AddSwaggerGen()`, `UseSwagger()`, `UseSwaggerUI()` at `/swagger`.
- **EF Core (ORM):**
  - `QuantityMeasurementRepository/DBContext/QuantityMeasurementDbContext.cs`:
    ```csharp
    public class QuantityMeasurementDbContext : DbContext
    {
        public DbSet<QuantityMeasurementEntity> QuantityMeasurements { get; set; } = null!;
        public DbSet<QuantityOperation> QuantityOperations { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map to QuantityMeasurementLogs and QuantityOperations tables
        }
    }
    ```
  - Registered in `QuantityMeasurementApi/Program.cs`:
    ```csharp
    builder.Services.AddDbContext<QuantityMeasurementDbContext>(options =>
        options.UseSqlServer(dbConnectionString));
    ```
- **Redis + Disconnected Architecture:**
  - `QuantityMeasurementRepository/IQuantityOperationRepository.cs`: Abstracts operations storage.
  - `QuantityMeasurementRepository/Redis/QuantityOperationRedisRepository.cs`:
    - Uses Redis (`StackExchange.Redis`) and EF Core.
    - Disconnected pattern: writes to Redis first, then DB; handles DB offline by keeping data in Redis.
  - `QuantityMeasurementApi/Program.cs`:
    ```csharp
    builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
        ConnectionMultiplexer.Connect(redisConnectionString));
    if (hasDatabase)
    {
        builder.Services.AddScoped<IQuantityOperationRepository, QuantityOperationRedisRepository>();
    }
    else
    {
        builder.Services.AddSingleton<IQuantityOperationRepository, InMemoryQuantityOperationRepository>();
    }
    ```
  - `QuantityApiController`:
    ```csharp
    [HttpPost("compare")]
    public async Task<ActionResult<CompareResponse>> Compare([FromBody] CompareRequest request)
    {
        bool equal = quantityService.CompareQuantities(request.First, request.Second);
        var op = new QuantityOperation { ... ResultValue = equal ? 1.0 : 0.0 };
        await operationRepository.SaveAsync(op); // Redis-first, DB behind
        return Ok(new CompareResponse { Equal = equal });
    }
    ```

**Concepts:**
- RESTful Web API with Swagger documentation.
- EF Core ORM for database mapping.
- Redis as cache layer for disconnected operations.
- Repository pattern hiding persistence details.

### UC18 – JWT Authentication and Protected Quantity APIs 🔐
**Goal:** Add authentication to the Web API so that users can register, log in, and securely access protected application features.

**What you implemented:**
- Added an `AuthController` with:
  - `POST /api/v1/auth/register` for user registration
  - `POST /api/v1/auth/login` for user login
- Introduced a `User` entity and `Users` table for storing application users.
- Added `IUserRepository` and `UserRepository` using EF Core for user data access.
- Implemented secure password storage using PBKDF2 hashing with random salt in `PasswordHasher`.
- Configured JWT authentication in `Program.cs`:
  - signing key
  - issuer
  - audience
  - token expiry
  - Bearer authentication
- Protected quantity APIs using `[Authorize]`.
- Updated Swagger configuration to support Bearer token authentication for testing secured endpoints.

**Concepts:**
- JWT-based stateless authentication
- secure password hashing and salting
- role of authentication vs authorization
- EF Core-based user persistence
- protected Web API endpoints
- Swagger authentication integration

### UC21 – Microservices Architecture 🏗️
**Goal:** Convert the monolithic Quantity Measurement backend into a microservices architecture with separate services, databases, and API gateway.

**What you implemented:**

🔄 Split monolithic backend into 4 independent services:
- 🔐 **Auth Service** (port 5001): user registration, login, JWT token generation
- 📊 **Quantity Service** (port 5002): compare, convert, add, subtract, divide operations
- 📜 **History Service** (port 5003): user operation history, admin history access
- 🚪 **API Gateway** (port 5000): single entry point using Ocelot reverse proxy

🗄️ Each service has its own SQL Server database:
- AuthServiceDb → Users table
- QuantityServiceDb → QuantityOperations table
- HistoryServiceDb → OperationHistories table

📡 Implemented inter-service communication:
- Quantity Service sends operation data to History Service via HTTP after each operation

🛣️ Configured Ocelot API Gateway for centralized routing:
- `/api/v1/auth/*` → Auth Service
- `/api/v1/quantities/*` → Quantity Service
- `/api/v1/history/*` → History Service

🔑 All services share the same JWT key so tokens generated by Auth Service are validated by Quantity and History services

📖 Each service has its own Swagger UI for independent testing

🌐 Quantity operations are publicly accessible without login

🔒 History endpoints require JWT authentication

👑 Admin history endpoint requires Admin role

**Concepts:**

- 🏗️ microservices architecture
- 🚪 API gateway pattern using Ocelot
- 🗄️ separate database per service
- 📡 inter-service HTTP communication
- 🔑 JWT-based distributed authentication
- 🚀 independent service deployment
- 🛡️ service isolation and responsibility separation
- 📖 Swagger per service for independent testing
