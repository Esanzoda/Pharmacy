using FluentValidation;
using Pharmasy.Models.Dto.Request;

namespace Pharmasy.Validators;

public class PurchaseVAlidator : AbstractValidator<PurchaseRequest>
{
    public PurchaseVAlidator()
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0)
            .WithMessage("EmployeId must be greater than 0");
        RuleForEach(x => x.PurchaseItems)
            .SetValidator(new PurchaseItemValidator());
    }
}