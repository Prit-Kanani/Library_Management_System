using FluentValidation;
using Library_Management_System.DTOs.Categories;
using Library_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController(
        ICategoryService categories,
        IValidator<CategoryCreateDto> createValidator,
        IValidator<CategoryUpdateDto> updateValidator) : ControllerBase
    {
        #region Query

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CategoryFilterDto filter)
        {
            return Ok(await categories.GetAllAsync(filter));
        }

        [HttpGet("{id:int}/view")]
        public async Task<IActionResult> GetByIdForView(int id)
        {
            var category = await categories.GetByIdAsync(id);
            return category is null ? NotFound() : Ok(category);
        }

        [HttpGet("{id:int}/update")]
        public async Task<IActionResult> GetByIdForUpdate(int id)
        {
            var category = await categories.GetByIdForUpdateAsync(id);
            return category is null ? NotFound() : Ok(category);
        }

        [HttpGet("dropdown")]
        public async Task<IActionResult> Dropdown()
        {
            return Ok(await categories.GetDropdownAsync());
        }

        #endregion

        #region Commands

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDto dto)
        {
            await createValidator.ValidateAndThrowAsync(dto);
            var category = await categories.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdForView), new { id = category.CategoryId }, category);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, CategoryUpdateDto dto)
        {
            await updateValidator.ValidateAndThrowAsync(dto);
            return await categories.UpdateAsync(id, dto) ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await categories.DeleteAsync(id) ? NoContent() : NotFound();
        }

        #endregion
    }
}
