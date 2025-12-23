using AutoMapper;
using LearnASP.Application.Common.Exceptions;
using LearnASP.Application.DTOs.Authors;
using LearnASP.Application.Interfaces;
using LearnASP.Domain.Entities;
using LearnASP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LearnASP.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public AuthorService(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AuthorDto>> GetAllAsync(CancellationToken token)
        {
            var authors = await _db.Authors
                .AsNoTracking()
                .ToListAsync(token);

            return _mapper.Map<IEnumerable<AuthorDto>>(authors);
        }

        public async Task<AuthorDto> GetByIdAsync(int id, CancellationToken token)
        {
            var author = await _db.Authors
                .AsNoTracking()
                .FirstOrDefaultAsync(author => author.Id == id, token);

            return author is null ? throw new NotFoundException("Author not found") : _mapper.Map<AuthorDto>(author);
        }

        public async Task<AuthorDto> CreateAsync(CreateAuthorRequest request, CancellationToken token)
        {
            var exists = await _db.Authors.AnyAsync(author => author.FullName == request.FullName, token);
            if (exists) throw new DomainException("Author already exists");

            var author = _mapper.Map<Author>(request);

            author.CreatedAt = DateTime.UtcNow;
            author.CreatedBy = 1; // TODO: ganti IUserContext nanti

            _db.Authors.Add(author);
            await _db.SaveChangesAsync(token);

            return _mapper.Map<AuthorDto>(author);
        }

        public async Task<AuthorDto> UpdateAsync(int id, UpdateAuthorRequest request, CancellationToken token)
        {
            var author = await _db.Authors
                .FirstOrDefaultAsync(author => author.Id == id, token);

            if (author is null) throw new NotFoundException("Author not found");

            _mapper.Map(request, author);
            author.UpdatedAt = DateTime.UtcNow;
            author.UpdatedBy = 1; // TODO: ambil dari user yang sedang login

            await _db.SaveChangesAsync(token);
            return _mapper.Map<AuthorDto>(author);
        }

        public async Task DeleteAsync(int id, CancellationToken token)
        {
            var author = await _db.Authors
                .FirstOrDefaultAsync(author => author.Id == id, token);

            if (author is null) throw new NotFoundException("Author not found");

            _db.Authors.Remove(author);
            await _db.SaveChangesAsync(token);
        }

        public async Task DeleteAllAsync(CancellationToken token)
        {
            var authors = await _db.Authors.ToListAsync(token);

            if (!authors.Any()) throw new NotFoundException("No authors found to delete");

            _db.Authors.RemoveRange(authors);
            await _db.SaveChangesAsync(token);
        }
    }
}
