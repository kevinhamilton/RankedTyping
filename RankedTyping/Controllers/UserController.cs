using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RankedTyping.Request;
using RankedTyping.Services;

namespace RankedTyping.Controllers
{
    [ApiController]
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
        [Authorize]
        [Route("/user/delete")]
        public ActionResult Delete()
        {
            var deleted = _userService.Delete(Convert.ToInt32(User.Identity.Name));
            if (!deleted) return BadRequest(new {message = "User not found."});
            return Ok();
        }
        
        [HttpPost]
        [Authorize]
        [Route("/user/change-password")]
        public ActionResult ChangePassword([FromBody] ResetPasswordRequest request)
        {
            var success = _userService.ChangePassword(Convert.ToInt32(User.Identity.Name), request);
            if (!success) return BadRequest(new {message = "User not found."});
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("/user/change-email")]
        public ActionResult ChangeEmail([FromBody] ChangeEmailRequest request)
        {
            var deleted = _userService.ChangeEmail(Convert.ToInt32(User.Identity.Name), request);
            if (!deleted) return BadRequest(new {message = "Email is already in use."});
            return Ok();
        }
        [HttpPost]
        [Route("/user/change-username")]
        public ActionResult ChangeUsername([FromBody] ChangeUsernameRequest request)
        {
            var deleted = _userService.ChangeUsername(Convert.ToInt32(User.Identity.Name), request);
            if (!deleted) return BadRequest(new {message = "Username already exists."});
            return Ok();
        }
    }
}