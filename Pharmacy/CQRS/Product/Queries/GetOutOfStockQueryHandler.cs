using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Product.Queries;

public record GetOutOfStockQuery(
    int Page,
    int PageSize) : IRequest<List<ProductResponse>>;

public class GetOutOfStockQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetOutOfStockQuery, List<ProductResponse>>
{
    public async Task<List<ProductResponse>> Handle(GetOutOfStockQuery request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .Where(x => x.Stock == 0)
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        if (!product.Any())
            throw new RecourseNotFoundException("Product  not found");

        return mapper.Map<List<ProductResponse>>(product);
    }
}