using Microsoft.AspNetCore.Mvc;
using RankedTyping.Request;
using RankedTyping.Services;

namespace RankedTyping.Controllers
{
    [ApiController]
    public class ForgotPasswordController : Controller
    {
        private readonly IForgotPasswordService _forgotPasswordService;

        /**
         * Constructor
         */
        public ForgotPasswordController(IForgotPasswordService forgotPasswordService)
        {
            _forgotPasswordService = forgotPasswordService;
        }

        [HttpPost]
        [Route("/password/forgot")]
        public ActionResult SendResetEmail([FromBody] SendResetEmailRequest request)
        {
            var response = _forgotPasswordService.SendResetEmail(request);
            if (response == null)
                return BadRequest(new { message = "Unable to send reset password information" });
            return Ok(new {message = "Forgot password information sent to your email."});
        }

        [HttpPost]
        [Route("/password/change")]
        public ActionResult ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var response = _forgotPasswordService.ChangePassword(request);
            if (!response)
                return BadRequest(new { message = "Invalid reset token" });
            return Ok(new {message = "Your password has been reset! You may now use that password log in."});
        }
    }
}