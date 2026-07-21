using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Pharmacy.Commands;

public record CreatePharmacyCommand(
    PharmacyRequest Request) : IRequest<PharmacyResponse>;

public class CreatePharmacyCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<CreatePharmacyCommand, PharmacyResponse>
{
    public async Task<PharmacyResponse> Handle(CreatePharmacyCommand request, CancellationToken cancellationToken)
    {
        var pharmacyExsist = await dbContext.Pharmacies
            .AnyAsync(x =>
                    x.Name == request.Request.Name &&
                    x.Email == request.Request.Email &&
                    x.Address == request.Request.Address
                , cancellationToken);

        if (pharmacyExsist)
        {
            throw new ResourseIsAlredyExistException("Pharmacy whith this information alredy exsist");
        }

        var pharmacy = mapper.Map<Models.Domain.Pharmacy>(request.Request);
        await dbContext.Pharmacies
            .AddAsync(pharmacy, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<PharmacyResponse>(pharmacy);
    }
}