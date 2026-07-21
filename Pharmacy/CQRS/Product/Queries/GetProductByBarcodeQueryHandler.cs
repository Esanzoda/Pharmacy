using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Product.Queries;

public record GetProductByBarcodeQuery(
    string Barcode) : IRequest<ProductResponse>;

public class GetProductByBarcodeQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetProductByBarcodeQuery, ProductResponse>
{
    public async Task<ProductResponse> Handle(GetProductByBarcodeQuery request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .FirstOrDefaultAsync(x => x.Barcode == request.Barcode, cancellationToken);
        if (product == null)
            throw new ResourseNotFoundException("Product whith this barcode not found");

        return mapper.Map<ProductResponse>(product);
    }
}