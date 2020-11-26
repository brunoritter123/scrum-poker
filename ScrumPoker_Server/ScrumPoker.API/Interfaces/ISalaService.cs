using ScrumPoker.API.Dtos;
using ScrumPoker.Domain.Models;
using System.Threading.Tasks;

namespace ScrumPoker.API.Interfaces
{
    public interface ISalaService
    {
        Task<SalaDto> ObterPorIdAsync(string id);
        Task<SalaDto> IncluirSalaPadraoAsync(string id);
        Task<SalaDto> AlterarAsync(SalaDto salaDto);
        Task ExcluirCartasAsync(string id);
        Task<SalaDto> ResetarSala(string salaId);
        Task<SalaDto> FinalizarJogo(string salaId);
    }
}