using AutoMapper;
using Pharmasy.Exeption;
using Pharmasy.Helpers;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services;

public interface ICartItemService
    : IBaseService<CartItemRequest, CartItemResponse>
{
    Task<CartResponse> AddItemToCartAsync(CartItemRequest itemrequest);
    Task<CartItemResponse> UpdateQuantityCartItem(long customerId, long cartItemid, int quantity);
    Task<bool> DeleteItemFromCartAsync(long customerId, long cartItemid);
    Task<CartItemResponse> GetItemWhithProductIdInCartItemAsync(long customerId, long productId);
}

public class CartItemService : BaseService<CartItem, CartItemRequest, CartItemResponse>, ICartItemService
{
    private readonly ICartItemRepository _cartItemRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;


    public CartItemService(ICartItemRepository cartItemRepository, IMapper mapper,
        ICartRepository cartRepository, IProductRepository productRepository)
        : base(cartItemRepository, mapper)
    {
        _cartItemRepository = cartItemRepository;
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }


    public async Task<CartResponse> AddItemToCartAsync(CartItemRequest itemrequest)
    {
        var cart = await _cartRepository.GetCartByCustomerId(itemrequest.CustomerId);
        if (cart == null)
        {
            throw new ResourseNotFoundExeption("Cart not found");
        }

        var product = await _productRepository.GetByIdAsync(itemrequest.ProductId);
        if (product == null)
        {
            throw new ResourseNotFoundExeption("Product not found");
        }

        if (product.Stock < itemrequest.Quantity)
        {
            throw new BusinessExseption(
                $"Insufficient product stock{product.Stock} for the requested quantity {itemrequest.Quantity}");
        }

        var existingCartItem = await _cartItemRepository.GetItemWhithProductIdInCartItemAsync(itemrequest.ProductId);
        if (existingCartItem != null)
        {
            existingCartItem.Quantity += itemrequest.Quantity;
            existingCartItem.TotalPrice = existingCartItem.Quantity * product.Price;
            await _cartItemRepository.UpdateAsync(existingCartItem);
            await _cartItemRepository.SavechangesAsync();
        }
        else
        {
            var cartItem = Mapper.Map<CartItem>(itemrequest);

            cartItem.Price = product.Price;
            cartItem.TotalPrice = product.Price * itemrequest.Quantity;
            await _cartItemRepository.CreateAsync(cartItem);
            await _cartItemRepository.SavechangesAsync();
            cart.CartItems.Add(cartItem);
        }

        cart.TotalAmout = cart.CartItems.Sum(x => x.TotalPrice);

        await _cartRepository.UpdateAsync(cart);
        await _cartRepository.SavechangesAsync();
        return Mapper.Map<CartResponse>(cart);
    }

    public async Task<CartItemResponse> UpdateQuantityCartItem(long customerId, long cartItemid, int quantity)
    {
        var cartItem = await _cartItemRepository.UpdateQuantityCartItemAsync(customerId, cartItemid);
        if (cartItem == null)
        {
            throw new ResourseNotFoundExeption("CartItem not found");
        }

        cartItem.Quantity = quantity;
        cartItem.TotalPrice = cartItem.Price * quantity;
        await _cartItemRepository.UpdateAsync(cartItem);
        await _cartItemRepository.SavechangesAsync();
        return Mapper.Map<CartItemResponse>(cartItem);
    }

    public async Task<bool> DeleteItemFromCartAsync(long customerId, long cartItemId)
    {
        var cart = await _cartRepository.GetCartByCustomerId(customerId);
        if (cart == null)
        {
            throw new ResourseNotFoundExeption("Cart not found");
        }

        var item = await _cartItemRepository.DeleteAsync(cartItemId);
        if (item != true)
        {
            throw new Exception("Cant't delete item in cart");
        }

        cart.CartItems.Remove(cart.CartItems.First(x => x.Id == cartItemId));
        cart.TotalAmout = cart.CartItems.Sum(x => x.TotalPrice);
        await _cartRepository.UpdateAsync(cart);
        await _cartRepository.SavechangesAsync();
        return true;
    }

    public async Task<CartItemResponse> GetItemWhithProductIdInCartItemAsync(long customerId, long productId)
    {
        var cart = await _cartRepository.GetCartByCustomerId(customerId);
        if (cart == null)
        {
            throw new ResourseNotFoundExeption("Cart not found");
        }

        var item = await _cartItemRepository.GetItemWhithProductIdInCartItemAsync(productId);
        if (item == null)
        {
            throw new ResourseNotFoundExeption($"Product with id {productId} was not found in the cart");
        }

        return Mapper.Map<CartItemResponse>(item);
    }
}