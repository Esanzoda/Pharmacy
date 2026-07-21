using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Product.Queries;

public record GetProductsByOrderPriceQuery(
    decimal Price,
    int Page,
    int PageSize) : IRequest<List<ProductResponse>>;

public class GetProductsByOrderPriseQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetProductsByOrderPriceQuery, List<ProductResponse>>
{
    public async Task<List<ProductResponse>> Handle(GetProductsByOrderPriceQuery request,
        CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .Where(x => x.Price == request.Price)
            .OrderBy(x => x.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        if (!product.Any())
            throw new ResourseNotFoundException("Product with this price  not found");

        return mapper.Map<List<ProductResponse>>(product);
    }
}