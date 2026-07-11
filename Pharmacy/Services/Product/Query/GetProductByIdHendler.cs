using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;
using Pharmasy.Services.Order.Query;

namespace Pharmasy.Services.Product.Query;

public record GetProductByIdQuery(long Id):IRequest<OrderResponse>;
public class GetProductByIdHendler:ProductDiBase,IRequestHandler<GetOrderByIdQuery,OrderResponse>
{
    public GetProductByIdHendler(IProductRepository productRepository) : base(productRepository)
    {
    }

    public async Task<OrderResponse> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await ProductRepository.GetByIdAsync(request.Id);
        if (product == null)
        {
            throw new ResourseNotFoundException("Product not found");
        }
        return Mapper.Map<OrderResponse>(product);
    }
}