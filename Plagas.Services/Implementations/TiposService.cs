using AutoMapper;
using Microsoft.Extensions.Logging;
using plagas.Dto;
using plagas.Dto.Request;
using plagas.Dto.Response;
using Plagas.Entities;
using Plagas.Repositories;
using Plagas.Services.Interfaces;

namespace Plagas.Services.Implementations;

public class TiposService : ITiposService
{
    private readonly ITiposRepository _repository;
    private readonly ILogger<TiposService> _logger;
    private readonly IMapper _mapper;

    public TiposService(ITiposRepository repository, ILogger<TiposService> logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<BaseResponseGeneric<ICollection<TiposDtoResponse>>> ListAsync()
    {
        var response = new BaseResponseGeneric<ICollection<TiposDtoResponse>>();

        try
        {
            // Codigo
            response.Data = _mapper.Map<ICollection<TiposDtoResponse>>(await _repository.ListAsync());
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al Listar los Tipos";
            _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }

        return response;

    }

    public async Task<BaseResponseGeneric<TiposDtoResponse>> FindByIdAsync(int id)
    {
        var response = new BaseResponseGeneric<TiposDtoResponse>();

        try
        {
            // Codigo
            response.Data = _mapper.Map<TiposDtoResponse>(await _repository.FindByIdAsync(id));
            response.Success = response.Data != null;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al leer el Tipo";
            _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }

        return response;

    }

    public async Task<BaseResponseGeneric<int>> AddAsync(TiposDtoRequest request)
    {
        var response = new BaseResponseGeneric<int>();

        try
        {
            // Codigo
            response.Data = await _repository.AddAsync(_mapper.Map<Tipos>(request));
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al agregar un Tipo";
            _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }

        return response;

    }

    public async Task<BaseResponse> UpdateAsync(int id, TiposDtoRequest request)
    {
        var response = new BaseResponse();

        try
        {
            // Codigo
            var entity = await _repository.FindByIdAsync(id);
            if (entity is null)
            {
                response.ErrorMessage = "No se encontro el registro el registro";
                return response;
            }

            _mapper.Map(request, entity);

            await _repository.UpdateAsync();

            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al actualizar Tipo";
            _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }

        return response;

    }

    public async Task<BaseResponse> DeleteAsync(int id)
    {
        var response = new BaseResponse();

        try
        {
            // Codigo
            await _repository.DeleteAsync(id);
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al eliminar";
            _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }

        return response;

    }
}