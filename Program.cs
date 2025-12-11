using Microsoft.EntityFrameworkCore;
using Obscura.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BookDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//Middeleware Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "ObscuraAPI";
    config.Title = "ObscuraAPI v1";
    config.Version = "v1";
});

var app = builder.Build();

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

// Get All Books
app.MapGet("/books", async (BookDb db) =>
    await db.Books.ToListAsync());

// Get A Book By Id
app.MapGet("/books/{id}", async (int id, BookDb db) =>
    await db.Books.FindAsync(id)
        is Book book
            ? Results.Ok(book)
            : Results.NotFound());

// Post A New Book
app.MapPost("/books", async (Book book, BookDb db) =>
{
    db.Books.Add(book);
    await db.SaveChangesAsync();

    // Give response to client
    return Results.Created($"/books/{book.Id}", book);
});


// Update an existing Book
app.MapPut("/books/{id}", async (int id, Book inputBook, BookDb db) =>
{
    var book = await db.Books.FindAsync(id);

    // if todo is not found
    if (book is null) return Results.NotFound();

    book.Title = inputBook.Title;
    book.IsAvailable = inputBook.IsAvailable;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

// Delete a Book
app.MapDelete("/books/{id}", async (int id, BookDb db) =>
{
    if (await db.Books.FindAsync(id) is Book todo)
    {
        db.Books.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

// Run the application
app.Run();