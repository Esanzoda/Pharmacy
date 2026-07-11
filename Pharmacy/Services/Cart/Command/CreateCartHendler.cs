using AutoMapper;
using MediatR;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Cart.Command;

public record CreateCartCommand(CartRequest Request) : IRequest<CartResponse>;
public class CreateCartHendler:IRequestHandler<CreateCartCommand,CartResponse>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    public CreateCartHendler(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    public async Task<CartResponse> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        var existingCart = await _cartRepository.GetCartByCustomerId(request.Request.CustomerId);
        if (existingCart != null)
            return _mapper.Map<CartResponse>(existingCart);

        var cart = _mapper.Map<Models.Domain.Cart>(request);
        cart.TotalAmount = 0;
        await _cartRepository.CreateAsync(cart);
        await _cartRepository.SaveChangesAsync();
        return _mapper.Map<CartResponse>(cart);
    }
}