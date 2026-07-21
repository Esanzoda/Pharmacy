using FluentValidation;
using Pharmasy.Models.Dto.Request;


namespace Pharmasy.Validators;

public class CartItemRequestValidator : AbstractValidator<CartItemRequest>
{
    public CartItemRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotNull()
            .NotEmpty()
            .WithMessage("Product id is required")
            .GreaterThan(0)
            .WithMessage("Product id must be greater than 0");
        RuleFor(x => x.Quantity)
            .NotNull()
            .NotEmpty()
            .WithMessage("Quantity is required")
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");
    }
}