using FluentValidation;
using ProductService.DTOs;

namespace ProductService.Validation
{
    public class ProductValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Имя обязательно.");
            
            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Цена должна быть положительной.");
            
            RuleFor(p => p.Category)
                .NotEmpty().WithMessage("Категория обязательна.");
        }
    }
}