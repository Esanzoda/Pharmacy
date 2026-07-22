using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Cart.Commands;

public record UpdateQuantityCartItemCommand(
    long CustomerId,
    long ProductId,
    int Quantity)
    : IRequest<CartItemResponse>;

public class UpdateQuantityCartItemCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : 
    IRequestHandler<UpdateQuantityCartItemCommand, CartItemResponse>
{
   

    public async Task<CartItemResponse> Handle(UpdateQuantityCartItemCommand request,
        CancellationToken cancellationToken)
    {
        var cartItem = await dbContext.CartItems
            .Include(x => x.Cart)
            .FirstOrDefaultAsync(x =>x.ProductId == request.ProductId &&
                                     x.CustomerId==request.CustomerId,
                cancellationToken);
        
        if (cartItem is null )
        {
            throw new ResourseNotFoundException("CartItem not found");
        }
        cartItem.Quantity = request.Quantity;
        cartItem.TotalPrice = cartItem.Price * request.Quantity;
       cartItem.Cart!.TotalAmount = cartItem.Cart.CartItems.Sum(x=>x.TotalPrice);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<CartItemResponse>(cartItem);
    }
}