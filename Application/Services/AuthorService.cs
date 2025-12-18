using AutoMapper;
using LearnASP.Application.DTOs.Authors;
using LearnASP.Application.Interfaces;
using LearnASP.Domain.Entities;
using LearnASP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LearnASP.Application.Services.Authors
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

        public async Task<IEnumerable<AuthorDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var authors = await _db.Authors
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<AuthorDto>>(authors);
        }

        public async Task<AuthorDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var author = await _db.Authors
                .AsNoTracking()
                .FirstOrDefaultAsync(author => author.Id == id, cancellationToken);

            return author is null ? null : _mapper.Map<AuthorDto>(author);
        }

        public async Task<AuthorDto> CreateAsync(CreateAuthorRequest request, CancellationToken cancellationToken)
        {
            var author = _mapper.Map<Author>(request);

            author.CreatedAt = DateTime.UtcNow;
            author.CreatedBy = 1; // TODO: ganti IUserContext nanti

            _db.Authors.Add(author);
            await _db.SaveChangesAsync(cancellationToken);

            return _mapper.Map<AuthorDto>(author);
        }

        public async Task<AuthorDto?> UpdateAsync(int id, UpdateAuthorRequest request, CancellationToken cancellationToken)
        {
            var author = await _db.Authors
                .FirstOrDefaultAsync(author => author.Id == id, cancellationToken);

            if (author is null) return null;

            _mapper.Map(request, author);
            author.UpdatedAt = DateTime.UtcNow;
            author.UpdatedBy = 1; // TODO: ambil dari user yang sedang login

            await _db.SaveChangesAsync(cancellationToken);

            var updateDto = _mapper.Map<AuthorDto>(author);

            return _mapper.Map<AuthorDto>(author);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var author = await _db.Authors
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

            if (author is null) return false;

            _db.Authors.Remove(author);
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
