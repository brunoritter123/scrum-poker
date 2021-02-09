using AutoMapper;
using ScrumPoker.Application.DTOs.ViewModels;
using ScrumPoker.Domain.Entities.SalaEntity;
using ScrumPoker.Application.Interfaces.ApplicationServices;
using ScrumPoker.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<SalaViewModel> IncluirSalaPadraoAsync(string id)
        {
            var sala = await _repo.BuscarPorIdAsync(id);

            if (sala != null)
                return _mapper.Map<SalaViewModel>(sala);

            sala = await _repo.IncluirAsync(GerarSalaPadrao(id));

            return _mapper.Map<SalaViewModel>(sala);
        }

        public async Task<SalaViewModel> IncluiAsync(SalaViewModel SalaViewModel)
        {
            if (await _repo.ExisteSalaAsync(SalaViewModel.Id))
                throw new Exception($"Id da sala {SalaViewModel.Id}, já existe no banco de dados");

            var sala = await _repo.IncluirAsync(_mapper.Map<Sala>(SalaViewModel));
            return _mapper.Map<SalaViewModel>(sala);
        }

        public async Task<SalaViewModel> AlterarAsync(SalaViewModel SalaViewModel)
        {
            var sala = await _repo.BuscarPorIdAsync(SalaViewModel.Id);
            if (sala is null) throw new Exception("Não foi encontrado o registro");

            sala.Configuracao.Cartas = sala.Configuracao.Cartas.ToList();
            _mapper.Map(SalaViewModel, sala);
            await _repo.AlterarAsync(sala);

            return _mapper.Map<SalaViewModel>(sala);
        }

        public async Task ExcluirCartasAsync(string id)
        {
            if ( !(await _repo.ExisteSalaAsync(id)) )
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

        public async Task<SalaViewModel> ResetarSala(string salaId)
        {
            Sala sala = await _repo.BuscarPorIdAsync(salaId);
            sala.JogoFinalizado = false;
            sala.Titulo = null;
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
            sala = await _repo.AlterarAsync(sala);

            return _mapper.Map<SalaViewModel>(sala);
        }
    }
}