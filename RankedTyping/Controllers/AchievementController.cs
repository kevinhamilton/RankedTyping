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
    [Route("/achievements")]
    public class AchievementController : ControllerBase
    {
        
        private readonly RankedContext _context;

        /**
         * Constructor
         */
        public AchievementController(RankedContext context)
        {
            _context = context;
        }
        
        // GET /
        [HttpGet]
        public async Task<OkObjectResult> List()
        {
            var list = await _context.Achievements
                .OrderByDescending(a => a.Id)
                .ToListAsync();
            
            return Ok(list);
        }
    }
}