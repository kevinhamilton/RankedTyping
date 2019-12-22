using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RankedTyping.Request;

namespace RankedTyping.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/contact")]
    public class ContactController : ControllerBase
    {
        [HttpPost]
        public ActionResult Contact([FromBody] ContactRequest request)
        {
            //todo: send mail.
            return Ok();
        }
    }
}