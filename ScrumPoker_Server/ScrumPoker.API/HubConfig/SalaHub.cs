using Microsoft.AspNetCore.SignalR;
using ScrumPoker.Application.DTOs.InputModels;
using ScrumPoker.Application.DTOs.ViewModels;
using ScrumPoker.Application.Interfaces.ApplicationServices;
using System;
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
            if (participanteDto != null)
            {
                string metodo = participanteDto.Jogador ? "RecberJogadorDesconectado" : "RecberAdministradorDesconectado";
                await Clients.Group(participanteDto.SalaId).SendAsync(metodo, participanteDto.Id);
            }

            await base.OnDisconnectedAsync(exception);
        }

        private async Task IncluirParticipante()
        {
            await Clients.User(Context.UserIdentifier).SendAsync("ParticipanteRemovido", "", Context.UserIdentifier);
            var participanteDto = await _participanteService.Conectar(Context.ConnectionId, Context.UserIdentifier);
            await Groups.AddToGroupAsync(Context.ConnectionId, participanteDto.SalaId);

            string metodo = participanteDto.Jogador ? "ReceberNovoJogador" : "ReceberNovoAdministrador";
            await Clients.Group(participanteDto.SalaId).SendAsync(metodo, participanteDto);
        }

        public async Task RemoverParticipante(string participanteId, string participanteQueRemoveu)
        {
            var participanteDto = await _participanteService.BuscarParticipantePorId(participanteId);
            await _participanteService.RemoverParticipante(participanteId);
            await Clients.Group(participanteDto.SalaId).SendAsync("ParticipanteRemovido", participanteQueRemoveu, participanteId);
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