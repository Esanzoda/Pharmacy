using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Pharmacy.Commands;

public record UpdatePharmacyEmailCommand(long Id, string NewEmail) : IRequest<PharmacyResponse>;

public class UpdatePharmacyEmailCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<UpdatePharmacyEmailCommand, PharmacyResponse>
{
    public async Task<PharmacyResponse> Handle(UpdatePharmacyEmailCommand request, CancellationToken cancellationToken)
    {
        var pharmacy = await dbContext.Pharmacies
            .FindAsync(request.Id, cancellationToken);
        if (pharmacy is null)
        {
            throw new ResourseNotFoundException("Pharmacy not found");
        }

        if (pharmacy.Email == request.NewEmail)
        {
            throw new BusinessException("Pharmacy with this email alredy exsist");
        }

        pharmacy.Email = request.NewEmail;
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<PharmacyResponse>(pharmacy);
    }
}