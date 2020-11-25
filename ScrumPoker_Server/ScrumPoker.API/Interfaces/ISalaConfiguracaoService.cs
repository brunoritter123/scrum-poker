using ScrumPoker.API.Dtos;
using System.Threading.Tasks;

namespace ScrumPoker.API.Interfaces
{
    public interface ISalaConfiguracaoService
    {
        Task<SalaConfiguracaoDto> BuscarPorIdAsync(long id);
        Task<SalaConfiguracaoDto> AlterarAsync(SalaConfiguracaoDto salaConfiguracaoDto);
    }
}