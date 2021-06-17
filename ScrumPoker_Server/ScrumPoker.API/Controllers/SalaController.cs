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
    public class SalaController : ControllerBase
    {
        private readonly ISalaService _salaService;

        public SalaController(ISalaService salaService)
        {
            _salaService = salaService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalaViewModel>> ObterPorIdAsync(string id)
        {
            var sala = await _salaService.ObterPorIdAsync(id);
            return Ok(sala);
        }

        [HttpPost("gerar-sala-padrao")]
        public async Task<ActionResult<SalaViewModel>> GerarSalaPadraoAsync(GerarSalaPadraoInputModel gerarSalaInput)
        {
            var sala = await _salaService.GerarSalaPadraoAsync(gerarSalaInput);
            return Ok(sala);
        }

        [HttpDelete("{id}/cartas")]
        public async Task<ActionResult> ExcluirCartasAsync(string id)
        {
            await _salaService.ExcluirCartasAsync(id);

            return NoContent();
        }
    }
}