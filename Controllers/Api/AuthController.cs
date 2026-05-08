using FluentValidation;
using Library_Management_System.Common.Exceptions;
using Library_Management_System.DTOs.Auth;
using Library_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers.Api
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthController(
        IAuthService authService,
        IValidator<LoginDto> loginValidator,
        IValidator<RegisterDto> registerValidator) : ControllerBase
    {
        #region Commands

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            await loginValidator.ValidateAndThrowAsync(dto);

            var authResponse = await authService.LoginAsync(dto);

            if (authResponse is null)
            {
                throw new UnauthorizedException("Invalid email or password.");
            }

            return Ok(authResponse);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            await registerValidator.ValidateAndThrowAsync(dto);
            return Ok(await authService.RegisterAsync(dto));
        }

        #endregion
    }
}
