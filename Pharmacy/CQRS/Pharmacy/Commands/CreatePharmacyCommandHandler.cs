using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Pharmacy.Commands;

public record CreatePharmacyCommand(
    PharmacyRequest Request) : IRequest<PharmacyResponse>;

public class CreatePharmacyCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<CreatePharmacyCommand, PharmacyResponse>
{
    public async Task<PharmacyResponse> Handle(CreatePharmacyCommand request, CancellationToken cancellationToken)
    {
        var pharmacyExist = await dbContext.Pharmacies
            .AnyAsync(x =>
                    x.Name == request.Request.Name &&
                    x.Email == request.Request.Email &&
                    x.Address == request.Request.Address
                , cancellationToken);

        if (pharmacyExist)
        {
            throw new RecourseIsAlreadyExistException("Pharmacy with this information already exist");
        }

        var pharmacy = mapper.Map<Models.Domain.Pharmacy>(request.Request);
        await dbContext.Pharmacies
            .AddAsync(pharmacy, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<PharmacyResponse>(pharmacy);
    }
}