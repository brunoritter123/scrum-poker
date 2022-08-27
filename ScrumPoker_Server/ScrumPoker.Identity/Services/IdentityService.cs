using Microsoft.AspNetCore.Identity;
using ScrumPoker.Domain.DTOs.Application;
using ScrumPoker.Identity.Entities;
using ScrumPoker.Identity.Interfaces;

namespace ScrumPoker.Identity.Services

{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _singInManager;

        public IdentityService(UserManager<User> userManager, SignInManager<User> singInManager)
        {
            _userManager = userManager;
            _singInManager = singInManager;
        }

        public async Task<ResultadoDto<User>> BuscarPorUserNameAsync(string userName)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user == null)
                    throw new Exception($"Usuário '{userName}' não encontrado");

                return new ResultadoDto<User>(user);
            }
            catch (Exception ex)
            {
                var erro = new ErroDto("BuscarPorUserName", ex.Message);
                return new ResultadoDto<User>(erro);
            }

        }

        public async Task<ResultadoDto> ConfirmarEmailAsync(User user, string token)
        {
            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
                return new ResultadoDto();

            var erros = new List<ErroDto>();
            foreach (var erro in result.Errors)
            {
                erros.Add(new ErroDto(erro.Code, erro.Description));
            }
            return new ResultadoDto(erros);
        }

        public async Task<ResultadoDto> CriarUsuarioAsync(User user, string senha)
        {
            var result = await _userManager.CreateAsync(user, senha);
            if (result.Succeeded)
                return new ResultadoDto();

            var erros = new List<ErroDto>();
            foreach (var erro in result.Errors)
            {
                erros.Add(new ErroDto(erro.Code, erro.Description));
            }
            return new ResultadoDto(erros);
        }

        public async Task<ResultadoDto<string>> GerarTokenConfirmacaoEmailAsync(User user)
        {
            try
            {
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                return new ResultadoDto<string>(token);

            }
            catch (Exception ex)
            {
                var erro = new ErroDto("GerarTokenConfirmacaoEmailAsync", ex.Message);
                return new ResultadoDto<string>(erro);
            }
        }

        public Task<string> GerarTokenParaResetDeSenhaAsync(User user)
        {
            return _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<ResultadoDto> ResetarSenhaAsync(User user, string token, string novaSenha)
        {
            var result = await _userManager.ResetPasswordAsync(user, token, novaSenha);

            if (result.Succeeded)
                return new ResultadoDto();

            var erros = new List<ErroDto>();
            foreach (var erro in result.Errors)
            {
                erros.Add(new ErroDto(erro.Code, erro.Description));
            }
            return new ResultadoDto(erros);
        }

        public async Task<ResultadoDto> ValidarSenhaAsync(User user, string senha)
        {
            var result = await _singInManager.CheckPasswordSignInAsync(user, senha, false);
            if (result.Succeeded)
                return new ResultadoDto();

            return new ResultadoDto(new ErroDto("", "Usuário ou senha estão incorretos."));

        }
    }
}
