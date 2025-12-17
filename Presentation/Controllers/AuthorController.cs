using AutoMapper;
using LearnASP.Application.DTOs.Authors;
using LearnASP.Domain.Entities;
using LearnASP.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnASP.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthorController : ControllerBase
    {
        // Dependency Injection
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        // Constructor
        public AuthorController(AppDbContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        /// <summary> Get All Authors </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAllAuthors()
        {
            var authors = await _db.Authors.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authors));
        }

        /// <summary> Get An Author By Id </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetAuthorById(int id)
        {
            var author = await _db.Authors.FindAsync(id);
            return author is null ? NotFound() : Ok(_mapper.Map<AuthorDto>(author));
        }

        /// <summary> Create A New Author </summary>
        [HttpPost]
        public async Task<ActionResult<CreateAuthorRequest>> CreateAuthor([FromBody] CreateAuthorRequest request, CancellationToken cancellationToken)
        {
            // konversi request dto ke domain entity
            var author = _mapper.Map<Author>(request);

            author.CreatedAt = DateTime.UtcNow;
            author.CreatedBy = 1; // TODO: ambil dari user yang sedang login

            _db.Authors.Add(author);
            await _db.SaveChangesAsync(cancellationToken);

            // konversi author entity ke dto untuk response (Id sudah terisi setelah SaveChanges)
            var authorDto = _mapper.Map<AuthorDto>(author);

            // 201 response
            return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, authorDto);
        }

        /// <summary> Update An Author </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAuthor(int id, [FromBody] UpdateAuthorRequest request, CancellationToken cancellationToken)
        {
            var author = await _db.Authors.FindAsync(id);
            if (author is null) return NotFound();

            // map fields dari request ke entity
            _mapper.Map(request, author);
            author.UpdatedAt = DateTime.UtcNow;
            author.UpdatedBy = 1; // TODO: ambil dari user yang sedang login
            
            _db.Authors.Update(author);
            await _db.SaveChangesAsync(cancellationToken);

            var updateDto = _mapper.Map<AuthorDto>(author);
            return Ok(new
            {
                Message = "Author updated successfully",
                data = updateDto
            });
        }

        /// <summary> Delete An Author </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuthor(int id, CancellationToken cancellationToken)
        {
            var author = await _db.Authors.FindAsync(id);
            if (author is null) return NotFound();
            _db.Authors.Remove(author);
            await _db.SaveChangesAsync(cancellationToken);
            return Ok(new
            {
                Message = $"Author with ID:{id} deleted successfully"
            });
        }
    }
}
