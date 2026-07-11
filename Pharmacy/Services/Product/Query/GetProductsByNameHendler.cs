using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Product.Query;
public record GetProductsByNameQuery(string Name,int Page,int PageSize):IRequest<List<ProductResponse>>;
public class GetProductsByNameHendler:ProductDiBase,IRequestHandler<GetProductsByNameQuery,List<ProductResponse>>
{
    public GetProductsByNameHendler(IProductRepository productRepository) : base(productRepository)
    {
    }

    public async Task<List<ProductResponse>> Handle(GetProductsByNameQuery request, CancellationToken cancellationToken)
    {
        var product = await ProductRepository.GetProductsByNameAsync(request.Name,request.Page,request.PageSize);
        if (!product.Any())
            throw new ResourseNotFoundException("Product with this naame not found");

        return Mapper.Map<List<ProductResponse>>(product);
    }
}