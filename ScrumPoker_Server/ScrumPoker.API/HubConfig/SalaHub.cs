using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ScrumPoker.Domain.Models;
using ScrumPoker.API.Dtos;
using ScrumPoker.API.Services;
using ScrumPoker.API.Interfaces;

namespace ScrumPoker.API.HubConfig
{
    public class SalaHub : Hub
    {
        private readonly ISalaConfiguracaoService _salaConfiguracaoService;
        private readonly ISalaParticipanteService _participanteService;

        public SalaHub(
            ISalaConfiguracaoService salaConfiguracaoService,
            ISalaParticipanteService participanteService)
        {
            _salaConfiguracaoService = salaConfiguracaoService;
            _participanteService = participanteService;
        }
        public override async Task OnConnectedAsync()
        {
            await IncluirParticipante();
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var participanteDto = await _participanteService.Desconectar(Context.UserIdentifier);
            if(participanteDto != null)
                await AtualizarParticipantesSala(participanteDto.SalaId, participanteDto.Jogador);

            await base.OnDisconnectedAsync(exception);
        }

        private async Task IncluirParticipante()
        {
            var participanteDto = await _participanteService.Conectar(Context.ConnectionId, Context.UserIdentifier);
            await Groups.AddToGroupAsync(Context.ConnectionId, participanteDto.SalaId);
            await AtualizarParticipantesSala(participanteDto.SalaId, participanteDto.Jogador);
        }

        public async Task RemoverParticipante(string participanteId, string participanteQueRemoveu)
        {
            var participanteDto = await _participanteService.BuscarParticipantePorId(participanteId);
            await _participanteService.RemoverParticipante(participanteId);
            await Clients.User(participanteId).SendAsync("ParticipanteRemovido", participanteQueRemoveu);
            await AtualizarParticipantesSala(participanteDto.SalaId, participanteDto.Jogador);
        }

        private async Task AtualizarParticipantesSala(string salaId, bool jogador)
        {
            if (jogador)
            {
                var jogadores = await _participanteService.BuscarJogadoresPorSalaId(salaId);
                await Clients.Group(salaId).SendAsync("ReceberJogadores", jogadores);
            }
            else
            {
                var administradores = await _participanteService.BuscarAdministradoresPorSalaId(salaId);
                await Clients.Group(salaId).SendAsync("ReceberAdministradores", administradores);
            }
        }

        public async Task EnviarConfiguracaoSala(SalaConfiguracaoDto salaConfiguracaoDto)
        {
            SalaConfiguracaoDto salaConfiguracaoAlterada = await _salaConfiguracaoService.AlterarAsync(salaConfiguracaoDto);
            await Clients.Group(salaConfiguracaoDto.SalaId).SendAsync("ReceberConfiguracaoSala", salaConfiguracaoAlterada);
        }

        public async Task EnviarVoto(String votoValor)
        {
            var participanteDto = await _participanteService.VotoParticipante(Context.UserIdentifier, votoValor);
            await AtualizarParticipantesSala(participanteDto.SalaId, participanteDto.Jogador);
        }
    }
}