using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Product.Command;

public record CreateProductCommand(ProductRequest Request) : IRequest<ProductResponse>;
public class CreateProductHendler:ProductDiBase,IRequestHandler<CreateProductCommand,ProductResponse>
{
    public CreateProductHendler(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper, IDistributedCache cache) 
        : base(productRepository, categoryRepository, mapper, cache)
    {
    }

    public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var productByName = await ProductRepository.ProductExistsAsync(request.Request.Name);
        if (productByName)
            throw new ResourseIsAlredyExistException($"Product already exists whith this name {request.Request.Name}");
        var category = await CategoryRepository.GetByIdAsync(request.Request.CategoryId);
        if (category == null)
            throw new ResourseNotFoundException($"Category with this[{request.Request.CategoryId}] not found");
        var productByBarcode = await ProductRepository.GetProductByBarcodeAsync(request.Request.Barcode);
        if (productByBarcode != null)
        {
            throw new ResourseIsAlredyExistException($"Product already exists with Barcode {request.Request.Barcode}");
        }

        var product = Mapper.Map<Models.Domain.Product>(request);
        await ProductRepository.CreateAsync(product);
        await ProductRepository.SaveChangesAsync();
        return Mapper.Map<ProductResponse>(product);
    }
}