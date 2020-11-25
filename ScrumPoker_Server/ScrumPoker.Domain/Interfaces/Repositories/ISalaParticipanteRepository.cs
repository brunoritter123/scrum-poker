using System.Collections.Generic;
using System.Threading.Tasks;
using ScrumPoker.Domain.Models;

namespace ScrumPoker.Domain.Interfaces.Repositories
{
    public interface ISalaParticipanteRepository
    {
        Task<SalaParticipante> BuscarPorIdAsync(string id);
        Task<IEnumerable<SalaParticipante>> BuscarJogadoresPorSalaAsync(string salaId);
        Task<IEnumerable<SalaParticipante>> BuscarAdministradoresPorSalaAsync(string salaId);
        Task<bool> ExisteEntityAsync(string id);
        Task<SalaParticipante> IncluirAsync(SalaParticipante participante);
        Task<SalaParticipante> AlterarAsync(SalaParticipante participante);
        Task RemoverAsync(string id);
    }
}