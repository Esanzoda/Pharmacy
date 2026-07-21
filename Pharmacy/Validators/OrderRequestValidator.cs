using FluentValidation;
using Pharmasy.Models.Dto.Request;

namespace Pharmasy.Validators;

public class OrderRequestValidator : AbstractValidator<OrderRequest>
{
    public OrderRequestValidator()
    {
        
    }
}