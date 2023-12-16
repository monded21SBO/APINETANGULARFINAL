using plagas.Dto;
using Plagas.Dto;
using Plagas.Dto.Request;
using Plagas.Dto.Response;




namespace Plagas.Services.Interfaces;


public interface ITrampasService
{
    Task<BaseResponsePagination<TrampasDtoResponse>> ListAsync(TrampasSearch search , CancellationToken cancellationToken = default);

    Task<BaseResponseGeneric<TrampasDtoResponse>> FindByIdAsync(int id);

    Task<BaseResponseGeneric<int>> AddAsync(TrampasDtoRequest request);

    Task<BaseResponse> UpdateAsync(int id, TrampasDtoRequest request);

    Task<BaseResponse> DeleteAsync(int id);



}
