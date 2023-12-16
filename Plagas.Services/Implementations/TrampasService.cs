using AutoMapper;
using Microsoft.Extensions.Logging;
using plagas.Dto;
using Plagas.Dto;
using Plagas.Dto.Request;
using Plagas.Dto.Response;
using Plagas.Entities;
using Plagas.Repositories;
using Plagas.Services.Interfaces;
using System;

namespace Plagas.Services.Implementations;

public class TrampasService : ITrampasService
{

    private readonly ITrampasRepository _repository;
    private readonly ILogger<TrampasService> _logger;
    private readonly IMapper _mapper;
    private readonly IFileUploader _fileUploader;

    public TrampasService(ITrampasRepository repository, ILogger<TrampasService> logger, IMapper mapper, IFileUploader fileUploader)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _fileUploader = fileUploader;
    }

    public async Task<BaseResponsePagination<TrampasDtoResponse>> ListAsync(TrampasSearch search, CancellationToken cancellationToken = default)
    {
        var response = new BaseResponsePagination<TrampasDtoResponse>();

        try
        {
            var tupla = await _repository.ListAsync(predicate: c => c.Nombre.Contains(search.Nombre ?? string.Empty),
             selector: p => _mapper.Map<TrampasDtoResponse>(p),
             orderby: p => p.Nombre,
             page: search.Page,
             rows: search.Rows,
             relationships: "Tipos");

            response.Data = tupla.Collection;
            response.TotalPages = Utilities.GetTotalPages(tupla.Total, search.Rows);
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al Listar las Trampas";
            _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }

        return response;
    }

    public async Task<BaseResponseGeneric<TrampasDtoResponse>> FindByIdAsync(int id)
    {
        var response = new BaseResponseGeneric<TrampasDtoResponse>();

        try
        {
            // Codigo

            response.Data = _mapper.Map<TrampasDtoResponse>(await _repository.FindByIdAsync(id));
            response.Success = response.Data != null;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al seleccionar La Trampa";
            _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }

        return response;

    }

    public async Task<BaseResponseGeneric<int>> AddAsync(TrampasDtoRequest request)
    {
        var response = new BaseResponseGeneric<int>();

        try
        {
            // Codigo
            var trampa = _mapper.Map<Trampas>(request);

            trampa.ImageUrl = await _fileUploader.UploadFileAsync(request.Base64Image, request.FileName);

            response.Data = await _repository.AddAsync(trampa);
            response.Success = true;

        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al agregar una Trampa";
            _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }

        return response;

    }

    public async Task<BaseResponse> UpdateAsync(int id, TrampasDtoRequest request)
    {
        var response = new BaseResponse();

        try
        {
          
            var entity = await _repository.FindByIdAsync(id); 
            if (entity is null)
            {
                response.ErrorMessage = "El registro no se encuentra";
                return response;
            }

            _mapper.Map(request, entity);

            if (!string.IsNullOrEmpty(request.Base64Image))
            {
                entity.ImageUrl = await _fileUploader.UploadFileAsync(request.Base64Image, request.FileName);
            }


            await _repository.UpdateAsync();

            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al actualizar una Trampa";
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
            response.ErrorMessage = "Error al eliminar Trampa";
            _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }

        return response;

    }







}
