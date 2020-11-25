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
    public class SalaParticipanteController : ControllerBase
    {
        private readonly ISalaParticipanteService _participanteService;

        public SalaParticipanteController(ISalaParticipanteService participanteService)
        {
            _participanteService = participanteService;
        }

        [HttpPost()]
        public async Task<ActionResult<SalaDto>> IncluirOuAlterarSalaParticipanteAsync(PostParticipanteDto participanteDto)
        {
            var participante = await _participanteService.IncluirOuAlterarAsync(participanteDto);
            return Ok(participante);
        }
    }
}