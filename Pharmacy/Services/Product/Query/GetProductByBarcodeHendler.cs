using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Product.Query;

public record GetProductByBarcodeQuery(string Barcode) : IRequest<ProductResponse>;

public class GetProductByBarcodeHendler : ProductDiBase, IRequestHandler<GetProductByBarcodeQuery, ProductResponse>
{
    public GetProductByBarcodeHendler(IProductRepository productRepository) : base(productRepository)
    {
    }

    public async Task<ProductResponse> Handle(GetProductByBarcodeQuery request, CancellationToken cancellationToken)
    {
        var product = await ProductRepository.GetProductByBarcodeAsync(request.Barcode);
        if (product == null)
            throw new ResourseNotFoundException("Product whith this barcode not found");

        return Mapper.Map<ProductResponse>(product);
    }
}