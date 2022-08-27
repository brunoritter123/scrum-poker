using ScrumPoker.Domain.Entities.Salas.Participantes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScrumPoker.Domain.Interfaces.Repositories
{
    public interface IParticipanteRepository
    {
        Task<Participante> BuscarPorIdAsync(string id);
        Task<IEnumerable<Participante>> BuscarJogadoresPorSalaAsync(string salaId);
        Task<IEnumerable<Participante>> BuscarAdministradoresPorSalaAsync(string salaId);
        Task<bool> ExisteEntityAsync(string id);
        Task<Participante> IncluirAsync(Participante participante);
        Task<Participante> AlterarAsync(Participante participante);
        Task RemoverAsync(string id);
        Task<IEnumerable<Participante>> BuscarParticipantesPorSalaAsync(string salaId);
    }
}