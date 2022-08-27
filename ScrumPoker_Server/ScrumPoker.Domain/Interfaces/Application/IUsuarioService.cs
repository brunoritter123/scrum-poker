using ScrumPoker.Domain.DTOs.Application;
using ScrumPoker.Domain.Entities.Perfis;
using ScrumPoker.Domain.Entities.Usuarios.Requests;
using System.Threading.Tasks;

namespace ScrumPoker.Domain.Interfaces.Application
{
    public interface IUsuarioService
    {
        Task<ResultadoDto<Perfil>> RegistrarNovoUsuarioAsync(RegistrarUsuarioRequest registrarUsuario, string urlConfirmaEmail);
        Task<ResultadoDto> EnviarConfirmacaoEmailAsync(string login, string urlConfirmaEmail);
        Task<ResultadoDto<Perfil>> ConfirmarEmailAsync(string login, string token);
        Task<ResultadoDto<Perfil>> LoginAsync(UserLoginRequest userLogin);
        Task<ResultadoDto> SolicitarResetDeSenhaAsync(string login, string urlResetarSenha);
        Task<ResultadoDto> ResetarSenhaAsync(UserResetarSenhaRequest userResetarSenha);
    }
}
