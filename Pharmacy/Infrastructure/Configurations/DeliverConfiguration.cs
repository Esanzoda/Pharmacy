using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharmacy.Models.Domain;

namespace Pharmacy.Infrastructure.Configurations;

public class DeliverConfiguration : IEntityTypeConfiguration<Deliver>
{
    public void Configure(EntityTypeBuilder<Deliver> builder)
    {
        builder.ToTable("Suppliers");

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id)
            .IsUnique();
    }
}