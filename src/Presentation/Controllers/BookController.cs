using LearnASP.Application.Common.Responses;
using LearnASP.Application.DTOs.Books;
using LearnASP.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LearnASP.Presentation.Controllers
{
    [ApiController]
    // [Route("api/[controller]")] // Not Best Practice
    [Route("api/books")]
    [Produces("application/json")]
    public class BookController : ControllerBase
    {
        // Dependency Injection
        private readonly IBookService _bookService;

        // Constructor
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary> Get All Books </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<BookDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBooks(CancellationToken token)
        {
            var books = await _bookService.GetAllAsync(token);
            return Ok(ApiResponse<IEnumerable<BookDto>>.SuccessResponse(books, "Books retrieved successfully"));
        }

        /// <summary> Get A Book By Id </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<BookDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookById(int id, CancellationToken token)
        {
            var book = await _bookService.GetByIdAsync(id, token);
            return Ok(ApiResponse<BookDto>.SuccessResponse(book, "Book retrieved successfully"));
        }

        /// <summary> Post A New Book </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<BookDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookRequest request, CancellationToken token)
        {
            var book = await _bookService.CreateAsync(request, token);
            return CreatedAtAction(
                 nameof(GetBookById), new { id = book.Id }, 
                 ApiResponse<BookDto>.SuccessResponse(book, "Book created successfully"));
        }

        /// <summary> Update an Existing Book </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<BookDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookRequest request, CancellationToken token)
        {
            var updated = await _bookService.UpdateAsync(id, request, token);
            return Ok(ApiResponse<BookDto>.SuccessResponse(updated, "Book updated successfully"));
        }

        /// <summary> Delete a Book </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBook(int id, CancellationToken token)
        {
            var deleted = await _bookService.DeleteAsync(id, token);
            return Ok(ApiResponse<object>.SuccessResponse(null, "Book deleted successfully"));
        }

        /// <summary> Delete all Books </summary>
        [HttpDelete("all-delete")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAllBook(CancellationToken token)
        {
            await _bookService.DeleteAllAsync(token);
            return Ok(ApiResponse<object>.SuccessResponse(null, "All Books deleted successfully"));
        }
    }
}