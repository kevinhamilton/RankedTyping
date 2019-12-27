using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RankedTyping.Request;
using RankedTyping.Services;

namespace RankedTyping.Controllers
{
    [ApiController]
    [Route("/contact")]
    public class ContactController : ControllerBase
    {
        private IContactService _contactService;

        /**
         * Constructor
         */
        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpPost]
        public ActionResult Contact([FromBody] ContactRequest request)
        {
            var success = _contactService.SendContactNotification(request);
            if(!success){
                return BadRequest(new {message = "Unable to send contact message."});
            }
            return Ok(new {message = "Thank you for your feedback! We will review it shortly."});
        }
    }
}