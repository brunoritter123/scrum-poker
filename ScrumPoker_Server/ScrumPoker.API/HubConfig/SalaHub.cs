using Microsoft.AspNetCore.SignalR;
using ScrumPoker.Application.DTOs.InputModels;
using ScrumPoker.Application.DTOs.ViewModels;
using ScrumPoker.Application.Interfaces.ApplicationServices;
using ScrumPoker.Domain.Entities.SalaEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPoker.API.HubConfig
{
    public class SalaHub : Hub
    {
        private readonly ISalaConfiguracaoService _salaConfiguracaoService;
        private readonly IParticipanteService _participanteService;
        private readonly ISalaService _salaService;

        public SalaHub(
            ISalaConfiguracaoService salaConfiguracaoService,
            IParticipanteService participanteService,
            ISalaService salaService)
        {
            _salaConfiguracaoService = salaConfiguracaoService;
            _participanteService = participanteService;
            _salaService = salaService;
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
            await Clients.User(Context.UserIdentifier).SendAsync("ParticipanteRemovido", "");
            var participanteDto = await _participanteService.Conectar(Context.ConnectionId, Context.UserIdentifier);
            await Groups.AddToGroupAsync(Context.ConnectionId, participanteDto.SalaId);
            await AtualizarParticipantesSala(participanteDto.SalaId);
        }

        public async Task RemoverParticipante(string participanteId, string participanteQueRemoveu)
        {
            var participanteDto = await _participanteService.BuscarParticipantePorId(participanteId);
            await _participanteService.RemoverParticipante(participanteId);
            await Clients.User(participanteId).SendAsync("ParticipanteRemovido", participanteQueRemoveu);
            await AtualizarParticipantesSala(participanteDto.SalaId, participanteDto.Jogador);
        }

        private async Task AtualizarParticipantesSala(string salaId, bool? jogador = null)
        {
            if (!jogador.HasValue)
            {
                var participantes = await _participanteService.BuscarParticipantesPorSala(salaId);
                await Clients.Group(salaId).SendAsync("ReceberJogadores", participantes.Where(x => x.Jogador));
                await Clients.Group(salaId).SendAsync("ReceberAdministradores", participantes.Where(x => !x.Jogador));
            }
            else if (jogador.Value)
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

        public async Task EnviarConfiguracaoSala(AlterarConfiguracaoSalaInputModel salaConfiguracaoDto)
        {
            SalaConfiguracaoViewModel salaConfiguracaoAlterada = await _salaConfiguracaoService.AlterarAsync(salaConfiguracaoDto);
            await Clients.Group(salaConfiguracaoDto.SalaId).SendAsync("ReceberConfiguracaoSala", salaConfiguracaoAlterada);
        }

        public async Task EnviarVoto(String votoValor)
        {
            var votoDto = await _participanteService.VotoParticipante(Context.UserIdentifier, votoValor);
            await Clients.Group(votoDto.SalaId).SendAsync("ReceberVoto", votoDto);
        }

        public async Task ResetarSala(string salaId)
        {
            SalaViewModel sala = await _salaService.ResetarSala(salaId);
            await Clients.Group(salaId).SendAsync("ReceberSala", sala);
            await Clients.Group(salaId).SendAsync("ReceberJogadores", sala.Jogadores);
        }

        public async Task FinalizarJogo(string salaId)
        {
            SalaViewModel sala = await _salaService.FinalizarJogo(salaId);
            await Clients.Group(salaId).SendAsync("ReceberSala", sala);
            await Clients.Group(salaId).SendAsync("ReceberJogadores", sala.Jogadores);
        }

        public async Task RevotarSala(string salaId)
        {
            SalaViewModel sala = await _salaService.ResetarSala(salaId);
            await Clients.Group(salaId).SendAsync("ReceberSala", sala);
            await Clients.Group(salaId).SendAsync("ReceberJogadores", sala.Jogadores);
        }

        public async Task ConcluirSala(string salaId)
        {
            SalaViewModel sala = await _salaService.ResetarSala(salaId);
            await Clients.Group(salaId).SendAsync("ReceberSala", sala);
            await Clients.Group(salaId).SendAsync("ReceberJogadores", sala.Jogadores);
        }
    }
}