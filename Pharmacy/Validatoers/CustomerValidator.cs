using System.ComponentModel.DataAnnotations;
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
            .EmailAddress()
            .WithMessage("Invalid email format");

        RuleFor(x => x.Phonenumber)
            .NotEmpty()
            .NotNull()
            .Matches(@"^\+[1-9]\d{7,14}$")
            .WithMessage("Invalid phone number format");

        RuleFor(x => x.Address)
            .NotEmpty()
            .NotNull()
            .Matches(@"^[a-zA-Z0-9\s,.\-/#]+$")
            .WithMessage("Invalid adress format");
        
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .Matches(@"[A-Z]")
            .WithMessage("Password must contain at least one uppercase letter")
            .Matches(@"[a-z]")
            .WithMessage("Password must contain at least one lowercase letter")
            .Matches(@"[0-9]")
            .WithMessage("Password must contain at least one digit")
            .Matches(@"[!@#$%^&*(),.?""{}|<>]")
            .WithMessage("Password must contain at least one special character");

    }
    
}
