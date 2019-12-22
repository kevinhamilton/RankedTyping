using Microsoft.AspNetCore.Mvc;
using RankedTyping.Services;

namespace RankedTyping.Controllers
{
    [ApiController]
    [Route("/test")]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;

        /**
         * Constructor
         */
        public TestController(ITestService testService)
        {
            _testService = testService;
        }
        
        // GET /
        [HttpGet]
        public ActionResult Fetch(int language = 1)
        {
            var response = _testService.Fetch(language);
            if (response == null) return BadRequest(new {message = "Test not found."});
            return Ok(response);
        }
    }
}