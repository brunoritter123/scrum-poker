using System.Net.Sockets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ScrumPoker.Domain.Identity;
using System.Threading.Tasks;
using ScrumPoker.API.Dtos;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.IO;
using ScrumPoker.CrossCutting.Templates;

namespace ScrumPoker.API.Controllers
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _singInManager;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        public AuthController(
            IConfiguration config,
            UserManager<User> userManager,
            SignInManager<User> singInManager,
            IEmailSender emailSender,
            IMapper mapper)
        {
            this._userManager = userManager;
            this._singInManager = singInManager;
            this._mapper = mapper;
            this._config = config;
            this._emailSender = emailSender;
        }

        [HttpPost("Registrar")]
        public async Task<ActionResult> Registrar(
            [FromBody] UserDto userDto,
            [FromQuery] string urlConfirmaEmail)
        {
            if (urlConfirmaEmail is null || !Uri.IsWellFormedUriString(urlConfirmaEmail, UriKind.Absolute))
                return BadRequest( new List<ErroRequestDto>()
                {
                    new ErroRequestDto()
                    {
                        Code = "QueryString",
                        Description = $"Informe uma URL valida na querystring: '{nameof(urlConfirmaEmail)}'."
                    }
                } );

            var user = _mapper.Map<User>(userDto);
            var result = await _userManager.CreateAsync(user, userDto.Password);
            var userResult = _mapper.Map<UserDto>(user);

            if (!result.Succeeded) return BadRequest(result.Errors);

            await EnviarConfirmacaoEmail(user.UserName, urlConfirmaEmail);

            return Created("Login", userResult);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(UserLoginDto userLogin)
        {
            var user = await _userManager.FindByNameAsync(userLogin.UserName);
            if (user is null)
                return Unauthorized(new ErroRequestDto()
                    {
                        Code = "Unauthorized",
                        Description = $"Usuário ou senha estão incorretos."
                    });

            var result = await _singInManager.CheckPasswordSignInAsync(user, userLogin.Password, false);

            if (!result.Succeeded)
                return Unauthorized(new ErroRequestDto()
                    {
                        Code = "Unauthorized",
                        Description = $"Usuário ou senha estão incorretos."
                    });

            if (!user.EmailConfirmed)
                return Unauthorized(new  ErroRequestDto()
                    {
                        Code = "EmailNotConfirmed",
                        Description = $"E-mail não foi confirmado."
                    });

            var appUser = await _userManager
                .Users
                .FirstOrDefaultAsync(u => u.NormalizedUserName == userLogin.UserName.ToUpper());

            return Ok(new {
                token = GenerateJWToken(appUser).Result,
                user = appUser
            });
        }

        [HttpPost("resetar-senha")]
        public async Task<ActionResult> ResetarSenha(UserResetarSenhaDto userResetarSenha)
        {
            var user = await _userManager.FindByNameAsync(userResetarSenha.UserName);
            if (user is null) return NotFound();

            var result = await _userManager.ResetPasswordAsync(user, userResetarSenha.Token, userResetarSenha.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            return NoContent();
        }

        [HttpPost("confirmar-email")]
        public async Task<ActionResult> ConfirmarEmail(UserConfirmaEmailDto userEmail)
        {
            var user = await _userManager.FindByNameAsync(userEmail.UserName);
            if (user is null) return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, userEmail.Token);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var appUser = await _userManager
                .Users
                .FirstOrDefaultAsync(u => u.NormalizedUserName == userEmail.UserName.ToUpper());

            return Ok(new {
                token = GenerateJWToken(appUser).Result,
                user = appUser
            });
        }

        [HttpPost("enviar-confirmacao-email/{userName}")]
        public async Task<ActionResult> EnviarConfirmacaoEmail(string userName, [FromQuery] string urlConfirmaEmail)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null) return NotFound();

            if (urlConfirmaEmail is null || !Uri.IsWellFormedUriString(urlConfirmaEmail, UriKind.Absolute))
                return BadRequest( new List<ErroRequestDto>()
                {
                    new ErroRequestDto()
                    {
                        Code = "QueryString",
                        Description = $"Informe uma URL valida na querystring: '{nameof(urlConfirmaEmail)}'."
                    }
                } );

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            if (urlConfirmaEmail.EndsWith('/'))
                urlConfirmaEmail = urlConfirmaEmail.Remove(urlConfirmaEmail.Length-1, 1);

            urlConfirmaEmail += $"?token={System.Web.HttpUtility.UrlEncode(token)}";
            urlConfirmaEmail += $"&userName={System.Web.HttpUtility.UrlEncode(user.UserName)}";

            string emailHtml = await EmailTemplate.GetEmailConfirmarEmailAsync(urlConfirmaEmail, user.UserName);
            await _emailSender.SendEmailAsync(user.Email, "Confirmação de e-mail - ScrumPoker" ,emailHtml);

            return NoContent();
        }

        [HttpPost("solicitacao-resetar-senha/{userName}")]
        public async Task<ActionResult> SolicitacaoResetarSenha(string userName, [FromQuery] string urlResetarSenha)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null) return NotFound();

            if (urlResetarSenha is null || !Uri.IsWellFormedUriString(urlResetarSenha, UriKind.Absolute))
                return BadRequest( new List<ErroRequestDto>()
                {
                    new ErroRequestDto()
                    {
                        Code = "QueryString",
                        Description = $"Informe uma URL valida na querystring: '{nameof(urlResetarSenha)}'."
                    }
                } );

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            if (urlResetarSenha.EndsWith('/'))
                urlResetarSenha = urlResetarSenha.Remove(urlResetarSenha.Length-1, 1);

            urlResetarSenha += $"?token={System.Web.HttpUtility.UrlEncode(token)}";
            urlResetarSenha += $"&userName={System.Web.HttpUtility.UrlEncode(user.UserName)}";

            string emailHtml = await EmailTemplate.GetEmailRestarSenhaAsync(urlResetarSenha, user.UserName);
            await _emailSender.SendEmailAsync(user.Email, "Recuperar Senha - ScrumPoker" ,emailHtml);

            return NoContent();
        }

        private async Task<string> GenerateJWToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["JwtSecretKey"]));
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