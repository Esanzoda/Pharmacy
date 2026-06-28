using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharmasy.Models.Domain;

namespace Pharmasy.Infrastructure.Configurations;

public class SupplierConfiguration:IEntityTypeConfiguration<Deliver>
{
    public void Configure(EntityTypeBuilder<Deliver> builder)
    { builder.ToTable("Suppliers");
        
        builder.HasKey(x => x.Id);
    }
} 