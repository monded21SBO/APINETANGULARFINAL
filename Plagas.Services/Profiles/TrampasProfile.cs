using AutoMapper;
using Plagas.Dto.Request;
using Plagas.Dto.Response;
using Plagas.Entities;
using Plagas.Entities.Infos;
using System;

namespace Plagas.Services.Profiles
{
    public class TrampasProfile : Profile
    {
        public TrampasProfile() {

            // Las conversiones son de izquierda a derecha
            CreateMap<TrampasInfo, TrampasDtoResponse>();

            CreateMap<Trampas, TrampasDtoResponse>()
                .ForMember(d => d.FechaInstalacion, o => o.MapFrom(x => x.FechaInstalacion.ToShortDateString()))
                .ForMember(d => d.Status, o => o.MapFrom(x => x.Status ? "Activo" : "Inactivo"))
                .ForMember(d => d.Tipos, o => o.MapFrom(x => x.Tipos.Name));
            CreateMap<TrampasDtoRequest, Trampas>()
                .ForMember(d => d.FechaInstalacion, o => o.MapFrom(x => Convert.ToDateTime($"{x.FechaInstalacion}")));

        }
    }
}
