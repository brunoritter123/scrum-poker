using ScrumPoker.Domain.Entities.UsuarioEntity;
using System;
using System.Threading.Tasks;

namespace ScrumPoker.Domain.Interfaces.Repositories
{
    public interface IPerfilRepository
    {
        //Task<Perfil> BuscarPorIdAsync(Guid id);
        //Task<bool> ExisteEntityAsync(Guid id);
        //Task<Perfil> IncluirAsync(Perfil perfil);
        Task<Perfil> AlterarAsync(Perfil perfil);
        Task<Perfil> BuscarPorIdAsync(Guid id);
    }
}