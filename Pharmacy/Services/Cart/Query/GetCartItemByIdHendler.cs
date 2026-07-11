using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Cart.Query;

public record GetCartItemByIQuery(long Id, long CartItemId) : IRequest<CartResponse>;

public class GetCartItemByIdHendler : IRequestHandler<GetCartItemByIQuery, CartResponse>
{
    private readonly ICartItemRepository _cartItemRepository;

    private readonly IMapper _mapper;

    public GetCartItemByIdHendler(ICartItemRepository cartItemRepository, IMapper mapper)
    {
        _cartItemRepository = cartItemRepository;
        _mapper = mapper;
    }

    public async Task<CartResponse> Handle(GetCartItemByIQuery request, CancellationToken cancellationToken)
    {
        var cartItem = await _cartItemRepository.GetCartItemByCustomerIdtemAsync(request.Id, request.CartItemId);
        if (cartItem == null)
        {
            throw new ResourseNotFoundException("Item not found");
        }

        return _mapper.Map<CartResponse>(cartItem);
    }
}