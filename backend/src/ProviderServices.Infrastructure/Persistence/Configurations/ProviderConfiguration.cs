using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProviderServices.Domain.Entities;

namespace ProviderServices.Infrastructure.Persistence.Configurations;

public class ProviderConfiguration : IEntityTypeConfiguration<Provider>
{
    public void Configure(EntityTypeBuilder<Provider> builder)
    {
        builder.ToTable("Providers");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nit).IsRequired().HasMaxLength(20);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Website).IsRequired().HasMaxLength(300);
        builder.Property(p => p.Email).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Country).IsRequired().HasMaxLength(100);

        builder.HasIndex(p => p.Nit).IsUnique();
    }
}
