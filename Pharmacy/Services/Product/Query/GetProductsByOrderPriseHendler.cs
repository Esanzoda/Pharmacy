using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Product.Query;

public record GetProductsByOrderPriseQuery(decimal Price, int Page, int PageSize) : IRequest<List<ProductResponse>>;
public class GetProductsByOrderPriseHendler:ProductDiBase,IRequestHandler<GetProductsByOrderPriseQuery,List<ProductResponse>>
{
    public GetProductsByOrderPriseHendler(IProductRepository productRepository) : base(productRepository)
    {
    }

    public async Task<List<ProductResponse>> Handle(GetProductsByOrderPriseQuery request, CancellationToken cancellationToken)
    {
        var product = await ProductRepository.GetProductsByOrderPriceAsync(request.Price, request.Page, request.PageSize);
        if (!product.Any())
            throw new ResourseNotFoundException("Product with this price  not found");

        return Mapper.Map<List<ProductResponse>>(product);
    }
}