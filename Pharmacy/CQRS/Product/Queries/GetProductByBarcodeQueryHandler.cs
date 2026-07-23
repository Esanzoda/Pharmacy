using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Product.Queries;

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
            throw new RecourseNotFoundException("Product whith this barcode not found");

        return mapper.Map<ProductResponse>(product);
    }
}