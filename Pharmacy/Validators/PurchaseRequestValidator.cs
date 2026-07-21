using FluentValidation;
using Pharmasy.Models.Dto.Request;

namespace Pharmasy.Validators;

public class PurchaseRequestValidator : AbstractValidator<PurchaseRequest>
{
    public PurchaseRequestValidator()
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0)
            .WithMessage("EmployeId must be greater than 0");
        RuleForEach(x => x.PurchaseItems)
            .SetValidator(new PurchaseItemRequestValidator());
    }
}