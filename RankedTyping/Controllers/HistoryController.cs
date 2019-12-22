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
    }
}