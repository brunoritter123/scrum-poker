using AutoMapper;
using ScrumPoker.Application.DTOs.InputModels;
using ScrumPoker.Application.DTOs.ViewModels;
using ScrumPoker.Domain.Entities.SalaEntity;
using ScrumPoker.Application.Interfaces.ApplicationServices;
using ScrumPoker.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace ScrumPoker.Application.Services
{
    public class ParticipanteService : IParticipanteService
    {
        private readonly IParticipanteRepository _repo;
        private readonly IMapper _mapper;

        public ParticipanteService(IParticipanteRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ParticipanteViewModel> IncluirOuAlterarAsync(IncluirParticipanteSalaInputModel participanteDto)
        {
            Participante participante = await _repo.BuscarPorIdAsync(participanteDto.Id);
            if (participante is null)
            {
                participante = _mapper.Map<Participante>(participanteDto);
                participante = await _repo.IncluirAsync(participante);
            }
            else
            {
                _mapper.Map(participanteDto, participante);
                await _repo.AlterarAsync(participante);
            }

            return _mapper.Map<ParticipanteViewModel>(participante);
        }

        public async Task<ParticipanteViewModel> Desconectar(string participanteId)
        {
            var participante =  await _repo.BuscarPorIdAsync(participanteId);
            if (participante != null)
            {
                participante.ConexaoId = null;
                participante.Online = false;
                await _repo.AlterarAsync(participante);
                return _mapper.Map<ParticipanteViewModel>(participante);
            }
            else
                return null;
        }

        public async Task RemoverParticipante(string participanteId)
        {
            await _repo.RemoverAsync(participanteId);
        }

        public async Task<ParticipanteViewModel> Conectar(string conexaoId, string participanteId)
        {
            var participante = await _repo.BuscarPorIdAsync(participanteId);
            if (participante is null)
                throw new ApplicationException($"Participante '{participanteId}' n�o foi encontrado");

            participante.ConexaoId = conexaoId;
            participante.Online = true;
            participante = await _repo.AlterarAsync(participante);

            return _mapper.Map<ParticipanteViewModel>(participante);
        }

        public async Task<ParticipanteViewModel> BuscarParticipantePorId(string id)
        {
            return _mapper.Map<ParticipanteViewModel>(await _repo.BuscarPorIdAsync(id));
        }

        public async Task<VotoViewModel> VotoParticipante(string participanteId, string votoValor)
        {
            var participante = await _repo.BuscarPorIdAsync(participanteId);
            if (participante is null)
                throw new ApplicationException($"Participante '{participanteId}' n�o foi encontrado");

            participante.VotoCartaValor = votoValor;
            participante = await _repo.AlterarAsync(participante);

            return _mapper.Map<VotoViewModel>(participante);
        }      
    }
}