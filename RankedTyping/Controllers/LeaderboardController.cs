using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RankedTyping.Models;
using RankedTyping.Responses;

namespace RankedTyping.Controllers
{
    [ApiController]
    [Route("/leaderboard")]
    public class LeaderboardController : ControllerBase
    {

        private readonly RankedContext _context;

        /**
         * Constructor
         */
        public LeaderboardController(RankedContext context)
        {
            _context = context;
        }

        // GET /
        [HttpGet]
        public async Task<ActionResult> List()
        {
            var leaders = await _context.Results
                .OrderByDescending(r => r.Wpm)
                .ThenBy(r => r.Id)
                .Include(r => r.User)
                .Take(10)
                .ToListAsync();

            var recent = await _context.Results
                .OrderByDescending(r => r.Id)
                .Include(r => r.User)
                .Take(10)
                .ToListAsync();

            var today = await _context.Results
                .Where(r => Convert.ToDateTime(r.CreatedAt).CompareTo(DateTime.Now.AddDays(-1)) > 1)
                .OrderByDescending(r => r.Id)
                .Include(r => r.User)
                .Take(10)
                .ToListAsync();

            return Ok(new LeaderboardResponse { Leaders = leaders, Recent = recent, Today = today });
        }

        // GET /leaderboard/achievements
        [Route("/leaderboard/achievements")]
        [HttpGet]
        public async Task<ActionResult> Achievements()
        {
            var leaders = await _context.Users
                .OrderByDescending(u => u.AchievementPoints)
                .Take(10)
                .ToListAsync();

            return Ok(leaders);
        }
    }
}