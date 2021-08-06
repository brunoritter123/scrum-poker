using ScrumPoker.Application.DTOs.InputModels;
using ScrumPoker.Application.DTOs.ViewModels;
using System.Threading.Tasks;

namespace ScrumPoker.Application.Interfaces.ApplicationServices
{
    public interface ISalaService
    {
        Task<SalaViewModel> ObterPorIdAsync(string id);
        Task<SalaViewModel> GerarSalaPadraoAsync(GerarSalaPadraoInputModel gerarSalaInput);
        Task<SalaViewModel> ResetarSala(string salaId);
        Task<SalaViewModel> FinalizarJogo(string salaId);
    }
}