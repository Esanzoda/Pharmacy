using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Product.Query;

public record GetProductsByCategoryIdQuery(long CategoryId, int Page, int PageSize) : IRequest<List<ProductResponse>>;
public class GetProductsByCategoryIdHendler: ProductDiBase,IRequestHandler<GetProductsByCategoryIdQuery,List<ProductResponse>>
{
    public GetProductsByCategoryIdHendler(IProductRepository productRepository) : base(productRepository)
    {
    }

    public async Task<List<ProductResponse>> Handle(GetProductsByCategoryIdQuery request, CancellationToken cancellationToken)
    {
        
        var category = await CategoryRepository.GetByIdAsync(request.CategoryId);
        if (category == null)
            throw new ResourseNotFoundException("Category with this id  not found");

        var product = await ProductRepository.GetProductsByCategoryIdAsync(request.CategoryId, request.Page, request.PageSize);
        if (!product.Any())
            throw new ResourseNotFoundException("We dont have product  with categoryId ");
        return Mapper.Map<List<ProductResponse>>(product);
    }
}