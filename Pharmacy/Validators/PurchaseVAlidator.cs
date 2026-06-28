using FluentValidation;
using Pharmasy.Models.Dto.Request;

namespace Pharmasy.Validators;

public class  PurchaseVAlidator:AbstractValidator<PurchaseRequest>
{
    public PurchaseVAlidator()
    {
        RuleFor(x=>x.EmployeId)
            .GreaterThan(0) 
            .WithMessage("EmployeId must be greater than 0");
        RuleForEach(x=>x.PurchaseItemRequests)
            .SetValidator(new PurchaseItemValidator());
    }
    
}