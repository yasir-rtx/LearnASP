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

books.MapGet("/", GetAllBooks);
books.MapGet("/{id}", GetBook);
books.MapPost("/", PostBook);
books.MapPut("/{id}", UpdateBook);
books.MapDelete("/{id}", DeleteBook);

// Run the application
app.Run();

// Get All Books
static async Task<IResult> GetAllBooks(AppDbContext db)
{
    return TypedResults.Ok(await db.Books.ToListAsync());
}

// Get A Book By Id
static async Task<IResult> GetBook(int id, AppDbContext db)
{
    var book = await db.Books.FindAsync(id);
    return book is null ? TypedResults.NotFound() : TypedResults.Ok(book);
}

// Post A New Book
static async Task<IResult> PostBook(Book book, AppDbContext db)
{
    db.Books.Add(book);
    await db.SaveChangesAsync();

    // Give response to client
    return TypedResults.Created($"/books/{book.Id}", book);
}

// Update an existing Book
static async Task<IResult> UpdateBook(int id, Book inputBook, AppDbContext db)
{
    var book = await db.Books.FindAsync(id);

    // if todo is not found
    if (book is null) return TypedResults.NotFound();

    book.Title = inputBook.Title;
    book.IsAvailable = inputBook.IsAvailable;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

// Delete a Book
static async Task<IResult> DeleteBook(int id, AppDbContext db)
{
    if (await db.Books.FindAsync(id) is Book book)
    {
        db.Books.Remove(book);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}