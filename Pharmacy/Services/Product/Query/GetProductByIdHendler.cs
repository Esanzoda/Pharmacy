using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;
using Pharmasy.Services.Order.Query;

namespace Pharmasy.Services.Product.Query;

public record GetProductByIdQuery(long Id):IRequest<ProductResponse>;
public class GetProductByIdHendler:ProductDiBase,IRequestHandler<GetProductByIdQuery,ProductResponse>
{
    public GetProductByIdHendler(IProductRepository productRepository) : base(productRepository)
    {
    }

    public async Task<ProductResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await ProductRepository.GetByIdAsync(request.Id);
        if (product == null)
        {
            throw new ResourseNotFoundException("Product not found");
        }
        return Mapper.Map<ProductResponse>(product);
    }
}