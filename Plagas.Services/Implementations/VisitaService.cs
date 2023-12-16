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

namespace Plagas.Services.Implementations
{
    public  class VisitaService : IVisitaService
    {
        private readonly IVisitaRepository _repository;
        private readonly ILogger<VisitaService> _logger;
        private readonly IMapper _mapper;
        private readonly ITrampasRepository _trampaRepository;
        private readonly ITecnicoRepository _tecnicoRepository;


        public VisitaService(IVisitaRepository repository, ILogger<VisitaService> logger, IMapper mapper, ITrampasRepository trampaRepository,
        ITecnicoRepository tecnicoRepository)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _trampaRepository = trampaRepository;
            _tecnicoRepository = tecnicoRepository;
        }



        public async Task<BaseResponseGeneric<int>> AddAsync(string Email, VisitaDtoRequest request)
        {
            var response = new BaseResponseGeneric<int>();

            try
            {
                // Codigo
                await _repository.CreateTransactionAsync();
                var entity = _mapper.Map<Visita>(request);

                var Tecnico = await _tecnicoRepository.FindByEmailAsync(request.Email);
                if (Tecnico is null)
                {
                    Tecnico = new Tecnicos()
                    {
                        Email = request.Email,
                        FullName = request.FullName
                    };
                    Tecnico.Id = await _tecnicoRepository.AddAsync(Tecnico);
                }

                entity.TecnicoId = Tecnico.Id;




                var trampa = await _trampaRepository.FindByIdAsync(request.TrampaId);
                if (trampa is null)
                    throw new InvalidOperationException($"El concierto con el Id {request.TrampaId} no existe!");





                await _repository.AddAsync(entity);
                await _repository.UpdateAsync();

                response.Data = entity.Id;
                response.Success = true;

                _logger.LogInformation("Se creo correctamente la venta para {email}", request.Email);
            }
            catch (InvalidOperationException ex)
            {

                await _repository.RollBackAsync();
                response.ErrorMessage = ex.Message;

                _logger.LogWarning(ex, "{ErrorMessage}", response.ErrorMessage);
            }
            catch (Exception ex)
            {
                //await _repository.RollBackAsync();
                response.ErrorMessage = "Error al crear la venta";
                _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }

            return response;

        }






        public async Task<BaseResponseGeneric<VisitaDtoResponse>> FindByIdAsync(int id)
        {
            var response = new BaseResponseGeneric<VisitaDtoResponse>();

            try
            {

                var visita = await _repository.FindByIdAsync(id);
                response.Data = _mapper.Map<VisitaDtoResponse>(visita);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al seleccionar la visita";
                _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }

            return response;

        }


        public async Task<BaseResponsePagination<VisitaDtoResponse>> ListAsync(VisitaByDateSearch search)
        {
            var response = new BaseResponsePagination<VisitaDtoResponse>();

            try
            {
                // Codigo
                var dateInit = Convert.ToDateTime(search.DateStart);
                var dateEnd = Convert.ToDateTime(search.DateEnd);


                var tupla = await _repository.ListAsync(predicate: s => s.FechaVisita >= dateInit && s.FechaVisita <= dateEnd,
                    selector: p => _mapper.Map<VisitaDtoResponse>(p),
                    orderby: x => x.OperationNumber,
                    page: search.Page,
                    rows: search.Rows,
                    relationships: "Tecnico,Trampa,Trampa.Tipos");

                response.Data = tupla.Collection;
                response.TotalPages = Utilities.GetTotalPages(tupla.Total, search.Rows);

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al filtrar las Visitas por fecha";
                _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }

            return response;

        }


        public async Task<BaseResponsePagination<VisitaDtoResponse>> ListAsync(string email, VisitaByTitleSearch search)
        {
            var response = new BaseResponsePagination<VisitaDtoResponse>();

            try
            {
                // Codigo

                var tupla = await _repository.ListAsync(predicate: s => s.Tecnico.Email == email
                                                                        && s.Trampa.Nombre.Contains(search.Nombre ?? string.Empty),
                    selector: p => _mapper.Map<VisitaDtoResponse>(p),
                    orderby: x => x.FechaVisita,
                    page: search.Page,
                    rows: search.Rows,
                    relationships: "Tecnico,Trampa,Trampa.Tipos");

                response.Data = tupla.Collection;
                response.TotalPages = Utilities.GetTotalPages(tupla.Total, search.Rows);

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al filtrar las Visitas del usuario";
                _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }

            return response;
        }

        public async Task<BaseResponseGeneric<ICollection<ReportDtoResponse>>> GetReportSaleAsync(DateTime dateStart, DateTime dateEnd)
        {
            var response = new BaseResponseGeneric<ICollection<ReportDtoResponse>>();

            try
            {
                // Codigo
                var list = await _repository.GetReportSaleAsync(dateStart, dateEnd);
                response.Data = _mapper.Map<ICollection<ReportDtoResponse>>(list);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al obtener los datos del reporte";
                _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }

            return response;

        }








    }

}
