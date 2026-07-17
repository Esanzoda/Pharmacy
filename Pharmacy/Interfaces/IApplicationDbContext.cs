using Microsoft.EntityFrameworkCore;
using Pharmasy.Models.Domain;

namespace Pharmasy.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Cart> Carts { get; }
    DbSet<CartItem> CartItems { get; }
    DbSet<Category> Categories { get; }
    DbSet<Customer> Customers { get; }
    DbSet<Employee> Employees { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
    DbSet<Product> Products { get; }
    DbSet<Purchase> Purchases { get; }
    DbSet<PurchaseItem> PurchaseItems { get; }
    DbSet<Deliver> Delivers { get; }
    DbSet<ExpiryDateProduct> ExpireDateProducts { get; }
    DbSet<ExpireDateItems> ExpireDateItems { get; }
    DbSet<RefreshToken> RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}