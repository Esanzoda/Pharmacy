using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Deliver.Commands;

public record CreateDeliverCommand(
    DeliverRequest DeliverRequest
) : IRequest<DeliverResponse>;

public class CreateDeliverCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext
) : IRequestHandler<CreateDeliverCommand, DeliverResponse>
{
    public async Task<DeliverResponse> Handle(CreateDeliverCommand request, CancellationToken cancellationToken)
    {
        var deliver = await dbContext.Delivers
            .FirstOrDefaultAsync(x => x.Email == request.DeliverRequest.Email, cancellationToken);

        if (deliver is not null)
        {
            throw new ResourseIsAlredyExistException("Deliver already exist");
        }

        var newDeliver = mapper.Map<Models.Domain.Deliver>(request.DeliverRequest);
        await dbContext.Delivers
            .AddAsync(newDeliver, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<DeliverResponse>(newDeliver);
    }
}