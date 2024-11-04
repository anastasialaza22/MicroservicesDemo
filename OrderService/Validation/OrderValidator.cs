using FluentValidation;
using OrderService.DTOs;

namespace OrderService.Validation
{
    public class OrderValidator : AbstractValidator<OrderCreateDto>
    {
        public OrderValidator()
        {
            RuleFor(o => o.ProductId)
                .GreaterThan(0).WithMessage("Идентификатор продукта должен быть положительным.");

            RuleFor(o => o.Quantity)
                .GreaterThan(0).WithMessage("Количество должно быть положительным.");
        }
    }
}