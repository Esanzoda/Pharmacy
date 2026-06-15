using FluentValidation;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;

namespace Pharmasy.Validatoers;

public class CategoryValidator:AbstractValidator<CategoryRequest>

{
    public CategoryValidator()
    {
        RuleFor(request => request.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Name is required");
    }
    
}