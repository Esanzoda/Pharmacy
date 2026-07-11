using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Cart.Command;

public record AddItemToCartCommand(CartItemRequest ItemRequest):IRequest<CartResponse>;
public class AddItemToCartHendler:IRequestHandler<AddItemToCartCommand,CartResponse>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public AddItemToCartHendler(ICartRepository cartRepository, IProductRepository productRepository, IMapper mapper, ICartItemRepository cartItemRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _mapper = mapper;
        _cartItemRepository = cartItemRepository;
    }

    public async Task<CartResponse> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetCartByCustomerId(request.ItemRequest.CustomerId);
        if (cart == null)
        {
            throw new ResourseNotFoundException("Cart not found");
        }

        var product = await _productRepository.GetByIdAsync(request.ItemRequest.ProductId);
        if (product == null)
        {
            throw new ResourseNotFoundException("Product not found");
        }

        if (product.Stock < request.ItemRequest.Quantity)
        {
            throw new BusinessException(
                $"Insufficient product stock{product.Stock} for the requested quantity {request.ItemRequest.Quantity}");
        }

        var existingCartItem =
            await _cartItemRepository.GetItemWhithProductIdInCartItemAsync(cart.CustomerId, request.ItemRequest.ProductId);
        if (existingCartItem != null)
        {
            existingCartItem.Quantity += request.ItemRequest.Quantity;
            existingCartItem.TotalPrice = existingCartItem.Quantity * product.Price;
            await _cartItemRepository.UpdateAsync(existingCartItem);
            await _cartItemRepository.SaveChangesAsync();
        }
        else
        {
            var cartItem = _mapper.Map<CartItem>(request.ItemRequest);

            cartItem.Price = product.Price;
            cartItem.TotalPrice = product.Price * request.ItemRequest.Quantity;
            await _cartItemRepository.CreateAsync(cartItem);
            await _cartItemRepository.SaveChangesAsync();
            cart.CartItems.Add(cartItem);
        }

        cart.TotalAmount = cart.CartItems.Sum(x => x.TotalPrice);

        await _cartRepository.UpdateAsync(cart);
        await _cartRepository.SaveChangesAsync();
        return _mapper.Map<CartResponse>(cart);
    }
}