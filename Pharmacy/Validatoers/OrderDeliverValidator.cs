using FluentValidation;
using Pharmasy.Models.Dto.Request;

namespace Pharmasy.Validatoers;

public class OrderDeliverValidator:AbstractValidator<OrderDeliverRequest>
{
    public OrderDeliverValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("Customer id must be greater than 0");
    }
}