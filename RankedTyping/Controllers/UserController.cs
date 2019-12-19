using System;
using System.Linq;
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
            return Ok(user);
        }
    }
}