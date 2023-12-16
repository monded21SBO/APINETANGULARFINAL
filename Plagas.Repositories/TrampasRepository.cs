using Microsoft.EntityFrameworkCore;
using Plagas.Entities;
using Plagas.Entities.Infos;
using Plagas.Persistence;
using System;

namespace Plagas.Repositories
{
    public class TrampasRepository : RepositoryBase<Trampas>, ITrampasRepository
    {
        public TrampasRepository(PlagasDbContext context) : base(context)
        {
        }

        public async Task<ICollection<TrampasInfo>> ListAsync(string? nombre, CancellationToken cancellationToken = default)
        {
           
            return await Context.Set<Trampas>()
                .Where(p => p.Nombre.Contains(nombre ?? string.Empty))
                .AsNoTracking()
                .IgnoreQueryFilters()
                .Select(p => new TrampasInfo
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Ubicacion = p.Ubicacion,
                    Tipos = p.Tipos.Name,
                    TiposId = p.TiposId,
                    FechaInstalacion = p.FechaInstalacion.ToShortDateString(), 
                    ImageUrl = p.ImageUrl,
                   
                    Status = p.Status ? "Activo" : "Inactivo",
                })
                .ToListAsync(cancellationToken); 
        }

        












    }
}
