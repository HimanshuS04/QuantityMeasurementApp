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

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Quantity Measurement API",
        Version = "v1",
        Description = "Quantity Measurement REST API with JWT authentication"
    });

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

    var securityRequirement = new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    };

    options.AddSecurityRequirement(securityRequirement);
});

var configuration = builder.Configuration;

string? dbConnectionString = configuration.GetConnectionString("QuantityMeasurementDb");
bool hasDatabase = !string.IsNullOrWhiteSpace(dbConnectionString);

if (hasDatabase)
{
    builder.Services.AddDbContext<QuantityMeasurementDbContext>(options =>
        options.UseNpgsql(dbConnectionString));
}
else
{
    Console.WriteLine("Warning: QuantityMeasurementDb connection string is missing. DbContext will not be registered.");
}

string redisConnectionString = configuration.GetSection("Redis")["ConnectionString"] ?? "localhost:6379";
builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    ConnectionMultiplexer.Connect(redisConnectionString));

builder.Services.AddSingleton<IQuantityMeasurementRepository>(_ =>
    QuantityMeasurementCacheRepository.Instance);
Console.WriteLine("Using in-memory cache + JSON repository for logs.");

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

if (hasDatabase)
{
    builder.Services.AddScoped<IUserRepository, UserRepository>();
}

builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementService>();

var jwtSection = configuration.GetSection("Jwt");
string jwtKey = jwtSection["Key"] ?? throw new InvalidOperationException("Jwt:Key is missing.");
string jwtIssuer = jwtSection["Issuer"] ?? "QuantityMeasurementApi";
string jwtAudience = jwtSection["Audience"] ?? "QuantityMeasurementApiUsers";

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
            ValidateLifetime = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ClockSkew = TimeSpan.Zero,
            RoleClaimType = System.Security.Claims.ClaimTypes.Role,
            NameClaimType = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularApp", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200",
                "https://cosmic-pixie-bef91b.netlify.app/");
                
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quantity Measurement API v1");
    });
}

// app.UseHttpsRedirection();

app.UseCors("AngularApp");
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

if (hasDatabase)
{
    try
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<QuantityMeasurementDbContext>();
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Database migration failed: " + ex.Message);
    }
}

app.Run();

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

    public Task<IReadOnlyList<QuantityOperation>> GetByUserIdAsync(int userId)
    {
        var list = operations
            .Where(o => o.UserId.HasValue && o.UserId.Value == userId)
            .ToList()
            .AsReadOnly();
        return Task.FromResult((IReadOnlyList<QuantityOperation>)list);
    }
}