using System.Threading.Tasks;
using ScrumPoker.Domain.Models;

namespace ScrumPoker.Domain.Interfaces.Repositories
{
    public interface ISalaRepository
    {
        Task<Sala> BuscarPorIdAsync(string id);
        Task<bool> ExisteEntityAsync(string id);
        Task<Sala> IncluirAsync(Sala sala);
        Task<Sala> AlterarAsync(Sala sala);
        Task ExcluirCartasAsync(string id);
    }
}