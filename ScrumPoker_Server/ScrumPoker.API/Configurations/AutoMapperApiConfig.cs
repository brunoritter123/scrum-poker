using AutoMapper;
using ScrumPoker.Application.DTOs.ViewModels;
using ScrumPoker.Application.DTOs.InputModels;
using ScrumPoker.Domain.Entities.SalaEntity;
using ScrumPoker.Domain.Entities.UsuarioEntity;
using ScrumPoker.Domain.Identity;
using System.Linq;
using ScrumPoker.API.Dtos;

namespace ScrumPoker.Application.Configurations
{
    public class AutoMapperApiConfig : Profile
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(x => x.AllowNullCollections = true);
        }

        public AutoMapperApiConfig()
        {
            this.AddApplicationAutoMapperConfig();

            CreateMap<User, UserRegistroDto>()
                .ForMember(dest => dest.Nome, opt =>
                {
                    opt.MapFrom(src => src.Perfil.Nome);
                });
            CreateMap<UserRegistroDto, User>()
                .ForMember(dest => dest.Perfil, opt =>
                {
                    opt.MapFrom(src => new Perfil()
                    {
                        Nome = src.Nome
                    });
                });
        }
    }
}
