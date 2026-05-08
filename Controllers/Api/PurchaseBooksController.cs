using FluentValidation;
using Library_Management_System.Common.Exceptions;
using Library_Management_System.DTOs.PurchaseBooks;
using Library_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers.Api
{
    [ApiController]
    [Authorize(Policy = "AdminOrLibrarian")]
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
            return purchaseBook is null
                ? throw new NotFoundException("Purchase book record does not exist!")
                : Ok(purchaseBook);
        }

        [HttpGet("{id:int}/update")]
        public async Task<IActionResult> GetByIdForUpdate(int id)
        {
            var purchaseBook = await purchaseBooks.GetByIdForUpdateAsync(id);
            return purchaseBook is null
                ? throw new NotFoundException("Purchase book record does not exist!")
                : Ok(purchaseBook);
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
            if (!await purchaseBooks.UpdateAsync(id, dto))
            {
                throw new NotFoundException("Purchase book record does not exist!");
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await purchaseBooks.DeleteAsync(id))
            {
                throw new NotFoundException("Purchase book record does not exist!");
            }

            return NoContent();
        }

        #endregion
    }
}
