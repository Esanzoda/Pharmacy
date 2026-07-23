using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Deliver.Commands;

public record UpdateDeliverCommand(
    long Id,
    DeliverRequest Request) : IRequest<DeliverResponse>;

public class UpdateDeliverHandler(
    IApplicationDbContext dbContext,
    IMapper mapper
) : IRequestHandler<UpdateDeliverCommand, DeliverResponse>
{
    public async Task<DeliverResponse> Handle(UpdateDeliverCommand request, CancellationToken cancellationToken)
    {
        var deliver = await dbContext.Delivers
            .FindAsync(request.Id, cancellationToken);
        if (deliver is null)
        {
            throw new RecourseNotFoundException("Deliver not found");
        }

        var deliverExist = await dbContext.Delivers
            .AnyAsync(
                x => x.Id != request.Id &&
                     (x.Email == request.Request.Email ||
                      x.PhoneNumber == request.Request.PhoneNumber),
                cancellationToken);


        if (deliverExist)
        {
            throw new BusinessException("Deliver with this number or email  alredy exsist");
        }

        mapper.Map(request.Request, deliver);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<DeliverResponse>(deliver);
    }
}