using MediatR;
using Pharmacy.CQRS.ExpiredProducts.Commands;

namespace Pharmacy.Jobs;

public class Report(IMediator mediator)
{
    
    
    public async Task ReportToCeo()
    {
        await mediator.Send(new ReportCommand());
    }
}