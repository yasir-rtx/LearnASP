using LearnASP.Application.Common.Responses;
using LearnASP.Application.DTOs.Authors;
using LearnASP.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnASP.Presentation.Controllers
{
    [ApiController]
    [Route("api/authors")]
    [Produces("application/json")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        /// <summary> Get all authors </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<AuthorDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAuthors(CancellationToken token)
        {
            var authors = await _authorService.GetAllAsync(token);

            return Ok(ApiResponse<IEnumerable<AuthorDto>>
                .SuccessResponse(authors, "Authors retrieved successfully"));
        }

        /// <summary> Get an author by id </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<AuthorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAuthorById(int id, CancellationToken token)
        {
            var author = await _authorService.GetByIdAsync(id, token);

            return Ok(ApiResponse<AuthorDto>
                .SuccessResponse(author, "Author retrieved successfully"));
        }

        /// <summary> Create a new author </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<AuthorDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateAuthor(
            [FromBody] CreateAuthorRequest request,
            CancellationToken token)
        {
            var author = await _authorService.CreateAsync(request, token);

            return CreatedAtAction(
                nameof(GetAuthorById),
                new { id = author.Id },
                ApiResponse<AuthorDto>
                    .SuccessResponse(author, "Author created successfully"));
        }

        /// <summary> Update an author </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<AuthorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAuthor(
            int id,
            [FromBody] UpdateAuthorRequest request,
            CancellationToken token)
        {
            var updated = await _authorService.UpdateAsync(id, request, token);

            return Ok(ApiResponse<AuthorDto>
                .SuccessResponse(updated, "Author updated successfully"));
        }

        /// <summary> Delete an author </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAuthor(int id, CancellationToken token)
        {
            await _authorService.DeleteAsync(id, token);

            return Ok(ApiResponse<object>
                .SuccessResponse(null, "Author deleted successfully"));
        }
    }
}
