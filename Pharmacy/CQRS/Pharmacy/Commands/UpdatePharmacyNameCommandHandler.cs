using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Pharmacy.Commands;

public record UpdatePharmacyNameCommand(
    long Id,
    string NewName) : IRequest<PharmacyResponse>;

public class UpdatePharmacyNameCommandHandler(IMapper mapper, IApplicationDbContext dbContext)
    : IRequestHandler<UpdatePharmacyNameCommand, PharmacyResponse>
{
    public async Task<PharmacyResponse> Handle(UpdatePharmacyNameCommand request, CancellationToken cancellationToken)
    {
        var pharmacy = await dbContext.Pharmacies
            .FindAsync(request.Id, cancellationToken);
        if (pharmacy is null)
        {
            throw new ResourseNotFoundException("Pharmacy not found");
        }

        if (pharmacy.Name == request.NewName)
        {
            throw new BusinessException(" Pharmacy with this name alredy exsist");
        }

        pharmacy.Name = request.NewName;
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<PharmacyResponse>(pharmacy);
    }
}