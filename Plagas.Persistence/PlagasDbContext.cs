using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Plagas.Entities;
using Plagas.Entities.Infos;

namespace Plagas.Persistence
{
    public class PlagasDbContext : IdentityDbContext<PlagasUserIdentity>
    {
        public PlagasDbContext(DbContextOptions<PlagasDbContext> options)
        : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Esta linea va a traer toda la configuracion de las tablas asociadas a la aplicacion
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //modelBuilder.Entity<TrampasInfo>()
            // .HasNoKey(); // Aqui decimos que no es una tabla, no hay llave primaria

            // Si queremos usar esquema
            //modelBuilder.Entity<Tipos>()
            //    .ToTable("Tiposs", schema: "principales");

            modelBuilder.Entity<PlagasUserIdentity>(e => e.ToTable("Usuario"));
            modelBuilder.Entity<IdentityRole>(e => e.ToTable("Rol"));
            modelBuilder.Entity<IdentityUserRole<string>>(e => e.ToTable("UsuarioRol"));

        }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            // Con esto eliminamos el Cascade como metodo principal para la eliminacion de registros
            configurationBuilder.Conventions.Remove(typeof(CascadeDeleteConvention));
        }


    }
}