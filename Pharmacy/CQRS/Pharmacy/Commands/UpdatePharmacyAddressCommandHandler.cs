using AutoMapper;
using MediatR;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Pharmacy.Commands;

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
            throw new RecourseNotFoundException("Pharmacy not found");
        }

        if (pharmacy.Address == request.NewAddress)
        {
            throw new BusinessException("Pharmacy with this email already exist");
        }

        pharmacy.Address = request.NewAddress;
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<PharmacyResponse>(pharmacy);
    }
}