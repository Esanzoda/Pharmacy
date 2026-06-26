
using FluentValidation;
using Pharmasy.Models.Dto.Request;

namespace Pharmasy.Validatoers;

public class CustomerValidator:AbstractValidator<CustomerRequest>
{
    public CustomerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .WithMessage("Name is requared");

        RuleFor(x => x.Email)
            .NotEmpty()
            .NotNull()
            .WithMessage("Email is required")
           // .EmailAddress()
            .WithMessage("Invalid email format");

        RuleFor(x => x.Phonenumber)
            .NotEmpty()
            .NotNull()
            .WithMessage("Invalid phone number format");

        RuleFor(x => x.Address)
            .NotEmpty()
            .NotNull()
            .WithMessage("Invalid adress format");

/*
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters");*/

    }
    
}
