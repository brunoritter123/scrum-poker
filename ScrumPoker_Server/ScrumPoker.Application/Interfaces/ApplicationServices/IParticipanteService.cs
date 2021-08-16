using ScrumPoker.Application.DTOs.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;
using ScrumPoker.Application.DTOs.InputModels;

namespace ScrumPoker.Application.Interfaces.ApplicationServices
{
    public interface IParticipanteService
    {
        Task<ParticipanteViewModel> IncluirOuAlterarAsync(IncluirParticipanteSalaInputModel participanteDto);
        Task<ParticipanteViewModel> Desconectar(string participanteId);
        Task RemoverParticipante(string participanteId);
        Task<ParticipanteViewModel> Conectar(string conexaoId, string participanteId);
        Task<ParticipanteViewModel> BuscarParticipantePorId(string id);
        Task<VotoViewModel> VotoParticipante(string participanteId, string votoValor);
    }
}