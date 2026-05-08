using FluentValidation;
using Library_Management_System.DTOs.Categories;

namespace Library_Management_System.Services.Validators
{
    public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateDtoValidator()
        {
            RuleFor(category => category.CategoryName).NotEmpty().MaximumLength(100);
        }
    }

    public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
    {
        public CategoryUpdateDtoValidator()
        {
            RuleFor(category => category.CategoryName).NotEmpty().MaximumLength(100);
        }
    }
}
