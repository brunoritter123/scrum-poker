using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ScrumPoker.Domain.Interfaces.Repositories;
using ScrumPoker.Domain.Models;
using System.Threading.Tasks;
using AutoMapper;
using ScrumPoker.API.Dtos;
using Microsoft.AspNetCore.Identity;
using ScrumPoker.Domain.Identity;
using Microsoft.AspNetCore.Hosting;

namespace ScrumPoker.API.Controllers
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PerfilController : ControllerBase
    {
        private readonly IPerfilRepository _repo;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _env;

        public PerfilController(
            IPerfilRepository repo,
            IMapper mapper,
            UserManager<User> userManager,
            IWebHostEnvironment env
            )
        {
            _repo = repo;
            _mapper = mapper;
            _userManager = userManager;
            _env = env;
        }

        [HttpGet("{userName}")]
        public async Task<ActionResult<PerfilDto>> BuscarPorIdAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user is null)
                return NotFound();

            var perfil = await _repo.BuscarPorIdAsync(user.PerfilId);
            return Ok(_mapper.Map<PerfilDto>(perfil));
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<PerfilDto>> AlterarAsync([FromBody] PerfilDto perfilDto)
        {
            var perfil = await _repo.BuscarPorIdAsync(perfilDto.Id);
            if (perfil is null) return NotFound();

            _mapper.Map(perfilDto, perfil);
            await _repo.AlterarAsync(perfil);

            return Ok(_mapper.Map<PerfilDto>(perfil));
        }

        // [HttpPost]
        // public async Task<string> AlteraImagem(FileUPloadAPI objfile)
        // {
        //     IFormFile
        // }
    }
}