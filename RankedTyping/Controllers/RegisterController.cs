using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RankedTyping.Request;
using RankedTyping.Services;

namespace RankedTyping.Controllers
{
    [ApiController]
    [Route("/register")]
    public class RegisterController : ControllerBase
    {
        private readonly IUserService _userService;

        public RegisterController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpPost]
        public ActionResult Store([FromBody] RegisterRequest request)
        {
            var user = _userService.Register(request.email, request.password, request.username);

            if (user == null)
                return BadRequest(new { message = "Unable to create an account." });
            
            return Ok(user);
        }
    }
}