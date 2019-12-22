using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RankedTyping.Services;

namespace RankedTyping.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult Fetch()
        {
            var user = _userService.Fetch(Convert.ToInt32(User.Identity.Name));
            if (user == null) return BadRequest(new {message = "User not found."});
            return Ok(user);
        }

        [HttpPost]
        [Route("/user/delete")]
        public ActionResult Delete()
        {
            var deleted = _userService.Delete(Convert.ToInt32(User.Identity.Name));
            if (!deleted) return BadRequest(new {message = "User not found."});
            return Ok();
        }
    }
}