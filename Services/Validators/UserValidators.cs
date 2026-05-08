using FluentValidation;
using Library_Management_System.DTOs.Users;

namespace Library_Management_System.Services.Validators
{
    public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
    {
        public UserCreateDtoValidator()
        {
            RuleFor(user => user.FullName).NotEmpty().MaximumLength(100);
            RuleFor(user => user.Email).NotEmpty().EmailAddress().MaximumLength(100);
            RuleFor(user => user.Password).NotEmpty().MaximumLength(100);
            RuleFor(user => user.Role).NotEmpty().Must(BeValidRole).WithMessage("Role must be Admin, Librarian, or User.");
        }

        private static bool BeValidRole(string role)
        {
            return role is "Admin" or "Librarian" or "User";
        }
    }

    public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateDtoValidator()
        {
            RuleFor(user => user.FullName).NotEmpty().MaximumLength(100);
            RuleFor(user => user.Email).NotEmpty().EmailAddress().MaximumLength(100);
            RuleFor(user => user.Password).NotEmpty().MaximumLength(100);
            RuleFor(user => user.Role).NotEmpty().Must(BeValidRole).WithMessage("Role must be Admin, Librarian, or User.");
        }

        private static bool BeValidRole(string role)
        {
            return role is "Admin" or "Librarian" or "User";
        }
    }
}
