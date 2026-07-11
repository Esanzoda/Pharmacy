using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Product.Query;

public record GetLowOfStockQuery(int MinQuantity, int Page, int PageSize) : IRequest<List<ProductResponse>>;
public class GetLowOfStockHendler:ProductDiBase,IRequestHandler<GetLowOfStockQuery,List<ProductResponse>>
{
    public GetLowOfStockHendler(IProductRepository productRepository) : base(productRepository)
    {
    }

    public async Task<List<ProductResponse>> Handle(GetLowOfStockQuery request, CancellationToken cancellationToken)
    {
       
        var product = await ProductRepository.GetLowOfStockAsync(request.MinQuantity, request.Page, request.PageSize);
        if (!product.Any())
            throw new ResourseNotFoundException("Product not found");

        return Mapper.Map<List<ProductResponse>>(product);
    }
}