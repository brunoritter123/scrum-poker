using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ScrumPoker.Domain.Identity;
using System.Threading.Tasks;
using ScrumPoker.API.Dtos;

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

        public AuthController(
            IConfiguration config,
            UserManager<User> userManager,
            SignInManager<User> singInManager,
            IMapper mapper)
        {
            this._userManager = userManager;
            this._singInManager = singInManager;
            this._mapper = mapper;
            this._config = config;
        }

        public async Task<ActionResult<User>> GetUser ()
        {
            return Ok(new User());
        }

        [HttpPost("Registrar")]
        public async Task<ActionResult> Registrar(UserDto user)
        {
            return Ok(new User());
        }
    }
}