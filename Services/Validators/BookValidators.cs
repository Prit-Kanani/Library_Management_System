using FluentValidation;
using Library_Management_System.DTOs.Books;

namespace Library_Management_System.Services.Validators
{
    public class BookCreateDtoValidator : AbstractValidator<BookCreateDto>
    {
        public BookCreateDtoValidator()
        {
            RuleFor(book => book.Title).NotEmpty().MaximumLength(200);
            RuleFor(book => book.Description).MaximumLength(500);
            RuleFor(book => book.Author).NotEmpty().MaximumLength(100);
            RuleFor(book => book.Price).GreaterThanOrEqualTo(0);
            RuleFor(book => book.CategoryId).GreaterThan(0);
            RuleFor(book => book.TotalQuantity).GreaterThanOrEqualTo(0);
            RuleFor(book => book.PenaltyAmount).GreaterThanOrEqualTo(0);
            RuleFor(book => book.AvailableQuantity).GreaterThanOrEqualTo(0);
            RuleFor(book => book.AvailableQuantity)
                .LessThanOrEqualTo(book => book.TotalQuantity)
                .WithMessage("Available quantity cannot be greater than total quantity.");
        }
    }

    public class BookUpdateDtoValidator : AbstractValidator<BookUpdateDto>
    {
        public BookUpdateDtoValidator()
        {
            RuleFor(book => book.Title).NotEmpty().MaximumLength(200);
            RuleFor(book => book.Description).MaximumLength(500);
            RuleFor(book => book.Author).NotEmpty().MaximumLength(100);
            RuleFor(book => book.Price).GreaterThanOrEqualTo(0);
            RuleFor(book => book.CategoryId).GreaterThan(0);
            RuleFor(book => book.TotalQuantity).GreaterThanOrEqualTo(0);
            RuleFor(book => book.PenaltyAmount).GreaterThanOrEqualTo(0);
            RuleFor(book => book.AvailableQuantity).GreaterThanOrEqualTo(0);
            RuleFor(book => book.AvailableQuantity)
                .LessThanOrEqualTo(book => book.TotalQuantity)
                .WithMessage("Available quantity cannot be greater than total quantity.");
        }
    }
}
