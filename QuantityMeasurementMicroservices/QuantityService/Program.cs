using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuantityService.BusinessLogic;
using QuantityService.Data;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Quantity Service API",
        Version = "v1",
        Description = "Quantity operations microservice"
    });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter 'Bearer {token}'",
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
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

var configuration = builder.Configuration;

// === Database ===
string? connString = configuration.GetConnectionString("QuantityDb");
bool hasDatabase = !string.IsNullOrWhiteSpace(connString);

if (hasDatabase)
{
    builder.Services.AddDbContext<QuantityDbContext>(options =>
        options.UseSqlServer(connString));
}
else
{
    Console.WriteLine("Warning: QuantityDb connection string is missing.");
}

// === Redis ===
string redisConnectionString = configuration.GetSection("Redis")["ConnectionString"] ?? "localhost:6379";
bool hasRedis = false;

try
{
    var multiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
    builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);
    hasRedis = true;
    Console.WriteLine("Redis connected successfully.");
}
catch (Exception ex)
{
    Console.WriteLine("Redis connection failed: " + ex.Message);
    Console.WriteLine("Falling back to direct DB repository.");
}

// === Repository wiring — disconnected architecture ===
if (hasDatabase && hasRedis)
{
    builder.Services.AddScoped<IQuantityOperationRepository, QuantityOperationRedisRepository>();
    Console.WriteLine("Using Redis-backed repository with SQL Server persistence.");
}
else if (hasDatabase)
{
    builder.Services.AddScoped<IQuantityOperationRepository, QuantityOperationRepository>();
    Console.WriteLine("Using direct SQL Server repository (no Redis).");
}

// === Business Service ===
builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementService>();

// === HTTP Client for History Service ===
builder.Services.AddHttpClient("HistoryService", client =>
{
    client.BaseAddress = new Uri("http://localhost:5003");
});

// === JWT Authentication ===
var jwtSection = configuration.GetSection("Jwt");
string jwtKey = jwtSection["Key"]!;
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ClockSkew = TimeSpan.Zero,
            RoleClaimType = System.Security.Claims.ClaimTypes.Role
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddCors();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quantity Service v1");
});

app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// === Migrate DB on startup ===
if (hasDatabase)
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<QuantityDbContext>();
        db.Database.Migrate();
    }
}

app.Run();