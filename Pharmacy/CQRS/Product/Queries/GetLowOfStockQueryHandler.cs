using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Product.Queries;

public record GetLowOfStockQuery(
    int MinQuantity, 
    int Page, 
    int PageSize) : IRequest<List<ProductResponse>>;

public class GetLowOfStockQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetLowOfStockQuery, List<ProductResponse>>
{
    public async Task<List<ProductResponse>> Handle(GetLowOfStockQuery request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .Where(x => x.Stock <= request.MinQuantity)
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        if (!product.Any())
            throw new ResourseNotFoundException("Product not found");

        return mapper.Map<List<ProductResponse>>(product);
    }
}