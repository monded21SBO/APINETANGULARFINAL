using System.Globalization;
using AutoMapper;
using Plagas.Dto.Request;
using Plagas.Dto.Response;
using Plagas.Entities;

namespace Plagas.Services.Profiles
{
    public class VisitaProfile : Profile

    {
        private static readonly CultureInfo Culture = new("es-PE");
        public VisitaProfile()
        {
            CreateMap<VisitaDtoRequest, Visita>();

            // Mapeo de Visitas a VisitasDtoResponse
            CreateMap<Visita, VisitaDtoResponse>()
                .ForMember(d => d.VisitaId, o => o.MapFrom(x => x.Id))
                .ForMember(d => d.FechaInstalacion, o => o.MapFrom(x => x.Trampa.FechaInstalacion.ToString("D", Culture)))
                .ForMember(d => d.Trampas, o => o.MapFrom(x => x.Trampa.Tipos.Name))
                .ForMember(d => d.ImageUrl, o => o.MapFrom(x => x.Trampa.ImageUrl))
                .ForMember(d => d.Nombre, o => o.MapFrom(x => x.Trampa.Nombre))
                .ForMember(d => d.OperationNumber, o => o.MapFrom(x => x.OperationNumber))
                .ForMember(d => d.FullName, o => o.MapFrom(x => x.Tecnico.FullName))

                .ForMember(d => d.FechaVisita, o => o.MapFrom(x => x.FechaVisita.ToString("dd/MM/yyyy HH:mm", Culture)))
                .ForMember(d => d.Comentarios, o => o.MapFrom(x => x.Comentarios));










    }

    }
}
