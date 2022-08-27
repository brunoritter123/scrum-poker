using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ScrumPoker.Domain.Entities.Perfis;
using ScrumPoker.Domain.Entities.Usuarios.Requests;
using ScrumPoker.Domain.Interfaces.Application;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ScrumPoker.API.Controllers
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUsuarioService _usuarioService;

        public UserController(
            IConfiguration config,
            IUsuarioService usuarioService)
        {
            this._config = config;
            _usuarioService = usuarioService;
        }

        [HttpPost("Registrar")]
        public async Task<ActionResult> Registrar(
            [FromBody] RegistrarUsuarioRequest userDto,
            [FromQuery] ConfirmacaoEmailRequest dto)
        {
            var result = await _usuarioService.RegistrarNovoUsuarioAsync(userDto, dto.UrlConfirmaEmail);

            if (!result.Sucesso) return BadRequest(result.Erros);

            return Created("Login", result.Resultado);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(UserLoginRequest userLogin)
        {
            var result = await _usuarioService.LoginAsync(userLogin);
            if (!result.Sucesso)
                return Unauthorized(result.Erros.First());

            return Ok(new
            {
                token = GenerateJWToken(result.Resultado),
                user = result.Resultado
            });
        }

        [HttpPost("resetar-senha")]
        public async Task<ActionResult> ResetarSenha(UserResetarSenhaRequest userResetarSenha)
        {
            var result = await _usuarioService.ResetarSenhaAsync(userResetarSenha);
            if (!result.Sucesso) return BadRequest(result.Erros);

            return NoContent();
        }

        [HttpPost("confirmar-email")]
        public async Task<ActionResult> ConfirmarEmail(UserConfirmaEmailRequest userEmail)
        {
            var result = await _usuarioService.ConfirmarEmailAsync(userEmail.UserName, userEmail.Token);
            if (!result.Sucesso) return BadRequest(result.Erros);

            return Ok(new
            {
                token = GenerateJWToken(result.Resultado),
                user = result.Resultado
            });
        }

        [HttpPost("enviar-confirmacao-email/{userName}")]
        public async Task<ActionResult> EnviarConfirmacaoEmail(string userName, [FromQuery] ConfirmacaoEmailRequest dto)
        {
            var resultado = await _usuarioService.EnviarConfirmacaoEmailAsync(userName, dto.UrlConfirmaEmail);
            if (!resultado.Sucesso)
                return BadRequest(resultado.Erros);

            return NoContent();
        }

        [HttpPost("solicitacao-resetar-senha/{userName}")]
        public async Task<ActionResult> SolicitacaoResetarSenha(string userName, [FromQuery] ResetarSenhaRequest dto)
        {
            await _usuarioService.SolicitarResetDeSenhaAsync(userName, dto.UrlCallback);
            return NoContent();
        }

        private string GenerateJWToken(Perfil perfil)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, perfil.Login),
                new Claim(ClaimTypes.Name, perfil.Nome),
                new Claim(ClaimTypes.Email, perfil.Email)
            };

            //var roles = await _userManager.GetRolesAsync(user);
            //foreach (var role in roles)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, role));
            //}

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.GetSection("TokenConfig:JwtSecretKey").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            int expiresToken = 24;
            int.TryParse(_config.GetSection("TokenConfig:Expiration").Value, out expiresToken);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _config.GetSection("TokenConfig:Issuer").Value,
                Audience = _config.GetSection("TokenConfig:Audience").Value,
                Expires = DateTime.UtcNow.AddHours(expiresToken),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}