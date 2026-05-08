using FluentValidation;
using Library_Management_System.Common.Exceptions;
using Library_Management_System.DTOs.Users;
using Library_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(
        IUserService users,
        IValidator<UserCreateDto> createValidator,
        IValidator<UserUpdateDto> updateValidator) : ControllerBase
    {
        #region Query

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] UserFilterDto filter)
        {
            return Ok(await users.GetAllAsync(filter));
        }

        [HttpGet("{id:int}/view")]
        public async Task<IActionResult> GetByIdForView(int id)
        {
            var user = await users.GetByIdAsync(id);
            return user is null
                ? throw new NotFoundException("User does not exist!")
                : Ok(user);
        }

        [HttpGet("{id:int}/update")]
        public async Task<IActionResult> GetByIdForUpdate(int id)
        {
            var user = await users.GetByIdForUpdateAsync(id);
            return user is null
                ? throw new NotFoundException("User does not exist!")
                : Ok(user);
        }

        [HttpGet("dropdown")]
        public async Task<IActionResult> Dropdown()
        {
            return Ok(await users.GetDropdownAsync());
        }

        #endregion

        #region Commands

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateDto dto)
        {
            await createValidator.ValidateAndThrowAsync(dto);
            var user = await users.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdForView), new { id = user.UserId }, user);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UserUpdateDto dto)
        {
            await updateValidator.ValidateAndThrowAsync(dto);
            if (!await users.UpdateAsync(id, dto))
            {
                throw new NotFoundException("User does not exist!");
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await users.DeleteAsync(id))
            {
                throw new NotFoundException("User does not exist!");
            }

            return NoContent();
        }

        #endregion
    }
}
