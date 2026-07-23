using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Product.Commands;

public record UpdateProductCommand(
    long Id,
    ProductRequest Request)
    : IRequest<ProductResponse>;

public class UpdateProductCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<UpdateProductCommand, ProductResponse>
{
    public async Task<ProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .FindAsync(request.Id, cancellationToken);
        if (product == null)
            throw new RecourseNotFoundException($"Product whith this id {request.Id} not found");
        var categoryExists = await dbContext.Categories
            .AnyAsync(x => x.Id == request.Request.CategoryId, cancellationToken);
        if (!categoryExists)
        {
            throw new RecourseNotFoundException($"Category whis this id {request.Request.CategoryId} not found");
        }

        var productExist = await dbContext.Products
            .AnyAsync(x => x.Id != request.Id &&
                           (x.Name == request.Request.Name ||
                            x.Barcode == request.Request.Barcode), cancellationToken);
        if (productExist)
        {
            throw new RecourseIsAlreadyExistException(
                $"Product already exists with Name {request.Request.Name} orwith Barcode {request.Request.Barcode} ");
        }

        mapper.Map(request.Request, product);

        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<ProductResponse>(product);
    }
}