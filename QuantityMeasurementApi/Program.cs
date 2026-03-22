using Microsoft.EntityFrameworkCore;
using QuantityMeasurementApp;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Access configuration
var configuration = builder.Configuration;

// === DbContext for operations + logs ===
// Both QuantityOperations and QuantityMeasurementLogs live in this DB.
string? dbConnectionString = configuration.GetConnectionString("QuantityMeasurementDb");
bool hasDatabase = !string.IsNullOrWhiteSpace(dbConnectionString);

if (hasDatabase)
{
    builder.Services.AddDbContext<QuantityMeasurementDbContext>(options =>
        options.UseSqlServer(dbConnectionString));
}
else
{
    Console.WriteLine("Warning: QuantityMeasurementDb connection string is missing. DbContext will not be registered.");
}

// === Redis connection (for operations) ===
// We create a single multiplexer that can be used by operation repository.
string redisConnectionString = configuration.GetSection("Redis")["ConnectionString"] ?? "localhost:6379";
builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    ConnectionMultiplexer.Connect(redisConnectionString));

// === Repository wiring for logs ===
// For now, always use the in-memory + JSON cache repository for logs.
// This keeps the logging flow simple and isolates the Redis+DB work to operations.
builder.Services.AddSingleton<IQuantityMeasurementRepository>(_ =>
    QuantityMeasurementCacheRepository.Instance);
Console.WriteLine("Using in-memory cache + JSON repository for logs.");

// === Repository wiring for operations ===
// For operations we use Redis + EF repository when DB is configured.
// If DB is not configured, we fall back to a simple in-memory repository.
if (hasDatabase)
{
    builder.Services.AddScoped<IQuantityOperationRepository, QuantityOperationRedisRepository>();
    Console.WriteLine("Using Redis-backed operations repository with SQL Server persistence.");
}
else
{
    builder.Services.AddSingleton<IQuantityOperationRepository, InMemoryQuantityOperationRepository>();
    Console.WriteLine("Using in-memory operations repository (no database configured).");
}

// === Business service ===
builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
// app.UseAuthorization();
app.Map("/" ,() => "WELCOME TO QUANTITY MEASEUREMENT APP");
app.MapControllers();

app.Run();

/// <summary>
/// Simple in-memory fallback for operations if no DB is configured.
/// NOT Redis + DB, only for emergency / dev without DB.
/// </summary>
public class InMemoryQuantityOperationRepository : IQuantityOperationRepository
{
    private readonly List<QuantityOperation> operations = new();

    public Task SaveAsync(QuantityOperation operation)
    {
        if (operation == null) throw new ArgumentNullException(nameof(operation));
        operations.Add(operation);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<QuantityOperation>> GetAllAsync()
    {
        return Task.FromResult((IReadOnlyList<QuantityOperation>)operations.AsReadOnly());
    }

    public Task<IReadOnlyList<QuantityOperation>> GetByOperationTypeAsync(string operationType)
    {
        var list = operations
            .Where(o => string.Equals(o.OperationType, operationType, StringComparison.OrdinalIgnoreCase))
            .ToList()
            .AsReadOnly();
        return Task.FromResult((IReadOnlyList<QuantityOperation>)list);
    }

    public Task<IReadOnlyList<QuantityOperation>> GetByCategoryAsync(MeasurementCategory category)
    {
        var list = operations
            .Where(o => o.Category == category)
            .ToList()
            .AsReadOnly();
        return Task.FromResult((IReadOnlyList<QuantityOperation>)list);
    }
}