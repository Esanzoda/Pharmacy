using AutoMapper;
using MediatR;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Cart.Command;

public record ClearCartCommand(long CustomerId) : IRequest<bool>;
public class ClearCartHendler:IRequestHandler<ClearCartCommand,bool>
{
    private readonly ICartRepository _cartRepository;

    public ClearCartHendler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<bool> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
      return  await _cartRepository.DeleteAsync(request.CustomerId);
    }
}