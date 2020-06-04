using System.Net.Security;
using AutoMapper;
using ScrumPoker.API.Dtos;
using ScrumPoker.Domain.Identity;
using ScrumPoker.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace ScrumPoker.API.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(x => x.AllowNullCollections = true);
        }

        public AutoMapperConfig()
        {
            CreateMap<Sala, SalaDto>().ReverseMap();
            CreateMap<Carta, CartaDto>().ReverseMap();
            CreateMap<User, UserRegistroDto>()
                .ForMember(dest => dest.Nome, opt => {
                    opt.MapFrom(src => src.Perfil.Nome);
                });
            CreateMap<UserRegistroDto, User>()
                .ForMember(dest => dest.Perfil, opt => {
                    opt.MapFrom(src => new Perfil() {
                        Nome = src.Nome
                    });
                });
            CreateMap<Perfil, PerfilDto>()
                .ForMember(dest => dest.Email, opt => {
                    opt.MapFrom(src => src.User.Email);
                });
            CreateMap<PerfilDto, Perfil>();
        }
    }
}
