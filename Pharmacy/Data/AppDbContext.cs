using Microsoft.EntityFrameworkCore;
using Pharmasy.Infrastructure.Configurations;
using Pharmasy.Models.Domain;

namespace Pharmasy.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
        
    }
    public DbSet<Cart>Carts { get; set; }
    public DbSet<CartItem>CartItems { get; set; }
    public DbSet<Category>Categories { get; set; }
    public DbSet<Customer>Customers { get; set; }
    public DbSet<Employee>Employees { get; set; }
    public DbSet<Order>Orders { get; set; }
    public DbSet<OrderItem>OrderItems { get; set; }
    public DbSet<Product>Products { get; set; }
    public  DbSet<Purchase>Purchases { get; set; }
    public DbSet<PurchaseItem>PurchaseItems { get; set; }
    public DbSet<Supplier>Suppliers { get; set; }
    public DbSet<ExpireDateProduct>ExpireDateProducts { get; set; }
    public DbSet<ExpireDateItems> ExpireDateItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ProductConfiguration).Assembly);
    }
    
    
}