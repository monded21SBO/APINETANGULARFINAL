using Plagas.Entities;
using Plagas.Entities.Infos;


namespace Plagas.Repositories;

    public interface IVisitaRepository : IRepositoryBase<Visita>
    {

        Task CreateTransactionAsync();

        Task RollBackAsync();

    Task<ICollection<ReportInfo>> GetReportSaleAsync(DateTime dateStart, DateTime dateEnd);

}

