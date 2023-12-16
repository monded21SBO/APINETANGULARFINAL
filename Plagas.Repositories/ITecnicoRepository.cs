using Plagas.Entities;

namespace Plagas.Repositories
{
    public interface ITecnicoRepository : IRepositoryBase<Tecnicos>
    {
        Task<Tecnicos?> FindByEmailAsync(string email);


    }
}
