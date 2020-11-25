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
    public class SalaService : ISalaService
    {
        private readonly ISalaRepository _repo;
        private readonly IMapper _mapper;

        public SalaService(ISalaRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<SalaDto> ObterPorIdAsync(string id)
        {
            var sala = await _repo.BuscarPorIdAsync(id);

            if (sala is null)
                throw new Exception("Sala não foi encontrada");

            return _mapper.Map<SalaDto>(sala);
        }

        public async Task<SalaDto> IncluirSalaPadraoAsync(string id)
        {
            var sala = await _repo.BuscarPorIdAsync(id);

            if (sala != null)
                return _mapper.Map<SalaDto>(sala);

            sala = await _repo.IncluirAsync(GerarSalaPadrao(id));

            return _mapper.Map<SalaDto>(sala);
        }

        public async Task<SalaDto> IncluiAsync(SalaDto salaDto)
        {
            if (await _repo.ExisteEntityAsync(salaDto.Id))
                throw new Exception($"Id da sala {salaDto.Id}, já existe no banco de dados");

            var sala = await _repo.IncluirAsync(_mapper.Map<Sala>(salaDto));
            return _mapper.Map<SalaDto>(sala);
        }

        public async Task<SalaDto> AlterarAsync(SalaDto salaDto)
        {
            var sala = await _repo.BuscarPorIdAsync(salaDto.Id);
            if (sala is null) throw new Exception("Não foi encontrado o registro");

            sala.Configuracao.Cartas = sala.Configuracao.Cartas.ToList();
            _mapper.Map(salaDto, sala);
            await _repo.AlterarAsync(sala);

            return _mapper.Map<SalaDto>(sala);
        }

        public async Task ExcluirCartasAsync(string id)
        {
            if ( !(await _repo.ExisteEntityAsync(id)) )
                throw new Exception("Não foi encontrado o registro");

            await _repo.ExcluirCartasAsync(id);

            return;
        }

        private Sala GerarSalaPadrao(string id)
        {
            return new Sala()
                {
                    Id = id,
                    JogoFinalizado = false,
                    Configuracao = new SalaConfiguracao()
                    {
                        SalaId = id,
                        JogadorFinalizaJogo = false,
                        JogadorResetaJogo = false,
                        JogadorRemoveJogador = false,
                        JogadorRemoveAdministrador = true,

                        Cartas = new List<Carta>()
                        {
                            new Carta()
                            {
                                Ordem = 1,
                                Value = "01",
                                Especial = false
                            },
                            new Carta()
                            {
                                Ordem = 2,
                                Value = "02",
                                Especial = false
                            },
                            new Carta()
                            {
                                Ordem = 3,
                                Value = "03",
                                Especial = false
                            },
                            new Carta()
                            {
                                Ordem = 4,
                                Value = "05",
                                Especial = false
                            },
                            new Carta()
                            {
                                Ordem = 5,
                                Value = "08",
                                Especial = false
                            },
                            new Carta()
                            {
                                Ordem = 6,
                                Value = "13",
                                Especial = false
                            },
                            new Carta()
                            {
                                Ordem = 7,
                                Value = "21",
                                Especial = false
                            },
                            new Carta()
                            {
                                Ordem = 8,
                                Value = "34",
                                Especial = false
                            },
                            new Carta()
                            {
                                Ordem = 9,
                                Value = "55",
                                Especial = false
                            },
                            new Carta()
                            {
                                Ordem = 10,
                                Value = "?",
                                Especial = true
                            }
                        }
                    }
                };
        }
    }
}