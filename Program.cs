using LearnASP.Domain.Entities;
using LearnASP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Get String Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// In Memory Database
//builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("ObscuraDB"));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Register Controllers Layer
builder.Services.AddControllers();

// Register AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        if (db.Database.CanConnect())
        {
            Console.WriteLine("Database connection successful.");
        }
        else
        {
            Console.WriteLine("Cannot connect to database.");
            throw new Exception("Database connection failed.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database connection error: {ex.Message}");
        throw; // stop app if DB is unreachable
    }
}

// Enable Swagger Middleware
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

// API MapGroups Configuration
var books = app.MapGroup("/books");

// Controllers Routing Middleware
app.MapControllers();

// Run the application
app.Run();