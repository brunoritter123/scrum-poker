using ScrumPoker.Application.DTOs.ViewModels;
using System.Threading.Tasks;

namespace ScrumPoker.Application.Interfaces.ApplicationServices
{
    public interface ISalaService
    {
        Task<SalaViewModel> ObterPorIdAsync(string id);
        Task<SalaViewModel> IncluirSalaPadraoAsync(string id);
        Task ExcluirCartasAsync(string id);
        Task<SalaViewModel> ResetarSala(string salaId);
        Task<SalaViewModel> FinalizarJogo(string salaId);
    }
}