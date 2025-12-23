using LearnASP.Application.DTOs.Books;

namespace LearnASP.Application.Interfaces {
    public interface IBookService {
        Task<IEnumerable<BookDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<BookDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<BookDto> CreateAsync(CreateBookRequest request, CancellationToken cancellationToken);
        Task<BookDto?> UpdateAsync(int id, UpdateBookRequest request, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
        Task DeleteAllAsync(CancellationToken cancellationToken);
    }
}