using plagas.Dto.Request;
using plagas.Dto.Response;
using plagas.Dto;

namespace Plagas.Services.Interfaces;

public interface ITiposService
{
    Task<BaseResponseGeneric<ICollection<TiposDtoResponse>>> ListAsync();

    Task<BaseResponseGeneric<TiposDtoResponse>> FindByIdAsync(int id);

    Task<BaseResponseGeneric<int>> AddAsync(TiposDtoRequest request);

    Task<BaseResponse> UpdateAsync(int id, TiposDtoRequest request);

    Task<BaseResponse> DeleteAsync(int id);
}