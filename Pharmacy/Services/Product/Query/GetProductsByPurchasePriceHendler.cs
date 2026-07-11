using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Product.Query;

public record GetProductsByPurchasePriceQuery(decimal Price, int Page, int PageSize) : IRequest<List<ProductResponse>>;
public class GetProductsByPurchasePriceHendler:ProductDiBase,IRequestHandler<GetProductsByPurchasePriceQuery,List<ProductResponse>>
{
    public GetProductsByPurchasePriceHendler(IProductRepository productRepository) : base(productRepository)
    {
    }

    public async Task<List<ProductResponse>> Handle(GetProductsByPurchasePriceQuery request, CancellationToken cancellationToken)
    {
        var product = await ProductRepository.GetProductsByPurchasePriceAsync(request.Price, request.Page, request.PageSize);
        if (!product.Any())
            throw new ResourseNotFoundException("Product with this purchase price  not found");

        return Mapper.Map<List<ProductResponse>>(product);
    }
}