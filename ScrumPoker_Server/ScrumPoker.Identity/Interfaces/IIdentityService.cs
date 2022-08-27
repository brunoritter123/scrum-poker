using ScrumPoker.Domain.DTOs.Application;
using ScrumPoker.Identity.Entities;

namespace ScrumPoker.Identity.Interfaces
{
    public interface IIdentityService
    {
        Task<ResultadoDto> CriarUsuarioAsync(User user, string senha);
        Task<ResultadoDto<User>> BuscarPorUserNameAsync(string userName);
        Task<ResultadoDto<string>> GerarTokenConfirmacaoEmailAsync(User user);
        Task<ResultadoDto> ConfirmarEmailAsync(User user, string token);
        Task<ResultadoDto> ValidarSenhaAsync(User user, string senha);
        Task<string> GerarTokenParaResetDeSenhaAsync(User user);
        Task<ResultadoDto> ResetarSenhaAsync(User user, string token, string novaSenha);
    }
}
