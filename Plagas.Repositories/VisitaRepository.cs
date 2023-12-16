using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Plagas.Entities;
using Plagas.Entities.Infos;
using Plagas.Persistence;

namespace Plagas.Repositories
{
    public  class VisitaRepository : RepositoryBase<Visita>, IVisitaRepository
    {
        public VisitaRepository(PlagasDbContext context)
        : base(context)
        {
        }

        public async Task CreateTransactionAsync()
        {
            await Context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        }

        public override async Task UpdateAsync()
        {
            await Context.Database.CommitTransactionAsync();
            await base.UpdateAsync();
        }

        public async Task RollBackAsync()
        {
            await Context.Database.RollbackTransactionAsync();
        }

        public override async Task<int> AddAsync(Visita entity)
        {
            entity.FechaVisita = DateTime.Now;
            var lastNumber = await Context.Set<Visita>().CountAsync() + 1;
            entity.OperationNumber = $"{lastNumber:000000}";

            // Agregar la entidad al Context de forma explicita
            //await Context.Set<Sale>().AddAsync(entity);

            // Agregar la entidad de forma implícita
            await Context.AddAsync(entity);

            return entity.Id;
        }


        public override async Task<Visita?> FindByIdAsync(int id)
        {
            return await Context.Set<Visita>()
                .Include(p => p.Tecnico)
                .Include(p => p.Trampa)
                .ThenInclude(p => p.Tipos)
                .Where(p => p.Id == id)
                .AsNoTracking()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync();
        }


        public async Task<ICollection<ReportInfo>> GetReportSaleAsync(DateTime dateStart, DateTime dateEnd)
        {
            var query = await Context.Database.GetDbConnection()
                .QueryAsync<ReportInfo>(sql: "uspReportSales", commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        DateStart = dateStart,
                        DateEnd = dateEnd
                    });

            return query.ToList();
        }

    }
}
