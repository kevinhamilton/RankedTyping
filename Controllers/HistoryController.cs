using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RankedTyping.Services;

namespace RankedTyping.Controllers
{
    [ApiController]
    [Route("/history")]
    public class HistoryController : ControllerBase
    {
        private readonly IResultService _resultService;

        /**
         * Constructor
         */
        public HistoryController(IResultService resultService)
        {
            _resultService = resultService;
        }
        
        // GET /
        [HttpGet]
        public async Task<OkObjectResult> List(int page = 1, int size = 20)
        {
            var results = _resultService.List(page, size);
            return Ok(results);
        }
        
        [HttpGet]
        [Route("/user/history")]
        public async Task<OkObjectResult> UserHistory(int page = 1, int size = 20)
        {
            var userId = Convert.ToInt32(User.Identity.Name);
            var results = _resultService.UserHistory(userId, page, size);
            return Ok(results);
        }
    }
}