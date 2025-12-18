using DotNetEnv;
using System.Text.Json;
using LearnASP.Domain.Entities;
using LearnASP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Load .env
if (builder.Environment.IsDevelopment())
{
    Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");
    Env.Load();
}

// In Memory Database
//builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("ObscuraDB"));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Reading Environment Variables
// builder.Configuration.AddEnvironmentVariables();

// Read .env
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

// Validate .env
void EnsureEnv(string? value, string name)
{
    if (string.IsNullOrWhiteSpace(value))
        throw new InvalidOperationException($"Environment variable {name} is not set.");
}

EnsureEnv(dbHost, "DB_HOST");
EnsureEnv(dbPort, "DB_PORT");
EnsureEnv(dbName, "DB_NAME");
EnsureEnv(dbUser, "DB_USER");
EnsureEnv(dbPassword, "DB_PASSWORD");

// Get String Connection
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString = $"Server={dbHost},{dbPort};Database={dbName};User Id={dbUser};Password={dbPassword};TrustServerCertificate=True";

// Register DbContext
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseSqlServer(connectionString));
// Best Practice based on gpt
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString, sql => {
    sql.EnableRetryOnFailure(
        maxRetryCount: 5,
        maxRetryDelay: TimeSpan.FromSeconds(10),
        errorNumbersToAdd: null
    );
    sql.MigrationsAssembly("LearnASP.Infrastructure");
}));

// Register Controllers Layer
builder.Services.AddControllers();

// Register AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Database HealtCheck
builder.Services.AddHealthChecks()
    .AddSqlServer(
        connectionString,
        name: "sqlserver",
        timeout: TimeSpan.FromSeconds(5),
        tags: new[] { "db", "sql" }
    );

// Middeleware Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "ObscuraAPI";
    config.Title = "ObscuraAPI v1";
    config.Version = "v1";
});

var app = builder.Build();

// Validate DB connection at startup
// Remove

// Enable Swagger Middleware in Development
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "ObscuraAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

// Controllers Routing Middleware
app.MapControllers();

// API MapGroups Configuration
var books = app.MapGroup("/books");

// Database Health Check Endpoint
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var result = new
        {
            status = report.Status.ToString(),
            database = dbName,
            checks = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                description = entry.Value.Description,
                error = entry.Value.Exception?.Message
            })
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(result));
    }
});

// Run the application
app.Run();