using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScrumPoker.Application.DTOs.InputModels;
using ScrumPoker.Application.DTOs.ViewModels;
using ScrumPoker.Application.Interfaces.ApplicationServices;
using System.Threading.Tasks;

namespace ScrumPoker.API.Controllers
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PerfilController : ControllerBase
    {
        private readonly IPerfilService _perfilService;

        public PerfilController(
            IPerfilService perfilService
            )
        {
            _perfilService = perfilService;
        }

        [HttpGet("{userName}")]
        public async Task<ActionResult<PerfilViewModel>> BuscarPorIdAsync(string userName)
        {
            var perfil = await _perfilService.BuscarPorLoginAsync(userName);
            if (perfil is null) return NotFound();

            return Ok(perfil);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<PerfilViewModel>> AlterarAsync(
            [FromBody] PerfilAlteracaoInputModel perfilAlteracao)
        {
            var novoPerfil = await _perfilService.AlterarAsync(perfilAlteracao);
            return Ok(novoPerfil);
        }
    }
}