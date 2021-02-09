using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScrumPoker.Application.DTOs.InputModels;
using ScrumPoker.Application.DTOs.ViewModels;
using ScrumPoker.Domain.Identity;
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
        private readonly UserManager<User> _userManager;

        public PerfilController(
            IPerfilService perfilService,
            UserManager<User> userManager
            )
        {
            _perfilService = perfilService;
            _userManager = userManager;
        }

        [HttpGet("{userName}")]
        public async Task<ActionResult<PerfilViewModel>> BuscarPorIdAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user is null) return NotFound();

            var perfil = await _perfilService.BuscarPorIdAsync(user.PerfilId);
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