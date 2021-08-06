using AutoMapper;
using ScrumPoker.Application.DTOs.InputModels;
using ScrumPoker.Application.DTOs.ViewModels;
using ScrumPoker.Domain.Entities.SalaEntity;
using ScrumPoker.Application.Interfaces.ApplicationServices;
using ScrumPoker.Domain.Interfaces.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPoker.Application.Services
{
    public class SalaConfiguracaoService : ISalaConfiguracaoService
    {
        private readonly ISalaConfiguracaoRepository _repo;
        private readonly IMapper _mapper;

        public SalaConfiguracaoService(ISalaConfiguracaoRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<SalaConfiguracaoViewModel> BuscarPorIdAsync(long id)
        {
            SalaConfiguracao sala = await _repo.BuscarPorIdAsync(id);

            if (sala is null)
                throw new ApplicationException($"Configuração Sala Id: '{id}' não foi encontrado");

            return _mapper.Map<SalaConfiguracaoViewModel>(sala);
        }

        public async Task<SalaConfiguracaoViewModel> AlterarAsync(AlterarConfiguracaoSalaInputModel salaConfiguracaoViewModel)
        {
            SalaConfiguracao salaConfig = await _repo.BuscarPorIdAsync(salaConfiguracaoViewModel.Id);
            if (salaConfig is null) 
                throw new ApplicationException($"Configuração Sala Id: '{salaConfiguracaoViewModel.Id}' não foi encontrado");

            salaConfig.Cartas = salaConfig.Cartas.ToList();
            _mapper.Map(salaConfiguracaoViewModel, salaConfig);
            await _repo.AlterarAsync(salaConfig);

            return _mapper.Map<SalaConfiguracaoViewModel>(salaConfig);
        }

    }
}