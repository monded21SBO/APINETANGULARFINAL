using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Plagas.Entities;
using System;

namespace Plagas.Persistence.Configurations
{
    public class VisitaConfiguration : IEntityTypeConfiguration<Visita>
    {
        public void Configure(EntityTypeBuilder<Visita> builder)
        {
            builder.Property(p => p.OperationNumber)
                .IsUnicode(false)
                .HasMaxLength(20);

            builder.Property(p => p.FechaVisita)
                .HasColumnType("date")
                .HasDefaultValueSql("GETDATE()");

            builder.ToTable(nameof(Visita), schema: "Plagas");
        }

    }
}
