using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Pharmacy.Commands;

public record UpdatePharmacyCommand(
    long Id,
    PharmacyRequest Request) : IRequest<PharmacyResponse>;

public class UpdatePharmacyCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<UpdatePharmacyCommand, PharmacyResponse>
{
    public async Task<PharmacyResponse> Handle(UpdatePharmacyCommand request, CancellationToken cancellationToken)
    {
        var pharmacy = await dbContext.Pharmacies
            .FindAsync(request.Id, cancellationToken);
        if (pharmacy is null)
        {
            throw new ResourseNotFoundException("Pharmacy not found");
        }

        mapper.Map(request.Request, pharmacy);
        dbContext.Pharmacies
            .Update(pharmacy);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<PharmacyResponse>(pharmacy);
    }
}