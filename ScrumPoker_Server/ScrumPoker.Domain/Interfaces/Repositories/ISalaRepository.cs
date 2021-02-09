using ScrumPoker.Domain.Entities.SalaEntity;
using System.Threading.Tasks;

namespace ScrumPoker.Domain.Interfaces.Repositories
{
    public interface ISalaRepository
    {
        Task<Sala> BuscarPorIdAsync(string id);
        Task<bool> ExisteSalaAsync(string id);
        Task<Sala> IncluirAsync(Sala sala);
        Task<Sala> AlterarAsync(Sala sala);
        Task ExcluirCartasAsync(string id);
    }
}