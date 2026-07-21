using MediatR;
using Pharmasy.CQRS.ExpiredProducts.Commands;

namespace Pharmasy.Jobs;

public class CheckExpiredProductsJob(IMediator mediator)
{
  
    public async Task CheckExpiredProductsAsync()
    {
        await mediator.Send(new CreateExpiredProductsCommand());
    }
}