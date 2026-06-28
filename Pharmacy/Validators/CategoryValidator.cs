using FluentValidation;
using Pharmasy.Models.Dto.Request;

namespace Pharmasy.Validators;

public class  CategoryValidator:AbstractValidator<CategoryRequest>

{
    public CategoryValidator()
    {
        RuleFor(request => request.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Name is required");
        RuleFor(request => request.Description)
            .NotNull()
            .NotEmpty()
            .WithMessage("Description is required");
    }
    
}