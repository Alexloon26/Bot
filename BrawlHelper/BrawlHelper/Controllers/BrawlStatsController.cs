using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using BrawlHelper.Model;
using BrawlHelper.Models;

namespace BrawlHelper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrawlStatsController : ControllerBase
    {
        private readonly ILogger<BrawlStatsController> _logger;

        public BrawlStatsController(ILogger<BrawlStatsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{playerId}")]
        public async Task<IActionResult> GetPlayerStats(string playerId)
        {
            var brawlClient = new BrawlClient();
            var playerStats = await brawlClient.GetPlayerInfo(playerId);
            return Ok(playerStats);
        }

        [HttpGet("brawlers")]
        public async Task<IActionResult> GetBrawlers()
        {
            var brawlClient = new BrawlClient();
            var brawlers = await brawlClient.GetBrawlers();
            return Ok(brawlers);
        }
    }
}

