using FluentValidation;
using Pharmacy.Models.Dto.Request;

namespace Pharmacy.Validators;

public class OrderRequestValidator : AbstractValidator<OrderRequest>
{
    public OrderRequestValidator()
    {
        
    }
}