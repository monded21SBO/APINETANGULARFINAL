using plagas.Dto;
using Plagas.Dto;
using Plagas.Dto.Request;
using Plagas.Dto.Response;

namespace Plagas.Services.Interfaces
{
    public interface IVisitaService
    {

        Task<BaseResponseGeneric<int>> AddAsync(string email, VisitaDtoRequest request);

        Task<BaseResponseGeneric<VisitaDtoResponse>> FindByIdAsync(int id);


        Task<BaseResponsePagination<VisitaDtoResponse>> ListAsync(VisitaByDateSearch search);

        Task<BaseResponsePagination<VisitaDtoResponse>> ListAsync(string email, VisitaByTitleSearch search);


        Task<BaseResponseGeneric<ICollection<ReportDtoResponse>>> GetReportSaleAsync(DateTime dateStart, DateTime dateEnd);
    }
}
