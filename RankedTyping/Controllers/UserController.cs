using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RankedTyping.Models;

namespace RankedTyping.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/user")]
    public class UserController : ControllerBase
    {
        private readonly RankedContext _context;

        public UserController(RankedContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Fetch()
        {
            var user = _context.Users.SingleOrDefault(x => x.Id == Convert.ToInt32(User.Identity.Name));
            if (user == null) return BadRequest(new {message = "User not found."});
            return Ok(user);
        }

        [HttpPost]
        [Route("/user/delete")]
        public ActionResult Delete()
        {
            var user = _context.Users.SingleOrDefault(x => x.Id == Convert.ToInt32(User.Identity.Name));
            if (user == null) return BadRequest(new {message = "User not found."});
            
            _context.Users.Remove(user);
            _context.SaveChanges();
            
            return Ok();
        }
        
        [HttpGet]
        public ActionResult Achievements()
        {
            var user = _context.Users.SingleOrDefault(x => x.Id == Convert.ToInt32(User.Identity.Name));
            if (user == null) return BadRequest(new {message = "User not found."});
            
            var list = _context.Achievements
                .OrderByDescending(a => a.Id)
                .ToList();
            
            return Ok(list);
        }
    }
}