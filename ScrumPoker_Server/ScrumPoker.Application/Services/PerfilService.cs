using AutoMapper;
using ScrumPoker.Application.DTOs.InputModels;
using ScrumPoker.Application.DTOs.ViewModels;
using ScrumPoker.Application.Interfaces.ApplicationServices;
using ScrumPoker.Domain.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace ScrumPoker.Application.Services
{
    public class PerfilService : IPerfilService
    {
        private readonly IPerfilRepository _repo;
        private readonly IMapper _mapper;

        public PerfilService(IPerfilRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PerfilViewModel> AlterarAsync(PerfilAlteracaoInputModel perfilAlteracao)
        {
            var perfil = await _repo.BuscarPorIdAsync(perfilAlteracao.Id);

            _mapper.Map(perfilAlteracao, perfil);
            var novoPerfil = await _repo.AlterarAsync(perfil);

            return _mapper.Map<PerfilViewModel>(novoPerfil);
        }

        public async Task<PerfilViewModel> BuscarPorIdAsync(Guid id)
        {
            var perfil = await _repo.BuscarPorIdAsync(id);
            return _mapper.Map<PerfilViewModel>(perfil);
        }
    }
}
