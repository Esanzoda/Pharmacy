using AutoMapper;
using MediatR;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Product.Queries;

public record GetProductByIdQuery(
    long Id) : IRequest<ProductResponse>;

public class GetProductByIdQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetProductByIdQuery, ProductResponse>
{
    public async Task<ProductResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .FindAsync(request.Id, cancellationToken);
        if (product == null)
        {
            throw new ResourseNotFoundException("Product not found");
        }

        return mapper.Map<ProductResponse>(product);
    }
}