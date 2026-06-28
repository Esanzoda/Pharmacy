using FluentValidation;
using Pharmasy.Models.Dto.Request;

namespace Pharmasy.Validatoers;

public class  SupplierValidator:AbstractValidator<DeliverRequest>
{
    public SupplierValidator()
    {
        RuleFor(x=>x.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Supplier name is required");
        RuleFor(x=>x.PhoneNumber)
            .NotNull()
            .NotEmpty()
            .WithMessage("Supplier phone number is required");
        RuleFor(x=>x.Address)
            .NotNull()
            .NotEmpty()
            .WithMessage("Supplier address is required");
        RuleFor(x=>x.Email)
            .NotNull()
            .NotEmpty()
            .WithMessage("Supplier email is required")
           // .EmailAddress()
            .WithMessage("Supplier email is invalid");
    }
    
}