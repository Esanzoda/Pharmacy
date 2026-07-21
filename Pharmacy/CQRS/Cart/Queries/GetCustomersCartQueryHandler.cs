using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Cart.Queries;

public record GetCartItemByIQuery(long CustomerId) : IRequest<CartResponse>;

public class GetAllCartItemQueryHandler(IMapper mapper,IApplicationDbContext dbContext) : IRequestHandler<GetCartItemByIQuery, CartResponse>
{

   

    public async Task<CartResponse> Handle(GetCartItemByIQuery request, CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts
            .Include(x => x.CartItems)
            .ThenInclude(x=>x!.Product)
            .FirstOrDefaultAsync(x => x.CustomerId == request.CustomerId, 
                cancellationToken);
        if (cart is null)
        {
            throw new ResourseNotFoundException("Cart is empty");
        }

        return mapper.Map<CartResponse>(cart);
    }
}