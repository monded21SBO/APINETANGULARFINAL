using Plagas.Entities;
using Plagas.Entities.Infos;
using System;

namespace Plagas.Repositories;
public interface ITrampasRepository : IRepositoryBase<Trampas>
{
    Task<ICollection<TrampasInfo>> ListAsync(string? Nombre, CancellationToken cancellationToken = default);


}
