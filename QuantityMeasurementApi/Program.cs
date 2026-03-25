using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuantityMeasurementApp;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Swagger + OpenAPI with JWT security definition
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Quantity Measurement API",
        Version = "v1",
        Description = "Quantity Measurement REST API with JWT authentication"
    });

    // Define the Bearer auth scheme (JWT)
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter 'Bearer {token}'. Example: 'Bearer eyJhbGciOi...'",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    options.AddSecurityDefinition("Bearer", securityScheme);

    // Make sure all operations can use this security scheme
    var securityRequirement = new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    };

    options.AddSecurityRequirement(securityRequirement);
});

// Access configuration
var configuration = builder.Configuration;

// === DbContext for operations + logs + users ===
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
string redisConnectionString = configuration.GetSection("Redis")["ConnectionString"] ?? "localhost:6379";
builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    ConnectionMultiplexer.Connect(redisConnectionString));

// === Repository wiring for logs ===
// For now, always use the in-memory + JSON cache repository for logs.
builder.Services.AddSingleton<IQuantityMeasurementRepository>(_ =>
    QuantityMeasurementCacheRepository.Instance);
Console.WriteLine("Using in-memory cache + JSON repository for logs.");

// === Repository wiring for operations ===
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

// === User repository ===
if (hasDatabase)
{
    builder.Services.AddScoped<IUserRepository, UserRepository>();
}

// === Business service ===
builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementService>();

// === JWT Authentication ===
var jwtSection = configuration.GetSection("Jwt");
string jwtKey = jwtSection["Key"] ?? throw new InvalidOperationException("Jwt:Key is missing.");
string jwtIssuer = jwtSection["Issuer"] ?? "QuantityMeasurementApi";
string jwtAudience = jwtSection["Audience"] ?? "QuantityMeasurementApiUsers";
int jwtExpiresMinutes = int.TryParse(jwtSection["ExpiresMinutes"], out int m) ? m : 60;

var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quantity Measurement API v1");
    });
}

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
if (hasDatabase)
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<QuantityMeasurementDbContext>();
        db.Database.Migrate();
    }
}

/// <summary>
/// Simple in-memory fallback for operations if no DB is configured.
/// NOT Redis + DB, only for emergency / dev without DB.
/// </summary>
public class InMemoryQuantityOperationRepository : IQuantityOperationRepository
{
    private readonly System.Collections.Generic.List<QuantityOperation> operations = new();

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