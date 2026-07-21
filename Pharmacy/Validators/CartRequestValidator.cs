using FluentValidation;
using Pharmasy.Models.Dto.Request;

namespace Pharmasy.Validators;

public class CartRequestValidator : AbstractValidator<CartRequest>
{
    public CartRequestValidator()
    {
        RuleForEach(x => x.CartItems)
            .SetValidator(new CartItemRequestValidator());
    }
}