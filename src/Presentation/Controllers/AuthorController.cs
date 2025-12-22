using LearnASP.Application.Common.Responses;
using LearnASP.Application.DTOs.Authors;
using LearnASP.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LearnASP.Presentation.Controllers
{
    [ApiController]
    // [Route("api/[controller]")] // Not Best Practice
    [Route("api/authors")]
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
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<AuthorDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAuthors(CancellationToken token)
        {
            var authors = await _authorService.GetAllAsync(token);
            return Ok(ApiResponse<IEnumerable<AuthorDto>>.SuccessResponse(authors, "Authors retrieved successfully"));
        }

        /// <summary> Get An Author By Id </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<AuthorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAuthorById(int id, CancellationToken token)
        {
            var author = await _authorService.GetByIdAsync(id, token);
            return author is null 
                ? NotFound(ApiResponse<object>.ErrorResponse("Author not found"))
                : Ok(ApiResponse<AuthorDto>.SuccessResponse(author, "Author retrieved successfully"));
        }

        /// <summary> Create A New Author </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<AuthorDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorRequest request, CancellationToken token)
        {
            var author = await _authorService.CreateAsync(request, token);
            return CreatedAtAction(
                 nameof(GetAuthorById), new { id = author.Id }, 
                 ApiResponse<AuthorDto>.SuccessResponse(author, "Author created successfully"));
        }

        /// <summary> Update An Author </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<AuthorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] UpdateAuthorRequest request, CancellationToken token)
        {
            var updated = await _authorService.UpdateAsync(id, request, token);
            return updated is null
                ? NotFound(ApiResponse<object>.ErrorResponse("Author not found"))
                : Ok(ApiResponse<AuthorDto>.SuccessResponse(updated, "Author updated successfully"));
        }

        /// <summary> Delete An Author </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAuthor(int id, CancellationToken token)
        {
            var deleted = await _authorService.DeleteAsync(id, token);
            return !deleted
                ? NotFound(ApiResponse<object>.ErrorResponse("Author not found"))
                : Ok(ApiResponse<object?>.SuccessResponse(null, "Author deleted successfully"));
        }
    }
}
