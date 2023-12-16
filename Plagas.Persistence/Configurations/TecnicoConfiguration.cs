using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Plagas.Entities;



namespace Plagas.Persistence.Configurations
{
    public class TecnicoConfiguration : IEntityTypeConfiguration<Tecnicos>
    {
        public void Configure(EntityTypeBuilder<Tecnicos> builder)
        {
            builder.Property(p => p.Email)
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(p => p.FullName)
                .HasMaxLength(200);

            builder.ToTable(nameof(Tecnicos), schema: "Plagas");
        }

    }
}
