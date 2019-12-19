using Microsoft.AspNetCore.Mvc;
using RankedTyping.Request;
using RankedTyping.Services;

namespace RankedTyping.Controllers
{
    [ApiController]
    [Route("/login")]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public LoginController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpPost]
        public ActionResult Login([FromBody] LoginRequest request)
        {
            var user = _userService.Authenticate(request.email, request.password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }
    }
}