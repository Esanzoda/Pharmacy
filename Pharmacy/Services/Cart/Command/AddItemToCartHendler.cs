using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Cart.Command;

public record AddItemToCartCommand(CartItemRequest ItemRequest) : IRequest<CartResponse>;

public class AddItemToCartHendler : CartDiBase,IRequestHandler<AddItemToCartCommand, CartResponse>
{
    public AddItemToCartHendler(ICartRepository cartRepository, ICartItemRepository cartItemRepository, IProductRepository productRepository, IMapper mapper) : base(cartRepository, cartItemRepository, productRepository, mapper)
    {
    }

    public async Task<CartResponse> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await CartRepository.GetCartByCustomerId(request.ItemRequest.CustomerId);
        if (cart == null)
        {
            throw new ResourseNotFoundException("Cart not found");
        }

        var product = await ProductRepository.GetByIdAsync(request.ItemRequest.ProductId);
        if (product == null)
        {
            throw new ResourseNotFoundException("Product not found");
        }
        var existingCartItem =
            await CartItemRepository.GetItemByProductIdInCartAsync(cart.CustomerId,
                request.ItemRequest.ProductId);
 
        var alreadyInCart = existingCartItem?.Quantity ?? 0;
        var totalRequestedQuantity = alreadyInCart + request.ItemRequest.Quantity;
        if (product.Stock < totalRequestedQuantity)
        {
            throw new BusinessException(
                $"Insufficient product stock{product.Stock} for the requested quantity {totalRequestedQuantity}");
        }

        
       

        if (existingCartItem != null)
        {
            existingCartItem.Quantity += totalRequestedQuantity; //request.ItemRequest.Quantity;
            existingCartItem.TotalPrice = existingCartItem.Quantity * product.Price;
            await CartItemRepository.UpdateAsync(existingCartItem);
            await CartItemRepository.SaveChangesAsync();
        }
        else
        {
            var cartItem = Mapper.Map<CartItem>(request.ItemRequest);
            cartItem.Price = product.Price;
            cartItem.TotalPrice = product.Price * request.ItemRequest.Quantity;
            await CartItemRepository.CreateAsync(cartItem);
            await CartItemRepository.SaveChangesAsync();
            cart.CartItems.Add(cartItem);
        }

        cart.TotalAmount = cart.CartItems.Sum(x => x.TotalPrice);
        await CartRepository.UpdateAsync(cart);
        await CartRepository.SaveChangesAsync();
        return Mapper.Map<CartResponse>(cart);
    }
}