using LearnASP.Application.DTOs.Categories;

namespace LearnASP.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken token);
        Task<CategoryDto?> GetByIdAsync(int id, CancellationToken token);
        Task<CategoryDto> CreateAsync(CreateCategoryRequest request, CancellationToken token);
        Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryRequest request, CancellationToken token);
        Task<bool> DeleteAsync(int id, CancellationToken token);
    }
}