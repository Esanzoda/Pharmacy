using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Product.Command;

public record UpdateProductCommand(long Id,ProductRequest Request):IRequest<ProductResponse>;
public class UpdateProductHendler:ProductDiBase,IRequestHandler<UpdateProductCommand,ProductResponse>
{
    public UpdateProductHendler(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper, IDistributedCache cache) : base(productRepository, categoryRepository, mapper, cache)
    {
    }

    public async Task<ProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var existingEntity = await ProductRepository.GetByIdAsync(request.Id);
        if (existingEntity == null)
            throw new ResourseNotFoundException($"Product whith this id {request.Id} not found");

        var category = await CategoryRepository.GetByIdAsync(request.Request.CategoryId);
        if (category == null)
            throw new ResourseNotFoundException($"Category whis this id {request.Id} not found");
        var productByName = await ProductRepository.GetProductByNameAsync(request.Request.Name);
        if (productByName != null)
        {
            throw new ResourseIsAlredyExistException($"Product already exists with Name {request.Request.Name}");
        }

        var productByBarcode = await ProductRepository.GetProductByBarcodeAsync(request.Request.Barcode);
        if (productByBarcode != null)
        {
            throw new ResourseIsAlredyExistException($"Product already exists with Barcode {request.Request.Barcode}");
        }


        var updateProduct = Mapper.Map(request, existingEntity);
        var updateEntity = await ProductRepository.UpdateAsync(updateProduct);
        await ProductRepository.SaveChangesAsync();
        return Mapper.Map<ProductResponse>(updateEntity);
    }
}