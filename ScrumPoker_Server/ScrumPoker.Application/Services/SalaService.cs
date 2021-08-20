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
                throw new ApplicationException($"Sala '{id}' não foi encontrada");

            return _mapper.Map<SalaViewModel>(sala);
        }

        public async Task<SalaViewModel> GerarSalaPadraoAsync(GerarSalaPadraoInputModel gerarSalaInput)
        {
            var sala = await _repo.BuscarPorIdAsync(gerarSalaInput.Id);
            if (sala != null)
                return _mapper.Map<SalaViewModel>(sala);

            await _repo.LimparSalasNaoUsadas();
            sala =_mapper.Map<Sala>(gerarSalaInput);
            sala = await _repo.IncluirAsync(sala);

            return _mapper.Map<SalaViewModel>(sala);
        }

        public async Task<SalaViewModel> ResetarSala(string salaId)
        {
            Sala sala = await _repo.BuscarPorIdAsync(salaId);
            if (sala is null)
                throw new ApplicationException($"Sala '{salaId}' não foi encontrada");

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
            if (sala is null)
                throw new ApplicationException($"Sala '{salaId}' não foi encontrada");

            sala.JogoFinalizado = true;
            sala.UltimaDataDeUtilizacao = DateTime.Now;
            sala = await _repo.AlterarAsync(sala);

            return _mapper.Map<SalaViewModel>(sala);
        }
    }
}