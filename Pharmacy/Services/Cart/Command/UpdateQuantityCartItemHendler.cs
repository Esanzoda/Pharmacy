using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Cart.Command;

public record UpdateQuantityCartItemCommand(long CustomerId, long CartItemId, int Quantity)
    : IRequest<CartItemResponse>;

public class UpdateQuantityCartItemHendler : IRequestHandler<UpdateQuantityCartItemCommand, CartItemResponse>
{
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IMapper _mapper;

    public UpdateQuantityCartItemHendler(ICartItemRepository cartItemRepository,
        IMapper mapper)
    {
        _cartItemRepository = cartItemRepository;
        _mapper = mapper;
    }

    public async Task<CartItemResponse> Handle(UpdateQuantityCartItemCommand request,
        CancellationToken cancellationToken)
    {
        var cartItem = await _cartItemRepository.GetCartItemByCustomerIdtemAsync(request.CustomerId, request.CartItemId);
        if (cartItem == null)
        {
            throw new ResourseNotFoundException("CartItem not found");
        }

        cartItem.Quantity = request.Quantity;
        cartItem.TotalPrice = cartItem.Price * request.Quantity;
        await _cartItemRepository.UpdateAsync(cartItem);
        await _cartItemRepository.SaveChangesAsync();
        return _mapper.Map<CartItemResponse>(cartItem);
    }
}