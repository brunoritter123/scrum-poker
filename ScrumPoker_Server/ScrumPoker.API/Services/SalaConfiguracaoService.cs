using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ScrumPoker.Domain.Interfaces.Repositories;
using ScrumPoker.Domain.Models;
using System.Threading.Tasks;
using AutoMapper;
using ScrumPoker.API.Dtos;
using ScrumPoker.API.Interfaces;
using System.Collections.Generic;

namespace ScrumPoker.API.Services
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

        public async Task<SalaConfiguracaoDto> BuscarPorIdAsync(long id)
        {
            SalaConfiguracao sala = await _repo.BuscarPorIdAsync(id);

            if (sala is null)
                throw new Exception("Não foi encontrado a Configuracao da Sala");

            return _mapper.Map<SalaConfiguracaoDto>(sala);
        }

        public async Task<SalaConfiguracaoDto> AlterarAsync(SalaConfiguracaoDto salaConfiguracaoDto)
        {
            SalaConfiguracao salaConfig = await _repo.BuscarPorIdAsync(salaConfiguracaoDto.Id);
            if (salaConfig is null) throw new Exception("Não foi encontrado o registro");

            salaConfig.Cartas = salaConfig.Cartas.ToList();
            _mapper.Map(salaConfiguracaoDto, salaConfig);
            await _repo.AlterarAsync(salaConfig);

            return _mapper.Map<SalaConfiguracaoDto>(salaConfig);
        }
    }
}