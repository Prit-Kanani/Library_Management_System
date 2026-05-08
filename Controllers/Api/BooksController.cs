using FluentValidation;
using Library_Management_System.DTOs.Books;
using Library_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController(
        IBookService books,
        IValidator<BookCreateDto> createValidator,
        IValidator<BookUpdateDto> updateValidator) : ControllerBase
    {
        #region Query

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] BookFilterDto filter)
        {
            return Ok(await books.GetAllAsync(filter));
        }

        [HttpGet("{id:int}/view")]
        public async Task<IActionResult> GetByIdForView(int id)
        {
            var book = await books.GetByIdAsync(id);
            return book is null ? NotFound() : Ok(book);
        }

        [HttpGet("{id:int}/update")]
        public async Task<IActionResult> GetByIdForUpdate(int id)
        {
            var book = await books.GetByIdForUpdateAsync(id);
            return book is null ? NotFound() : Ok(book);
        }

        [HttpGet("dropdown")]
        public async Task<IActionResult> Dropdown()
        {
            return Ok(await books.GetDropdownAsync());
        }

        #endregion

        #region Commands

        [HttpPost]
        public async Task<IActionResult> Create(BookCreateDto dto)
        {
            await createValidator.ValidateAndThrowAsync(dto);
            var book = await books.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdForView), new { id = book.BookId }, book);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, BookUpdateDto dto)
        {
            await updateValidator.ValidateAndThrowAsync(dto);
            return await books.UpdateAsync(id, dto) ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await books.DeleteAsync(id) ? NoContent() : NotFound();
        }

        #endregion
    }
}
