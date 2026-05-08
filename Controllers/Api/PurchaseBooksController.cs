using FluentValidation;
using Library_Management_System.DTOs.PurchaseBooks;
using Library_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseBooksController(
        IPurchaseBookService purchaseBooks,
        IBookService books,
        IValidator<PurchaseBookCreateDto> createValidator,
        IValidator<PurchaseBookUpdateDto> updateValidator) : ControllerBase
    {
        #region Query

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PurchaseBookFilterDto filter)
        {
            return Ok(await purchaseBooks.GetAllAsync(filter));
        }

        [HttpGet("{id:int}/view")]
        public async Task<IActionResult> GetByIdForView(int id)
        {
            var purchaseBook = await purchaseBooks.GetByIdAsync(id);
            return purchaseBook is null ? NotFound() : Ok(purchaseBook);
        }

        [HttpGet("{id:int}/update")]
        public async Task<IActionResult> GetByIdForUpdate(int id)
        {
            var purchaseBook = await purchaseBooks.GetByIdForUpdateAsync(id);
            return purchaseBook is null ? NotFound() : Ok(purchaseBook);
        }

        [HttpGet("dropdowns")]
        public async Task<IActionResult> Dropdowns()
        {
            return Ok(new
            {
                Books = await books.GetDropdownAsync()
            });
        }

        #endregion

        #region Commands

        [HttpPost]
        public async Task<IActionResult> Create(PurchaseBookCreateDto dto)
        {
            await createValidator.ValidateAndThrowAsync(dto);
            var purchaseBook = await purchaseBooks.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdForView), new { id = purchaseBook.PurchaseId }, purchaseBook);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, PurchaseBookUpdateDto dto)
        {
            await updateValidator.ValidateAndThrowAsync(dto);
            return await purchaseBooks.UpdateAsync(id, dto) ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await purchaseBooks.DeleteAsync(id) ? NoContent() : NotFound();
        }

        #endregion
    }
}
