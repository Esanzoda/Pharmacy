using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Cart.Commands;

public record CreateCartCommand(
    CartRequest Request
    ) : IRequest<CartResponse>;

public class CreateCartCommandHandler(IMapper mapper,IApplicationDbContext dbContext) : IRequestHandler<CreateCartCommand, CartResponse>
{

    public async Task<CartResponse> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        var existingCart = await dbContext.Carts
            .AnyAsync(x => x.CustomerId == request.Request.CustomerId,cancellationToken);

        if (existingCart)
        {
            throw new ResourseIsAlredyExistException("Cart olredy exsist");
        }
            

        var cart = mapper.Map<Models.Domain.Cart>(request.Request);
        cart.TotalAmount = 0;
        await dbContext.Carts
            .AddAsync(cart, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<CartResponse>(cart);
    }
}