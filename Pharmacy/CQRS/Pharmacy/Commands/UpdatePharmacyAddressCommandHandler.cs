using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Pharmacy.Commands;

public record UpdatePharmacyAddressCommand(
    long Id,
    string NewAddress) : IRequest<PharmacyResponse>;

public class UpdatePharmacyAddressCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext)
    : IRequestHandler<UpdatePharmacyAddressCommand, PharmacyResponse>
{
    public async Task<PharmacyResponse> Handle(UpdatePharmacyAddressCommand request,
        CancellationToken cancellationToken)
    {
        var pharmacy = await dbContext.Pharmacies
            .FindAsync(request.Id, cancellationToken);
        if (pharmacy is null)
        {
            throw new ResourseNotFoundException("Pharmacy not found");
        }

        if (pharmacy.Address == request.NewAddress)
        {
            throw new BusinessException("Pharmacy with this email alredy exsist");
        }

        pharmacy.Address = request.NewAddress;
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<PharmacyResponse>(pharmacy);
    }
}