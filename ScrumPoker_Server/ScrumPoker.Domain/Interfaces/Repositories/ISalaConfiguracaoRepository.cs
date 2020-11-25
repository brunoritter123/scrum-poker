using System.Threading.Tasks;
using ScrumPoker.Domain.Models;

namespace ScrumPoker.Domain.Interfaces.Repositories
{
    public interface ISalaConfiguracaoRepository
    {
        Task<SalaConfiguracao> BuscarPorIdAsync(long id);
        Task<bool> ExisteEntityAsync(long id);
        Task<SalaConfiguracao> IncluirAsync(SalaConfiguracao sala);
        Task<SalaConfiguracao> AlterarAsync(SalaConfiguracao sala);
        Task ExcluirCartasAsync(long id);
    }
}