using FluentValidation;
using Pharmasy.Models.Dto.Request;

namespace Pharmasy.Validators;

public class OrderItemValidator : AbstractValidator<OrderItemRequest>
{
    public OrderItemValidator()
    {
        RuleFor(order => order.ProductId)
            .GreaterThan(0)
            .WithMessage("Product id must be greater than 0");
        RuleFor(order => order.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");
    }
}