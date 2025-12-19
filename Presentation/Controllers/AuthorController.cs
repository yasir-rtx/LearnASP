using AutoMapper;
using LearnASP.Application.DTOs.Authors;
using LearnASP.Application.Interfaces;
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
        // Dependency Injection]
        private readonly IAuthorService _authorService;

        // Constructor
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        /// <summary> Get All Authors </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAllAuthors(CancellationToken cancellationToken)
        {
            var authors = await _authorService.GetAllAsync(cancellationToken);
            return Ok(authors);
        }

        /// <summary> Get An Author By Id </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetAuthorById(int id, CancellationToken cancellationToken)
        {
            var author = await _authorService.GetByIdAsync(id, cancellationToken);
            return author is null ? NotFound() : Ok(author);
        }

        /// <summary> Create A New Author </summary>
        [HttpPost]
        public async Task<ActionResult<CreateAuthorRequest>> CreateAuthor([FromBody] CreateAuthorRequest request, CancellationToken cancellationToken)
        {
            var author = await _authorService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, author);
        }

        /// <summary> Update An Author </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAuthor(int id, [FromBody] UpdateAuthorRequest request, CancellationToken cancellationToken)
        {
            var updatedDto = await _authorService.UpdateAsync(id, request, cancellationToken);
            if (updatedDto is null) return NotFound();

            return Ok(new
            {
                Message = "Author updated successfully",
                data = updatedDto
            });
        }

        /// <summary> Delete An Author </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuthor(int id, CancellationToken cancellationToken)
        {
            var deletedDto = await _authorService.DeleteAsync(id, cancellationToken);
            if (!deletedDto) return NotFound();

            return Ok(new
            {
                Message = $"Author with ID:{id} deleted successfully"
            });
        }
    }
}
