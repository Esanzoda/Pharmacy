using AutoMapper;
using Pharmasy.Exeption;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services;

public interface ICartService :
    IBaseService<CartRequest, CartResponse>
{
    Task<CartResponse> AddItemToCartAsync(CartItemRequest itemrequest);
    Task<CartResponse> RemoveItemFromCartAsync(long cartId, long cartItemId);
    Task<CartResponse> UpdateCartItem(long cartId, long cartItemid, int quantity);
}

public class  CartService : BaseService<Cart, CartRequest, CartResponse>, ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IProductRepository _productRepository;

    public CartService(ICartRepository cartRepository, ICartItemRepository cartItemRepository,IProductRepository productRepository,
        IMapper mapper)
        : base(cartRepository, mapper)
    {
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
        _productRepository = productRepository;
    }

    public async Task<CartResponse> AddItemToCartAsync(CartItemRequest itemrequest)
    {
        var cart = await _cartRepository.GetByIdAsync(itemrequest.CartId);
        if (cart == null)
            throw new ResourseNotFoundExeption("Cart not found");

        var product = await _productRepository.GetByIdAsync(itemrequest.ProductId);
        if (product == null)
            throw new ResourseNotFoundExeption("Product not found");
        if (product.Stock < itemrequest.Quantity)
            throw new ResourseNotFoundExeption($"Insufficient product stock{product.Stock} for the requested quantity {itemrequest.Quantity}");

        var existingCartItem = await _cartItemRepository.GetProductWhithProductIdInCartItemAsync(itemrequest.ProductId);
        if (existingCartItem != null)
        {
            existingCartItem.Quantity += itemrequest.Quantity;
            existingCartItem.TotalPrice = existingCartItem.Quantity * product.Price;
            await _cartItemRepository.UpdateAsync(existingCartItem);
            
        }
        else
        {
            var cartItem = Mapper.Map<CartItem>(itemrequest);
            cartItem.CreateAt = DateTime.UtcNow;
            cartItem.Price = product.Price;
            cartItem.TotalPrice = product.Price * itemrequest.Quantity;
            cart.CartItems.Add(cartItem);
            
        }
            cart.TotalAmout = cart.CartItems.Sum(x => x.TotalPrice); 
            
        await _cartRepository.UpdateAsync(cart);
        return Mapper.Map<CartResponse>(cart);
    }

    public async Task<CartResponse> RemoveItemFromCartAsync(long cartId, long cartItemId)
    {
        var cart = await _cartRepository.GetByIdAsync(cartId);
        if (cart == null)
            throw new ResourseNotFoundExeption("Cart not found");
        var cartItem = await _cartItemRepository.GetByIdAsync(cartItemId);
        if (cartItem == null)
            throw new ResourseNotFoundExeption("CartItem not found ");

        cart.CartItems.Remove(cartItem);
        cart.TotalAmout = cart.CartItems.Sum(x => x.TotalPrice);
        await _cartRepository.UpdateAsync(cart);
        return Mapper.Map<CartResponse>(cart);
    }

    public async Task<CartResponse> UpdateCartItem(long cartId, long cartItemid, int quantity)
    {
        var cartItemupdate=await _cartItemRepository.UpdateQuantityCartItemAsync(cartId, cartItemid, quantity);
        if(!cartItemupdate)
            throw new ResourseNotFoundExeption("Cart Item not found");
        var cart = await _cartRepository.GetByIdAsync(cartId);
        if (cart == null)
            throw new ResourseNotFoundExeption("Cart not found");
      
        cart.TotalAmout = cart.CartItems.Sum(x => x.TotalPrice);
        await _cartRepository.UpdateAsync(cart);
        return Mapper.Map<CartResponse>(cartItemupdate);
    }
}