using LearnASP.Application.DTOs.Authors;

namespace LearnASP.Application.Interfaces{
    public interface IAuthorService {
        Task<IEnumerable<AuthorDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<AuthorDto> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<AuthorDto> CreateAsync(CreateAuthorRequest request, CancellationToken cancellationToken);
        Task<AuthorDto> UpdateAsync(int id, UpdateAuthorRequest request, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}