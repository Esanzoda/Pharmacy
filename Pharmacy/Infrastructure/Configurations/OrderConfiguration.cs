using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharmasy.Models.Domain;

namespace Pharmasy.Infrastructure.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id)
            .IsUnique();
        
        builder.HasOne(x => x.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(x=>x.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}