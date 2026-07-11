using FluentValidation;
using Pharmasy.Models.Dto.Request;

namespace Pharmasy.Validators;

public class OrderResevationValidator : AbstractValidator<OrderReservationRequest>
{
    public OrderResevationValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("Customer id must be greater than 0hjhjhj");
    }
}