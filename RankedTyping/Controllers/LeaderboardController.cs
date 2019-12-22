using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RankedTyping.Services;

namespace RankedTyping.Controllers
{
    [ApiController]
    [Route("/leaderboard")]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardService _leaderboardService;

        /**
         * Constructor
         */
        public LeaderboardController(ILeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }

        [HttpGet]
        public ActionResult List()
        {
            var result = _leaderboardService.List();
            return Ok(result);
        }

        [Route("/leaderboard/achievements")]
        [HttpGet]
        public ActionResult Achievements()
        {
            var leaders = _leaderboardService.ListByAchievements();
            return Ok(leaders);
        }
    }
}