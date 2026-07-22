using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain;

namespace Pharmacy.CQRS.ExpiredProducts.Commands;

public record CreateExpiredProductsCommand : IRequest;

public class CheckExpiredProductsHandler(
    IApplicationDbContext dbcontext,
    ILogger<CheckExpiredProductsHandler> logger
) : IRequestHandler<CreateExpiredProductsCommand>
{
    public async Task Handle(CreateExpiredProductsCommand request,
        CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;

        var expiredProducts = await dbcontext.Products
            .Where(x => x.ExpiryDate.Date <= today && x.Stock > 0)
            .ToListAsync(cancellationToken);

        if (!expiredProducts.Any())
            return;

        var report = new ExpiryDateProduct();

        foreach (var product in expiredProducts)
        {
            var oldstock = product.Stock;
            var item = new ExpireDateItems
            {
                Product = product,
                ProductName = product.Name,
                Quantity = oldstock,
                TotalOrderPrice = product.Stock * product.Price,
                TotalPurchasePrice = product.Stock * product.PurchasePrice
            };

            report.ExpiredateItemsList.Add(item);

            report.TotalOrderPrice += item.TotalOrderPrice;
            report.TotalPurchasePrice += item.TotalPurchasePrice;

            product.Stock = 0;
        }

        dbcontext.ExpireDateProducts.Add(report);

        await dbcontext.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Created expired products report with {Count} items.",
            report.ExpiredateItemsList.Count);
    }
}