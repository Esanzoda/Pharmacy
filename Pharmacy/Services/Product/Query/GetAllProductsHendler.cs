using MediatR;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Product.Query;

public record GetAllProductsQuery(int Page, int PageSize) : IRequest<List<ProductResponse>>;

public class GetAllProductsHendler : ProductDiBase, IRequestHandler<GetAllProductsQuery, List<ProductResponse>>
{
    public GetAllProductsHendler(IProductRepository productRepository)
        : base(productRepository)
    {
    }

    public async Task<List<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await ProductRepository.GetAllByPaginationAsync(request.Page, request.PageSize);
        return Mapper.Map<List<ProductResponse>>(products);
    }
}