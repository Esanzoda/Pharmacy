using MediatR;
using Pharmacy.CQRS.ExpiredProducts.Commands;

namespace Pharmacy.Jobs;

public class CheckExpiredProductsJob(IMediator mediator)
{
    public async Task CheckExpiredProductsAsync()
    {
        await mediator.Send(new CreateExpiredProductsCommand());
    }
}