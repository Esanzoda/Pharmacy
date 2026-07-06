using FluentValidation;
using Pharmasy.Models.Dto.Request;

namespace Pharmasy.Validators;

public class OrderDeliverValidator : AbstractValidator<OrderRequest>
{
    public OrderDeliverValidator()
    {
        RuleFor(x => x.CustomererId)
            .GreaterThan(0)
            .WithMessage("Customer id must be greater than 0");
    }
}