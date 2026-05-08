using FluentValidation;
using Library_Management_System.DTOs.IssuedBooks;
using Library_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class IssuedBooksController(
        IIssuedBookService issuedBooks,
        IBookService books,
        IUserService users,
        IValidator<IssuedBookCreateDto> createValidator,
        IValidator<IssuedBookUpdateDto> updateValidator) : ControllerBase
    {
        #region Query

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] IssuedBookFilterDto filter)
        {
            return Ok(await issuedBooks.GetAllAsync(filter));
        }

        [HttpGet("{id:int}/view")]
        public async Task<IActionResult> GetByIdForView(int id)
        {
            var issuedBook = await issuedBooks.GetByIdAsync(id);
            return issuedBook is null ? NotFound() : Ok(issuedBook);
        }

        [HttpGet("{id:int}/update")]
        public async Task<IActionResult> GetByIdForUpdate(int id)
        {
            var issuedBook = await issuedBooks.GetByIdForUpdateAsync(id);
            return issuedBook is null ? NotFound() : Ok(issuedBook);
        }

        [HttpGet("dropdowns")]
        public async Task<IActionResult> Dropdowns()
        {
            return Ok(new
            {
                Books = await books.GetDropdownAsync(),
                Users = await users.GetDropdownAsync()
            });
        }

        #endregion

        #region Commands

        [HttpPost]
        public async Task<IActionResult> Create(IssuedBookCreateDto dto)
        {
            await createValidator.ValidateAndThrowAsync(dto);
            var issuedBook = await issuedBooks.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdForView), new { id = issuedBook.IssueId }, issuedBook);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, IssuedBookUpdateDto dto)
        {
            await updateValidator.ValidateAndThrowAsync(dto);
            return await issuedBooks.UpdateAsync(id, dto) ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await issuedBooks.DeleteAsync(id) ? NoContent() : NotFound();
        }

        #endregion
    }
}
