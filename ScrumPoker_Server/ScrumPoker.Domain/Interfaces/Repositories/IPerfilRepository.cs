using ScrumPoker.Domain.Entities.Perfis;
using System.Threading.Tasks;

namespace ScrumPoker.Domain.Interfaces.Repositories
{
    public interface IPerfilRepository
    {
        //Task<Perfil> BuscarPorIdAsync(Guid id);
        //Task<bool> ExisteEntityAsync(Guid id);
        Task<Perfil> AlterarAsync(Perfil perfil);
        Task<Perfil> BuscarPorIdAsync(string login);
        Task<Perfil> IncluirAsync(Perfil perfil);
    }
}