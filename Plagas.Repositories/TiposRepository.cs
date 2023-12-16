using Plagas.Entities;
using Plagas.Persistence;

namespace Plagas.Repositories
{
    public class TiposRepository : RepositoryBase<Tipos>, ITiposRepository
    {
        public TiposRepository(PlagasDbContext context) 
            : base(context)
        {
        }
    }
}