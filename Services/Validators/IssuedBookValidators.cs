using FluentValidation;
using Library_Management_System.DTOs.IssuedBooks;

namespace Library_Management_System.Services.Validators
{
    public class IssuedBookCreateDtoValidator : AbstractValidator<IssuedBookCreateDto>
    {
        public IssuedBookCreateDtoValidator()
        {
            RuleFor(issuedBook => issuedBook.BookId).GreaterThan(0);
            RuleFor(issuedBook => issuedBook.UserId).GreaterThan(0);
            RuleFor(issuedBook => issuedBook.IssuePurpose).MaximumLength(200);
            RuleFor(issuedBook => issuedBook.CurrentPenaltyAmount).GreaterThanOrEqualTo(0);
            RuleFor(issuedBook => issuedBook.Status).NotEmpty().Must(BeValidStatus).WithMessage("Status must be Issued, Returned, or Penalized.");
            RuleFor(issuedBook => issuedBook.ReturnDate)
                .GreaterThanOrEqualTo(issuedBook => issuedBook.IssueDate)
                .WithMessage("Return date cannot be before issue date.");
        }

        private static bool BeValidStatus(string status)
        {
            return status is "Issued" or "Returned" or "Penalized";
        }
    }

    public class IssuedBookUpdateDtoValidator : AbstractValidator<IssuedBookUpdateDto>
    {
        public IssuedBookUpdateDtoValidator()
        {
            RuleFor(issuedBook => issuedBook.BookId).GreaterThan(0);
            RuleFor(issuedBook => issuedBook.UserId).GreaterThan(0);
            RuleFor(issuedBook => issuedBook.IssuePurpose).MaximumLength(200);
            RuleFor(issuedBook => issuedBook.CurrentPenaltyAmount).GreaterThanOrEqualTo(0);
            RuleFor(issuedBook => issuedBook.Status).NotEmpty().Must(BeValidStatus).WithMessage("Status must be Issued, Returned, or Penalized.");
            RuleFor(issuedBook => issuedBook.ReturnDate)
                .GreaterThanOrEqualTo(issuedBook => issuedBook.IssueDate)
                .WithMessage("Return date cannot be before issue date.");
        }

        private static bool BeValidStatus(string status)
        {
            return status is "Issued" or "Returned" or "Penalized";
        }
    }
}
