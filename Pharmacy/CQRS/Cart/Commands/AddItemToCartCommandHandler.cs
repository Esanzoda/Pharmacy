using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Cart.Commands;

public record AddItemToCartCommand(
    long CustomerId,
    CartItemRequest ItemRequest
) : IRequest<CartResponse>;

public class AddItemToCartCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext)
    : IRequestHandler<AddItemToCartCommand, CartResponse>
{
    public async Task<CartResponse> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts
            .Include(x => x.CartItems)
            .FirstOrDefaultAsync(x => x.CustomerId == request.CustomerId, cancellationToken);

        if (cart is null)
        {
            throw new ResourseNotFoundException("Cart not found");
        }

        var product = await dbContext.Products
            .FirstOrDefaultAsync(x => x.Id == request.ItemRequest.ProductId, cancellationToken);
        if (product == null)
        {
            throw new ResourseNotFoundException("Product not found");
        }

        var existingCartItem = await dbContext.CartItems
            .FirstOrDefaultAsync(x => x.ProductId == request.ItemRequest.ProductId &&
                                      x.CustomerId == request.CustomerId, cancellationToken);


        var alreadyInCart = existingCartItem?.Quantity ?? 0;
        var totalRequestedQuantity = alreadyInCart + request.ItemRequest.Quantity;
        if (product.Stock < totalRequestedQuantity)
        {
            throw new BusinessException(
                $"Insufficient product stock{product.Stock} for the requested quantity {totalRequestedQuantity}");
        }


        if (existingCartItem != null)
        {
            existingCartItem.Quantity += request.ItemRequest.Quantity;
            existingCartItem.TotalPrice = existingCartItem.Quantity * product.Price;
        }
        else
        {
            var cartItem = mapper.Map<CartItem>(request.ItemRequest);
            cartItem.CustomerId = request.CustomerId;
            cartItem.Price = product.Price;
            cartItem.TotalPrice = product.Price * request.ItemRequest.Quantity;
            await dbContext.CartItems
                .AddAsync(cartItem, cancellationToken);
        }

        cart.TotalAmount = cart.CartItems.Sum(x => x.TotalPrice);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<CartResponse>(cart);
    }
}