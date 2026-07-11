using AutoMapper;
using MediatR;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Customer.Command;

public record DeleteCustomerCommand(long Id) : IRequest<bool>;

public class DeleteHendler : CustomerDiBase, IRequestHandler<DeleteCustomerCommand, bool>
{
    public DeleteHendler(ICustomerRepository customerRepository, IMapper mapper)
        : base(customerRepository, mapper)
    {
    }

    public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        await CustomerRepository.DeleteAsync(request.Id);
        return true;
    }
}