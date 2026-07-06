using FluentValidation;
using Pharmasy.Models.Dto.Request;

namespace Pharmasy.Validators;

public class PurchaseItemValidator : AbstractValidator<PurchaseItemRequest>
{
    public PurchaseItemValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");
        /*RuleFor(x=>x.PurchaseId)
            .GreaterThan(0)
            .WithMessage("PurchaseId must be greater than 0");*/
        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("ProductId must be greater than 0");
        RuleFor(x => x.PurchasePrice)
            .NotNull()
            .WithMessage("Purchase Price must not be null")
            .GreaterThan(0)
            .WithMessage("Purchase Price must be greater than 0");
        RuleFor(x => x.Price)
            .NotNull()
            .WithMessage("Purchase Price must not be null")
            .GreaterThan(0)
            .WithMessage("PurchasePrice must be greater than 0");
        RuleFor(x => x.ExpiryDate)
            .NotNull()
            .WithMessage("ExpiryDate must not be null")
            //    .GreaterThan(DateTime.UtcNow)
            .WithMessage("Expiry date must be in the future");
    }
}