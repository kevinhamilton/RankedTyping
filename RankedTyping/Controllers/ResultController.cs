using System;
using Microsoft.AspNetCore.Mvc;
using RankedTyping.Models;
using RankedTyping.Request;
using RankedTyping.Utils;

namespace RankedTyping.Controllers
{
    [ApiController]
    [Route("/result")]
    public class ResultController : ControllerBase
    {

        private readonly RankedContext _context;

        public ResultController(RankedContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult Store([FromBody] ResultRequest request)
        {
            var userId = Convert.ToInt32(User.Identity.Name);
            var result = new Result()
            {
                UserId = (userId > 0) ? userId : null as int?,
                Wpm = request.wpm,
                TestId = request.test_id,
                GoodKeystrokes = request.good_keystrokes,
                BadKeystrokes = request.bad_keystrokes,
                TotalPossibleKeystrokes = request.total_possible_keystrokes,
                Errors = request.errors,
                Accuracy = request.accuracy,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Results.Add(result);

            //If a registered user did this, check for new achievements.
            if (userId > 0)
            {
                var checker = new CheckForAchievements(_context, userId);
                checker.Check();
            }
            
            _context.SaveChanges();

            return Ok(result);
        }
    }
}