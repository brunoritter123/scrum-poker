using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ScrumPoker.Domain.Interfaces.Repositories;
using ScrumPoker.Domain.Models;
using System.Threading.Tasks;
using AutoMapper;
using ScrumPoker.API.Dtos;
using ScrumPoker.API.Interfaces;

namespace ScrumPoker.API.Controllers
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class SalaController : ControllerBase
    {
        private readonly ISalaService _salaService;
        private readonly IMapper _mapper;

        public SalaController(ISalaService salaService, IMapper mapper)
        {
            _mapper = mapper;
            _salaService = salaService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalaDto>> ObterPorIdAsync(string id)
        {
            var sala = await _salaService.ObterPorIdAsync(id);
            return Ok(sala);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<SalaDto>> IncluirSalaPadraoAsync(string id)
        {
            var sala = await _salaService.IncluirSalaPadraoAsync(id);
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