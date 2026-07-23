using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharmacy.Models.Domain;

namespace Pharmacy.Infrastructure.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id)
            .IsUnique();

        builder.HasOne<Order>()
            .WithMany(x => x.OrderItems)
            .HasForeignKey(x => x.OrderId);

        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId);
    }
}