using Microsoft.AspNetCore.Mvc;
using RankedTyping.Request;
using RankedTyping.Services;

namespace RankedTyping.Controllers
{
    [ApiController]
    [Route("/password/forgot")]
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
        public ActionResult SendResetEmail([FromBody] SendResetEmailRequest request)
        {
            var response = _forgotPasswordService.SendResetEmail(request);
            if (response == null)
                return BadRequest(new { message = "Unable to send reset password information" });
            return Ok();
        }
        
        [HttpPost]
        [Route("/password/forgot/change")]
        public ActionResult ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var response = _forgotPasswordService.ChangePassword(request);
            if (!response)
                return BadRequest(new { message = "Invalid reset token" });
            return Ok();
        }
    }
}