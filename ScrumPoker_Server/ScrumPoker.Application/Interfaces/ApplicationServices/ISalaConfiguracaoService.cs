using ScrumPoker.Application.DTOs.InputModels;
using ScrumPoker.Application.DTOs.ViewModels;
using System.Threading.Tasks;

namespace ScrumPoker.Application.Interfaces.ApplicationServices
{
    public interface ISalaConfiguracaoService
    {
        //internal Task<SalaConfiguracaoViewModel> BuscarPorIdAsync(long id);
        Task<SalaConfiguracaoViewModel> AlterarAsync(AlterarConfiguracaoSalaInputModel salaConfiguracaoDto);
    }
}