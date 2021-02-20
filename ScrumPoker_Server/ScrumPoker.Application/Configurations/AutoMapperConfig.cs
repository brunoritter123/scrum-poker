using AutoMapper;
using ScrumPoker.Application.DTOs.InputModels;
using ScrumPoker.Application.DTOs.ViewModels;
using ScrumPoker.Domain.Entities.SalaEntity;
using ScrumPoker.Domain.Entities.UsuarioEntity;
using System.Linq;

namespace ScrumPoker.Application.Configurations
{
    public static class ApplicationAutoMapperConfig
    { 
        public static Profile AddApplicationAutoMapperConfig(this Profile profile)
        {
            profile.CreateMap<Sala, SalaViewModel>()
                .ForMember(dest => dest.Administradores, opt => {
                    opt.MapFrom(src => src.Participantes.Where(x => !x.Jogador).ToList());
                })
                .ForMember(dest => dest.Jogadores, opt => {
                    opt.MapFrom(src => src.Participantes.Where(x => x.Jogador).ToList());
                });
            profile.CreateMap<SalaViewModel, Sala>()
                .ForMember(dest => dest.Participantes, opt => {
                    opt.MapFrom(src => src.Administradores);
                })
                .ForMember(dest => dest.Participantes, opt => {
                    opt.MapFrom(src => src.Jogadores);
                });
            profile.CreateMap<SalaConfiguracao, SalaConfiguracaoViewModel>().ReverseMap();
            profile.CreateMap<Participante, ParticipanteViewModel>().ReverseMap();
            profile.CreateMap<Participante, IncluirParticipanteSalaInputModel>().ReverseMap();
            profile.CreateMap<Carta, CartaViewModel>().ReverseMap();
            profile.CreateMap<Perfil, PerfilViewModel>()
                .ForMember(dest => dest.Email, opt => {
                    opt.MapFrom(src => src.User.Email);
                });
            profile.CreateMap<PerfilViewModel, Perfil>(); 
            profile.CreateMap<PerfilAlteracaoInputModel, Perfil>().ReverseMap();
            profile.CreateMap<SalaConfiguracao, AlterarConfiguracaoSalaInputModel>().ReverseMap();
            profile.CreateMap<Carta, AlterarConfiguracaoSalaCartaInputModel>().ReverseMap();
            profile.CreateMap<SalaConfiguracao, AlterarConfiguracaoSalaCartaInputModel>().ReverseMap();
            profile.CreateMap<Participante, VotoViewModel>()
                .ForMember(dest => dest.JogadorId, opt => {
                    opt.MapFrom(src => src.Id);
                })
                .ForMember(dest => dest.ValorVoto, opt => {
                    opt.MapFrom(src => src.VotoCartaValor);
                })
                .ForMember(dest => dest.SalaId, opt => {
                    opt.MapFrom(src => src.SalaId);
                });

            return profile;
        }
    }
}
