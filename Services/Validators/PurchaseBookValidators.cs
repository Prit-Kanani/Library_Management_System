using FluentValidation;
using Library_Management_System.DTOs.PurchaseBooks;

namespace Library_Management_System.Services.Validators
{
    public class PurchaseBookCreateDtoValidator : AbstractValidator<PurchaseBookCreateDto>
    {
        public PurchaseBookCreateDtoValidator()
        {
            RuleFor(purchaseBook => purchaseBook.BookId).GreaterThan(0);
            RuleFor(purchaseBook => purchaseBook.PurchaseAmountPerUnit).GreaterThanOrEqualTo(0);
            RuleFor(purchaseBook => purchaseBook.PurchaseQuantity).GreaterThan(0);
            RuleFor(purchaseBook => purchaseBook.PurchaseFrom).NotEmpty().MaximumLength(100);
            RuleFor(purchaseBook => purchaseBook.PaymentMethod).NotEmpty().Must(BeValidPaymentMethod).WithMessage("Payment method must be Cash, Card, or Online.");
            RuleFor(purchaseBook => purchaseBook.PaymentStatus).NotEmpty().Must(BeValidPaymentStatus).WithMessage("Payment status must be Paid or Unpaid.");
        }

        private static bool BeValidPaymentMethod(string paymentMethod)
        {
            return paymentMethod is "Cash" or "Card" or "Online";
        }

        private static bool BeValidPaymentStatus(string paymentStatus)
        {
            return paymentStatus is "Paid" or "Unpaid";
        }
    }

    public class PurchaseBookUpdateDtoValidator : AbstractValidator<PurchaseBookUpdateDto>
    {
        public PurchaseBookUpdateDtoValidator()
        {
            RuleFor(purchaseBook => purchaseBook.BookId).GreaterThan(0);
            RuleFor(purchaseBook => purchaseBook.PurchaseAmountPerUnit).GreaterThanOrEqualTo(0);
            RuleFor(purchaseBook => purchaseBook.PurchaseQuantity).GreaterThan(0);
            RuleFor(purchaseBook => purchaseBook.PurchaseFrom).NotEmpty().MaximumLength(100);
            RuleFor(purchaseBook => purchaseBook.PaymentMethod).NotEmpty().Must(BeValidPaymentMethod).WithMessage("Payment method must be Cash, Card, or Online.");
            RuleFor(purchaseBook => purchaseBook.PaymentStatus).NotEmpty().Must(BeValidPaymentStatus).WithMessage("Payment status must be Paid or Unpaid.");
        }

        private static bool BeValidPaymentMethod(string paymentMethod)
        {
            return paymentMethod is "Cash" or "Card" or "Online";
        }

        private static bool BeValidPaymentStatus(string paymentStatus)
        {
            return paymentStatus is "Paid" or "Unpaid";
        }
    }
}
