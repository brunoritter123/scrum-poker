using AutoMapper;
using ScrumPoker.Application.DTOs.ViewModels;
using ScrumPoker.Domain.Entities.SalaEntity;
using ScrumPoker.Application.Interfaces.ApplicationServices;
using ScrumPoker.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScrumPoker.Application.DTOs.InputModels;

namespace ScrumPoker.Application.Services
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

        public async Task<SalaViewModel> ObterPorIdAsync(string id)
        {
            var sala = await _repo.BuscarPorIdAsync(id);

            if (sala is null)
                throw new Exception("Sala não foi encontrada");

            return _mapper.Map<SalaViewModel>(sala);
        }

        public async Task<SalaViewModel> GerarSalaPadraoAsync(GerarSalaPadraoInputModel gerarSalaInput)
        {
            var sala = await _repo.BuscarPorIdAsync(gerarSalaInput.Id);
            if (sala != null)
                return _mapper.Map<SalaViewModel>(sala);

            await _repo.LimparSalasNaoUsadas();
            sala = await _repo.IncluirAsync(GerarSalaPadrao(gerarSalaInput));

            return _mapper.Map<SalaViewModel>(sala);
        }

        private Sala GerarSalaPadrao(GerarSalaPadraoInputModel gerarSalaInput)
        {
            Sala novaSala = new Sala()
                {
                    Id = gerarSalaInput.Id,
                    JogoFinalizado = false,
                    UltimaDataDeUtilizacao = DateTime.Now,
                };

            if (gerarSalaInput.SalaConfiguracao != null)
            {
                novaSala.Configuracao = _mapper.Map<SalaConfiguracao>(gerarSalaInput.SalaConfiguracao);
            }
            else
            {
                novaSala.Configuracao = new SalaConfiguracao()
                {
                    SalaId = gerarSalaInput.Id,
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
                };
            }

            return novaSala;
        }

        public async Task<SalaViewModel> ResetarSala(string salaId)
        {
            Sala sala = await _repo.BuscarPorIdAsync(salaId);
            sala.JogoFinalizado = false;
            sala.Titulo = null;
            sala.UltimaDataDeUtilizacao = DateTime.Now;
            foreach (var participante in sala.Participantes)
            {
                participante.VotoCartaValor = null;
            }
            sala = await _repo.AlterarAsync(sala);

            return _mapper.Map<SalaViewModel>(sala);
        }

        public async Task<SalaViewModel> FinalizarJogo(string salaId)
        {
            Sala sala = await _repo.BuscarPorIdAsync(salaId);
            sala.JogoFinalizado = true;
            sala.UltimaDataDeUtilizacao = DateTime.Now;
            sala = await _repo.AlterarAsync(sala);

            return _mapper.Map<SalaViewModel>(sala);
        }
    }
}