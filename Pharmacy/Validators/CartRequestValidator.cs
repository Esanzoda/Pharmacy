using FluentValidation;
using Pharmacy.Models.Dto.Request;

namespace Pharmacy.Validators;

public class CartRequestValidator : AbstractValidator<CartRequest>
{
    public CartRequestValidator()
    {
        RuleForEach(x => x.CartItems)
            .SetValidator(new CartItemRequestValidator());
    }
}