using AutoMapper;
using Microsoft.AspNetCore.Identity.UI.Services;
using ScrumPoker.Application.Interfaces.ApplicationServices;
using ScrumPoker.CrossCutting.Templates;
using ScrumPoker.Domain.DTOs.Application;
using ScrumPoker.Domain.Entities.Perfis;
using ScrumPoker.Domain.Entities.Usuarios.Requests;
using ScrumPoker.Domain.Interfaces.Application;
using ScrumPoker.Domain.Interfaces.Repositories;
using ScrumPoker.Identity.Entities;
using ScrumPoker.Identity.Interfaces;
using System.Threading.Tasks;

namespace ScrumPoker.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IIdentityService _identityService;
        private readonly IPerfilRepository _perfilRepository;
        private readonly IMapper _mapper;
        private readonly IPerfilService _perfilService;
        private readonly IEmailSender _emailSender;

        public UsuarioService(IIdentityService identityService, IMapper mapper, IPerfilRepository perfilRepository, IPerfilService perfilService, IEmailSender emailSender)
        {
            _identityService = identityService;
            _mapper = mapper;
            _perfilRepository = perfilRepository;
            _perfilService = perfilService;
            _emailSender = emailSender;
        }

        public async Task<ResultadoDto<Perfil>> RegistrarNovoUsuarioAsync(RegistrarUsuarioRequest registrarUsuario, string urlConfirmaEmail)
        {
            var user = _mapper.Map<User>(registrarUsuario);

            var result = await _identityService.CriarUsuarioAsync(user, registrarUsuario.Senha);

            if (!result.Sucesso)
                return new ResultadoDto<Perfil>(result.Erros);

            var perfil = _mapper.Map<Perfil>(registrarUsuario);
            perfil = await _perfilRepository.IncluirAsync(perfil);

            var emailResultado = await EnviarConfirmacaoEmailAsync(perfil.Login, urlConfirmaEmail);
            if (!emailResultado.Sucesso)
                return new ResultadoDto<Perfil>(emailResultado.Erros);

            return new ResultadoDto<Perfil>(perfil);
        }

        public async Task<ResultadoDto> EnviarConfirmacaoEmailAsync(string login, string urlConfirmaEmail)
        {
            var userResultado = await _identityService.BuscarPorUserNameAsync(login);
            if (!userResultado.Sucesso)
                return userResultado;

            var tokenResultado = await _identityService.GerarTokenConfirmacaoEmailAsync(userResultado.Resultado);
            if (!tokenResultado.Sucesso)
                return tokenResultado;

            if (urlConfirmaEmail.EndsWith('/'))
                urlConfirmaEmail = urlConfirmaEmail.Remove(urlConfirmaEmail.Length - 1, 1);

            urlConfirmaEmail += $"?token={System.Web.HttpUtility.UrlEncode(tokenResultado.Resultado)}";
            urlConfirmaEmail += $"&userName={System.Web.HttpUtility.UrlEncode(userResultado.Resultado.UserName)}";

            var perfil = await _perfilService.BuscarPorLoginAsync(userResultado.Resultado.UserName);
            string emailHtml = await EmailTemplate.GetEmailConfirmarEmailAsync(urlConfirmaEmail, perfil.Nome);
            await _emailSender.SendEmailAsync(userResultado.Resultado.Email, "Confirmação de e-mail - ScrumPoker", emailHtml);

            return new ResultadoDto();
        }

        public async Task<ResultadoDto<Perfil>> ConfirmarEmailAsync(string login, string token)
        {
            var userResultado = await _identityService.BuscarPorUserNameAsync(login);
            if (!userResultado.Sucesso)
                return new ResultadoDto<Perfil>(userResultado.Erros);

            var confirmacaoResultado = await _identityService.ConfirmarEmailAsync(userResultado.Resultado, token);
            if (!confirmacaoResultado.Sucesso)
                return new ResultadoDto<Perfil>(confirmacaoResultado.Erros);

            var perfil = await _perfilRepository.BuscarPorIdAsync(login);
            return new ResultadoDto<Perfil>(perfil);
        }

        public async Task<ResultadoDto<Perfil>> LoginAsync(UserLoginRequest userLogin)
        {
            var userResultado = await _identityService.BuscarPorUserNameAsync(userLogin.UserName);
            if (!userResultado.Sucesso)
                return new ResultadoDto<Perfil>(new ErroDto(
                     "Unauthorized",
                     "Usuário ou senha estão incorretos."
                 ));

            var result = await _identityService.ValidarSenhaAsync(userResultado.Resultado, userLogin.Password);
            if (!userResultado.Sucesso)
                return new ResultadoDto<Perfil>(new ErroDto(
                    "Unauthorized",
                    "Usuário ou senha estão incorretos."
                ));

            if (!userResultado.Resultado.EmailConfirmed)
                return new ResultadoDto<Perfil>(new ErroDto(
                    "EmailNotConfirmed",
                    "E-mail não foi confirmado."
                ));

            var perfil = await _perfilRepository.BuscarPorIdAsync(userLogin.UserName);
            return new ResultadoDto<Perfil>(perfil);
        }

        public async Task<ResultadoDto> SolicitarResetDeSenhaAsync(string login, string urlResetarSenha)
        {
            var userResultado = await _identityService.BuscarPorUserNameAsync(login);
            if (!userResultado.Sucesso)
                return userResultado;

            var token = await _identityService.GerarTokenParaResetDeSenhaAsync(userResultado.Resultado);

            if (urlResetarSenha.EndsWith('/'))
                urlResetarSenha = urlResetarSenha.Remove(urlResetarSenha.Length - 1, 1);

            urlResetarSenha += $"?token={System.Web.HttpUtility.UrlEncode(token)}";
            urlResetarSenha += $"&userName={System.Web.HttpUtility.UrlEncode(userResultado.Resultado.UserName)}";

            var perfil = await _perfilService.BuscarPorLoginAsync(userResultado.Resultado.UserName);
            string emailHtml = await EmailTemplate.GetEmailRestarSenhaAsync(urlResetarSenha, perfil.Nome);
            await _emailSender.SendEmailAsync(userResultado.Resultado.Email, "Recuperar Senha - ScrumPoker", emailHtml);

            return new ResultadoDto();
        }

        public async Task<ResultadoDto> ResetarSenhaAsync(UserResetarSenhaRequest userResetarSenha)
        {
            var userResultado = await _identityService.BuscarPorUserNameAsync(userResetarSenha.UserName);
            if (!userResultado.Sucesso)
                return userResultado;

            return await _identityService.ResetarSenhaAsync(userResultado.Resultado, userResetarSenha.Token, userResetarSenha.Password);
        }
    }
}
