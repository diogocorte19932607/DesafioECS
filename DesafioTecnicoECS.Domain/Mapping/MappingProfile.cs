using AutoMapper;
using DesafioTecnicoECS.Domain.Domain;
using DesafioTecnicoECS.Infra.Entity;

namespace DesafioTecnicoECS.Domain.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductModel, Product>().ReverseMap();
            CreateMap<Logradouro, LogradouroModel>().ReverseMap();
            CreateMap<ClienteModel, Cliente>().ReverseMap();

        }
    }
}
