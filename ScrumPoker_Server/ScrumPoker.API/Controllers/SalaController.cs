using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ScrumPoker.Domain.Interfaces.Repositories;
using ScrumPoker.Domain.Models;
using System.Threading.Tasks;
using AutoMapper;
using ScrumPoker.API.Dtos;

namespace ScrumPoker.API.Controllers
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class SalaController : ControllerBase
    {
        private readonly ISalaRepository _repo;
        private readonly IMapper _mapper;

        public SalaController(ISalaRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalaDto>> BuscarPorIdAsync(string id)
        {
            var sala = await _repo.BuscarPorIdAsync(id);

            if (sala is null)
                return NotFound();

            return Ok(_mapper.Map<SalaDto>(sala));
        }

        [HttpPost]
        public async Task<ActionResult<SalaDto>> IncluiAsync([FromBody] SalaDto salaDto)
        {
            if (await _repo.ExisteEntityAsync(salaDto.Id))
                return BadRequest($"Id da sala {salaDto.Id}, já existe no banco de dados");

            var sala = await _repo.IncluirAsync(_mapper.Map<Sala>(salaDto));
            return Created(nameof(BuscarPorIdAsync), _mapper.Map<SalaDto>(sala));
        }

        [HttpPut]
        public async Task<ActionResult<SalaDto>> AlterarAsync([FromBody] SalaDto salaDto)
        {
            var sala = await _repo.BuscarPorIdAsync(salaDto.Id);
            if (sala is null) return NotFound();

            sala.Cartas = sala.Cartas.ToList();
            _mapper.Map(salaDto, sala);
            await _repo.AlterarAsync(sala);

            return Ok(_mapper.Map<SalaDto>(sala));
        }

        [HttpDelete("{id}/cartas")]
        public async Task<ActionResult> ExcluirCartasAsync(string id)
        {
            if ( !(await _repo.ExisteEntityAsync(id)) )
                return NotFound();

            await _repo.ExcluirCartasAsync(id);

            return NoContent();
        }
    }
}