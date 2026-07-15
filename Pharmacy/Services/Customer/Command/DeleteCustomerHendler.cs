using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Customer.Command;

public record DeleteCustomerCommand(long Id) : IRequest<bool>;

public class DeleteCustomerHendler : CustomerDiBase, IRequestHandler<DeleteCustomerCommand, bool>
{
    private readonly IDistributedCache _cache;

    public DeleteCustomerHendler(ICustomerRepository customerRepository, IMapper mapper, IDistributedCache cache)
        : base(customerRepository, mapper)
    {
        _cache = cache;
    }

    public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        await CustomerRepository.DeleteAsync(request.Id);

        var key = $"CustomerById-{request.Id}";
        await _cache.RemoveAsync(key, cancellationToken);

        return true;
    }
}