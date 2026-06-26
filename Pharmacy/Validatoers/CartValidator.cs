using FluentValidation;
using Pharmasy.Models.Dto.Request;

namespace Pharmasy.Validatoers;

public class CartValidator:AbstractValidator<CartRequest>
{
    public CartValidator()
    {
        RuleForEach(x=>x.CartItems)
            .SetValidator(new CartItemValidator());
    }
}