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
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var categories = await _categoryService.GetAllAsync(token);
            return categories is null ? NotFound() : Ok(categories);
        }

        /// <summary> Get An Category By Id </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken token)
        {
            var category = await _categoryService.GetByIdAsync(id, token);
            return category is null ? NotFound() : Ok(category);
        }

        /// <summary> Create A New Category </summary>
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryRequest request, CancellationToken token)
        {
            var category = await _categoryService.CreateAsync(request, token);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        /// <summary> Update An Category </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCategoryRequest request, CancellationToken token)
        {
            var category = await _categoryService.UpdateAsync(id, request, token);
            return category is null ? NotFound() : Ok(category);
        }

        /// <summary> Delete A Category </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken token)
        {
            var deleted = await _categoryService.DeleteAsync(id, token);
            return deleted ? Ok() : NotFound();
        }
    }
}