using LearnASP.Application.Common.Responses;
using LearnASP.Application.DTOs.Categories;
using LearnASP.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LearnASP.Presentation.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [Produces("application/json")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary> Get All Categories </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CategoryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCategories(CancellationToken token)
        {
            var categories = await _categoryService.GetAllAsync(token);
            return Ok(ApiResponse<IEnumerable<CategoryDto>>.SuccessResponse(categories, "Categories retrieved successfully"));
        }

        /// <summary> Get A Category By Id </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<CategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryById(int id, CancellationToken token)
        {
            var category = await _categoryService.GetByIdAsync(id, token);
            return category is null 
                ? NotFound(ApiResponse<object>.ErrorResponse("Category not found"))
                : Ok(ApiResponse<CategoryDto>.SuccessResponse(category, "Category retrieved successfully"));
        }

        /// <summary> Create A New Category </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<CategoryDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request, CancellationToken token)
        {
            var category = await _categoryService.CreateAsync(request, token);
            return CreatedAtAction(
                 nameof(GetCategoryById), new { id = category.Id }, 
                 ApiResponse<CategoryDto>.SuccessResponse(category, "Category created successfully"));
        }

        /// <summary> Update A Category </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<CategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryRequest request, CancellationToken token)
        {
            var updated = await _categoryService.UpdateAsync(id, request, token);
            return updated is null
                ? NotFound(ApiResponse<object>.ErrorResponse("Category not found"))
                : Ok(ApiResponse<CategoryDto>.SuccessResponse(updated, "Category updated successfully"));
        }

        /// <summary> Delete A Category </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id, CancellationToken token)
        {
            var deleted = await _categoryService.DeleteAsync(id, token);
            return !deleted
                ? NotFound(ApiResponse<object>.ErrorResponse("Category not found"))
                : Ok(ApiResponse<object?>.SuccessResponse(null, "Category deleted successfully"));
        }
    }
}