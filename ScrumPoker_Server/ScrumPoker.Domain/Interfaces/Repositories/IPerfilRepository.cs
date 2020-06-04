using System;
using System.Threading.Tasks;
using ScrumPoker.Domain.Models;

namespace ScrumPoker.Domain.Interfaces.Repositories
{
    public interface IPerfilRepository
    {
        Task<Perfil> BuscarPorIdAsync(Guid id);
        Task<bool> ExisteEntityAsync(Guid id);
        Task<Perfil> IncluirAsync(Perfil perfil);
        Task<Perfil> AlterarAsync(Perfil perfil);
    }
}