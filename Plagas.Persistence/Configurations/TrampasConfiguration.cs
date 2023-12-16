using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Plagas.Entities;
using System;


namespace Plagas.Persistence.Configurations 
{
    public class TrampasConfiguration : IEntityTypeConfiguration<Trampas>
    {
        public void Configure(EntityTypeBuilder<Trampas> builder)
        {
            builder.Property(p => p.Nombre)
                .HasMaxLength(100);

            builder.Property(p => p.Descripcion)
                .HasMaxLength(200);

            builder.Property(p => p.Ubicacion)
                .HasMaxLength(100);

            builder.Property(p => p.FechaInstalacion)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()"); 

            builder.Property(p => p.ImageUrl)
                .IsUnicode(false)
                .HasMaxLength(1000);

            builder.HasIndex(p => p.Nombre);

            builder.ToTable("Trampas", schema: "Plagas");
        }




    

    }
}
