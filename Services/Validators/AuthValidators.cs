using FluentValidation;
using Library_Management_System.DTOs.Auth;

namespace Library_Management_System.Services.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(login => login.Email).NotEmpty().EmailAddress().MaximumLength(100);
            RuleFor(login => login.Password).NotEmpty().MaximumLength(100);
        }
    }

    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(register => register.FullName).NotEmpty().MaximumLength(100);
            RuleFor(register => register.Email).NotEmpty().EmailAddress().MaximumLength(100);
            RuleFor(register => register.Password).NotEmpty().MinimumLength(6).MaximumLength(100);
            RuleFor(register => register.ConfirmPassword)
                .Equal(register => register.Password)
                .WithMessage("Confirm password must match password.");
            RuleFor(register => register.Role)
                .NotEmpty()
                .Must(BeValidRole)
                .WithMessage("Role must be Admin, Librarian, or User.");
        }

        private static bool BeValidRole(string role)
        {
            return role is "Admin" or "Librarian" or "User";
        }
    }
}
