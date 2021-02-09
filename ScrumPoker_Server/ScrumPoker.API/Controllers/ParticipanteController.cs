using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScrumPoker.API.Dtos;
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
    public class ParticipanteController : ControllerBase
    {
        private readonly IParticipanteService _participanteService;

        public ParticipanteController(IParticipanteService participanteService)
        {
            _participanteService = participanteService;
        }

        [HttpPost()]
        public async Task<ActionResult<SalaViewModel>> IncluirOuAlterarSalaParticipanteAsync(IncluirParticipanteSalaInputModel participanteDto)
        {
            var participante = await _participanteService.IncluirOuAlterarAsync(participanteDto);
            return Ok(participante);
        }
    }
}