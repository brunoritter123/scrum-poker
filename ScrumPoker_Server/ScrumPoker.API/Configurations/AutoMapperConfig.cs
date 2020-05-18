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
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
