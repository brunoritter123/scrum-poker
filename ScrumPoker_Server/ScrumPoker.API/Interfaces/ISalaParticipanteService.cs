using System.Linq;
using ScrumPoker.API.Dtos;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ScrumPoker.API.Interfaces
{
    public interface ISalaParticipanteService
    {
        Task<SalaParticipanteDto> IncluirOuAlterarAsync(PostParticipanteDto participanteDto);
        Task<IEnumerable<SalaParticipanteDto>> BuscarJogadoresPorSalaId(string salaId);
        Task<IEnumerable<SalaParticipanteDto>> BuscarAdministradoresPorSalaId(string salaId);
        Task<SalaParticipanteDto> Desconectar(string participanteId);
        Task RemoverParticipante(string participanteId);
        Task<SalaParticipanteDto> Conectar(string conexaoId, string participanteId);
        Task<SalaParticipanteDto> BuscarParticipantePorId(string id);
        Task<SalaParticipanteDto> VotoParticipante(string participanteId, string votoValor);
    }
}