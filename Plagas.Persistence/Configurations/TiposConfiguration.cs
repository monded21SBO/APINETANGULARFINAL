using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Plagas.Entities;

namespace Plagas.Persistence.Configurations;

public class TiposConfiguration : IEntityTypeConfiguration<Tipos>
{
    public void Configure(EntityTypeBuilder<Tipos> builder)
    {
        // Fluent API

        builder
            .Property(p => p.Name)
            .HasMaxLength(50);

        builder.ToTable("Tipos", schema: "Plagas");

        builder.HasQueryFilter(p => p.Status);
    }
}