using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Product.Query;

public record GetOutOfStockQuery( int Page, int PageSize) : IRequest<List<ProductResponse>>;
public class GetOutOfStockHendler:ProductDiBase,IRequestHandler<GetOutOfStockQuery,List<ProductResponse>>
{
    public GetOutOfStockHendler(IProductRepository productRepository) : base(productRepository)
    {
    }

    public async Task<List<ProductResponse>> Handle(GetOutOfStockQuery request, CancellationToken cancellationToken)
    {
        var product = await ProductRepository.GetOutOfStockAsync(request.Page, request.PageSize);
        //epty list or eror meet with others
        if (!product.Any())
            throw new ResourseNotFoundException("Product  not found");

        return Mapper.Map<List<ProductResponse>>(product);
    }
}