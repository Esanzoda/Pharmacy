using MediatR;
using Pharmasy.CQRS.ExpiredProducts.Commands;

namespace Pharmasy.Jobs;

public class Report(IMediator mediator)
{
    
    
    public async Task ReportToCeo()
    {
        await mediator.Send(new ReportCommand());
    }
}