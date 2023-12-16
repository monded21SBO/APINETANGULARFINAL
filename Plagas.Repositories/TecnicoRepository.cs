using Microsoft.EntityFrameworkCore;
using Plagas.Entities;
using Plagas.Persistence;


namespace Plagas.Repositories
{
    public class TecnicoRepository : RepositoryBase<Tecnicos>, ITecnicoRepository
    {
        public TecnicoRepository(PlagasDbContext context)
      : base(context)
        {
        }

        public async Task<Tecnicos?> FindByEmailAsync(string email)
        {
            return await Context.Set<Tecnicos>().FirstOrDefaultAsync(p => p.Email == email);
        }


    }
}
