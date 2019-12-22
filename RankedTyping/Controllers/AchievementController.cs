using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using RankedTyping.Services;

namespace RankedTyping.Controllers
{
    [ApiController]
    [Route("/achievements")]
    public class AchievementController : ControllerBase
    {
        private IAchievementService _achievementService;

        /**
         * Constructor
         */
        public AchievementController(IAchievementService achievementService)
        {
            _achievementService = achievementService;
        }
        
        // GET /
        [HttpGet]
        public ActionResult List()
        {
            var results = _achievementService.LoadAchievements();
            return Ok(results);
        }

        [HttpGet]
        [Authorize]
        [Route("/user/achievements")]
        public ActionResult UserAchievements()
        {
            var userId = Convert.ToInt32(User.Identity.Name);
            var result = _achievementService.UserAchievements(userId);
            return Ok(result);
        }
    }
}