using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Product.Commands;

public record CreateProductCommand(
    ProductRequest Request
) : IRequest<ProductResponse>;

public class CreateProductCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<CreateProductCommand, ProductResponse>
{
    public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var productByName = await dbContext.Products
            .AnyAsync(x => x.Name == request.Request.Name &&
                           x.Barcode == request.Request.Barcode, cancellationToken);
        if (productByName)
            throw new ResourseIsAlredyExistException(
                $"Product already exists whith this name {request.Request.Name} or barcode {request.Request.Barcode}");
        var category = await dbContext.Categories
            .FindAsync(request.Request.CategoryId, cancellationToken);
        if (category == null)
            throw new ResourseNotFoundException($"Category with this[{request.Request.CategoryId}] not found");

        var product = mapper.Map<Models.Domain.Product>(request.Request);
        await dbContext.Products
            .AddAsync(product, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<ProductResponse>(product);
    }
}