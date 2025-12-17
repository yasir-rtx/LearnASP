using AutoMapper;
using LearnASP.Application.DTOs.Authors;
using LearnASP.Application.DTOs.Books;
using LearnASP.Domain.Entities;
using LearnASP.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YamlDotNet.Core.Tokens;

namespace LearnASP.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class BookController : Controller
    {
        // Dependency Injection
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        // Constructor
        public BookController(AppDbContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        // Get All Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAllBooks()
        {
            var books = await _db.Books
                .Include(Book => Book.Author)
                .ToListAsync();
            return books is null ? NotFound() : Ok(_mapper.Map<IEnumerable<BookDto>>(books));
        }

        // Get A Book By Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            var book = await _db.Books.FindAsync(id);
            return book is null ? NotFound() : Ok(_mapper.Map<BookDto>(book));
        }

        // Post A New Book
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook([FromBody] CreateBookRequest request, CancellationToken cancellationToken)
        {
            var book = _mapper.Map<Book>(request);

            book.CreatedAt = DateTime.UtcNow;
            book.CreatedBy = 1; // TODO: taken from logged in user

            _db.Books.Add(book);
            await _db.SaveChangesAsync(cancellationToken);

            var bookDto = _mapper.Map<BookDto>(book);
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, bookDto);
        }

        // Update an Existing Book
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookRequest request, CancellationToken cancellationToken)
        {
            var book = await _db.Books.FindAsync(id);
            if (book is null) return NotFound();

            _mapper.Map(request, book);
            book.UpdatedAt = DateTime.UtcNow;
            book.UpdatedBy = 1; // TODO: taken from logged in user

            _db.Books.Update(book);
            await _db.SaveChangesAsync(cancellationToken);

            var updateDto = _mapper.Map<BookDto>(book);
            return Ok(new
            {
                Message = "Book updated successfully",
                data = updateDto
            });
        }

        // Delete a Book
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _db.Books.FindAsync(id);
            if (book is null) return NotFound();

            _db.Books.Remove(book);
            await _db.SaveChangesAsync();
            return Ok(new
            {
                Message = $"Author with ID:{id} deleted successfully"
            });
        }
    }
}