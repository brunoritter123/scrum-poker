using ScrumPoker.Application.DTOs.InputModels;
using ScrumPoker.Application.DTOs.ViewModels;
using System;
using System.Threading.Tasks;

namespace ScrumPoker.Application.Interfaces.ApplicationServices
{
    public interface IPerfilService
    {
        Task<PerfilViewModel> BuscarPorLoginAsync(string login);
        Task<PerfilViewModel> AlterarAsync(PerfilAlteracaoInputModel perfilAlteracao);
    }   
}
