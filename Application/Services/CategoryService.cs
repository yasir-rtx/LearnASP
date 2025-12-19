using AutoMapper;
using LearnASP.Application.DTOs.Categories;
using LearnASP.Application.Interfaces;
using LearnASP.Domain.Entities;
using LearnASP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LearnASP.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public CategoryService(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken token)
        {
            var categories = await _db.Categories
                .AsNoTracking()
                .ToListAsync(token);
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetByIdAsync(int id, CancellationToken token)
        {
            var category = await _db.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(category => category.Id == id, token);
            return category is null ? null : _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryRequest request, CancellationToken token)
        {
            var category = _mapper.Map<Category>(request);

            category.CreatedAt = DateTime.UtcNow;
            category.CreatedBy = 1;

            _db.Categories.Add(category);
            await _db.SaveChangesAsync(token);
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryRequest request, CancellationToken token)
        {
            var category = await _db.Categories
                .FirstOrDefaultAsync(category => category.Id == id, token);

            if (category is null) return null;

            _mapper.Map(request, category);
            category.UpdatedAt = DateTime.UtcNow;
            category.UpdatedBy = 1;

            await _db.SaveChangesAsync(token);
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken token)
        {
            var category = await _db.Categories
                .FirstOrDefaultAsync(category => category.Id == id, token);
            
            if (category is null) return false;
            
            _db.Remove(category);
            await _db.SaveChangesAsync(token);
            return true;
        }
    }
}