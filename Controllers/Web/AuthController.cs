using FluentValidation;
using Library_Management_System.DTOs.Auth;
using Library_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers.Web
{
    [AllowAnonymous]
    public class AuthController(
        IAuthService authService,
        IValidator<LoginDto> loginValidator,
        IValidator<RegisterDto> registerValidator) : Controller
    {
        #region Views

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginDto());
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterDto());
        }

        #endregion

        #region Commands

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var validationResult = await loginValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                AddValidationErrors(validationResult);
                return View(dto);
            }

            var authResponse = await authService.LoginAsync(dto);

            if (authResponse is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(dto);
            }

            StoreToken(authResponse);
            return View("AuthResult", authResponse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var validationResult = await registerValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                AddValidationErrors(validationResult);
                return View(dto);
            }

            try
            {
                var authResponse = await authService.RegisterAsync(dto);
                StoreToken(authResponse);
                return View("AuthResult", authResponse);
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(dto);
            }
        }

        #endregion

        #region Helpers

        private void StoreToken(AuthResponseDto authResponse)
        {
            Response.Cookies.Append("LibraryJwt", authResponse.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Strict,
                Expires = authResponse.ExpiresAt
            });
        }

        private void AddValidationErrors(FluentValidation.Results.ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }

        #endregion
    }
}
