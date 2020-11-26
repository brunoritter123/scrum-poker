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
using Microsoft.AspNetCore.SignalR;

namespace ScrumPoker.API.Services
{
    public class SalaParticipanteService : ISalaParticipanteService
    {
        private readonly ISalaParticipanteRepository _repo;
        private readonly IMapper _mapper;

        public SalaParticipanteService(ISalaParticipanteRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SalaParticipanteDto>> BuscarAdministradoresPorSalaId(string salaId)
        {
            var administradores = await _repo.BuscarAdministradoresPorSalaAsync(salaId);
            return _mapper.Map<IEnumerable<SalaParticipanteDto>>(administradores);
        }

        public async Task<IEnumerable<SalaParticipanteDto>> BuscarJogadoresPorSalaId(string salaId)
        {
            var jogadores = await _repo.BuscarJogadoresPorSalaAsync(salaId);
            return _mapper.Map<IEnumerable<SalaParticipanteDto>>(jogadores);
        }

        public async Task<SalaParticipanteDto> IncluirOuAlterarAsync(PostParticipanteDto participanteDto)
        {
            SalaParticipante participante = await _repo.BuscarPorIdAsync(participanteDto.Id);
            if (participante is null)
            {
                participante = _mapper.Map<SalaParticipante>(participanteDto);
                participante = await _repo.IncluirAsync(participante);
            }
            else
            {
                _mapper.Map(participanteDto, participante);
                await _repo.AlterarAsync(participante);
            }

            return _mapper.Map<SalaParticipanteDto>(participante);
        }

        public async Task<SalaParticipanteDto> Desconectar(string participanteId)
        {
            var participante =  await _repo.BuscarPorIdAsync(participanteId);
            if (participante != null)
            {
                participante.ConexaoId = null;
                participante.Online = false;
                await _repo.AlterarAsync(participante);
                return _mapper.Map<SalaParticipanteDto>(participante);
            }
            else
                return null;
        }

        public async Task RemoverParticipante(string participanteId)
        {
            await _repo.RemoverAsync(participanteId);
        }

        public async Task<SalaParticipanteDto> Conectar(string conexaoId, string participanteId)
        {
            var participante = await _repo.BuscarPorIdAsync(participanteId);
            participante.ConexaoId = conexaoId;
            participante.Online = true;
            participante = await _repo.AlterarAsync(participante);

            return _mapper.Map<SalaParticipanteDto>(participante);
        }

        public async Task<SalaParticipanteDto> BuscarParticipantePorId(string id)
        {
            return _mapper.Map<SalaParticipanteDto>(await _repo.BuscarPorIdAsync(id));
        }

        public async Task<SalaParticipanteDto> VotoParticipante(string participanteId, string votoValor)
        {
            var participante = await _repo.BuscarPorIdAsync(participanteId);
            participante.VotoCartaValor = votoValor;
            participante = await _repo.AlterarAsync(participante);

            return _mapper.Map<SalaParticipanteDto>(participante);
        }

        public async Task<IEnumerable<SalaParticipanteDto>> BuscarParticipantesPorSala(string salaId)
        {
            var participantes = await _repo.BuscarParticipantesPorSalaAsync(salaId);
            return _mapper.Map<IEnumerable<SalaParticipanteDto>>(participantes);
        }
    }
}