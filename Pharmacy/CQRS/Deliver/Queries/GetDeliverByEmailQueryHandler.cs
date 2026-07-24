using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Deliver.Queries;

public record GetDeliverByEmailQuery(
    long PharmacyId,
    string Email ) : IRequest<DeliverResponse>;

public class GetDeliverByEmailHandler(
    IApplicationDbContext dbContext,
    IMapper mapper
) : IRequestHandler<GetDeliverByEmailQuery, DeliverResponse>
{
    public async Task<DeliverResponse> Handle(GetDeliverByEmailQuery request, CancellationToken cancellationToken)
    {
        var deliver = await dbContext.Delivers
            .FirstOrDefaultAsync(
                x => x.PharmacyId == request.PharmacyId &&
                     x.Email == request.Email,
                cancellationToken);
        if (deliver is null)
        {
            throw new RecourseNotFoundException("Deliver not found");
        }

        return mapper.Map<DeliverResponse>(deliver);
    }
}