using FluentValidation;
using Pharmacy.Models.Dto.Request;

namespace Pharmacy.Validators;

public class CategoryRequestValidator : AbstractValidator<CreateCategoryRequest>

{
    public CategoryRequestValidator()
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