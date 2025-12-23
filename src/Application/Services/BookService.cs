using AutoMapper;
using LearnASP.Application.Common.Exceptions;
using LearnASP.Application.DTOs.Books;
using LearnASP.Application.Interfaces;
using LearnASP.Domain.Entities;
using LearnASP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LearnASP.Application.Services
{
    public class BookService : IBookService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public BookService(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookDto>> GetAllAsync(CancellationToken token)
        {
            var books = await _db.Books
                .AsNoTracking()
                .ToListAsync(token);

            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<BookDto?> GetByIdAsync(int id, CancellationToken token)
        {
            var book = await _db.Books
                .AsNoTracking()
                .FirstOrDefaultAsync(book => book.Id == id, token);
            
            return book is null ? throw new NotFoundException("Book not found") : _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> CreateAsync(CreateBookRequest request, CancellationToken token)
        {
            var book = _mapper.Map<Book>(request);

            book.CreatedAt = DateTime.UtcNow;
            book.CreatedBy = 1; // TODO

            _db.Books.Add(book);
            await _db.SaveChangesAsync(token);

            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto?> UpdateAsync(int id, UpdateBookRequest request, CancellationToken token)
        {
            var exists = await _db.Books.AnyAsync(book => book.Title == request.Title, token);
            if (exists) throw new DomainException("Book already exists");

            var book = await _db.Books
                .FirstOrDefaultAsync(book => book.Id == id, token);

            if (book is null) throw new NotFoundException("Book not found");
            
            _mapper.Map(request, book);
            book.UpdatedAt = DateTime.UtcNow;
            book.UpdatedBy = 1;

            await _db.SaveChangesAsync(token);
            return _mapper.Map<BookDto>(book);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken token)
        {
            var book = await _db.Books
                .FirstOrDefaultAsync(book => book.Id == id, token);

            if (book is null) throw new NotFoundException("Book not found");

            _db.Books.Remove(book);
            await _db.SaveChangesAsync(token);

            return true;
        }

        public async Task DeleteAllAsync(CancellationToken token)
        {
            var books = await _db.Books.ToListAsync(token);

            if (!books.Any()) throw new NotFoundException("No books found to delete");

            _db.Books.RemoveRange(books);
            await _db.SaveChangesAsync(token);
        }
    }
}