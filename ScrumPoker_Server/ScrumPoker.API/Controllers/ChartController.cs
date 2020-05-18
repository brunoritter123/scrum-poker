using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ScrumPoker.API.DataStorage;
using ScrumPoker.API.HubConfig;
using ScrumPoker.API.TimerFeatures;

namespace ScrumPoker.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly IHubContext<ChartHub> _hub;

        public ChartController(IHubContext<ChartHub> hub)
        {
            _hub = hub;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var timerManager = new TimerManager(
                () => _hub.Clients.All.SendAsync("transferchartdata", DataManager.GetData())
            );

            return Ok(new {Message = "Request Completed"});
        }
    }
}