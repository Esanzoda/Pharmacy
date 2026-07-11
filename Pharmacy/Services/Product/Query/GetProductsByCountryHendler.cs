using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Domain.Enum;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Product.Query;

public record GetProductsByCountryQuery(CountryEnum Country, int Page, int PageSize) : IRequest<List<ProductResponse>>;
public class GetProductsByCountryHendler:ProductDiBase,IRequestHandler<GetProductsByCountryQuery,List<ProductResponse>>
{
    public GetProductsByCountryHendler(IProductRepository productRepository) : base(productRepository)
    {
    }

    public async Task<List<ProductResponse>> Handle(GetProductsByCountryQuery request, CancellationToken cancellationToken)
    {
        var product = await ProductRepository.GetProductsByCountryAsync(request.Country, request.Page, request.PageSize);
        if (!product.Any())
            throw new ResourseNotFoundException($"Product from this country[{request.Country}] not found");
        return Mapper.Map<List<ProductResponse>>(product);
    }
}