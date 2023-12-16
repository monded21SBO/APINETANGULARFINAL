using AutoMapper;
using plagas.Dto.Request;
using plagas.Dto.Response;
using Plagas.Entities;

namespace Plagas.Services.Profiles;

public class TiposProfile : Profile
{
    public TiposProfile()
    {
        CreateMap<Tipos, TiposDtoResponse>();

        CreateMap<TiposDtoRequest, Tipos>();
    }
}