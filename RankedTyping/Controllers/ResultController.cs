using System;
using Microsoft.AspNetCore.Mvc;
using RankedTyping.Request;
using RankedTyping.Services;

namespace RankedTyping.Controllers
{
    [ApiController]
    [Route("/result")]
    public class ResultController : ControllerBase
    {
        
        private readonly IResultService _resultService;

        /**
         * Constructor
         */
        public ResultController(IResultService resultService)
        {
            _resultService = resultService;
        }

        [HttpPost]
        public ActionResult Store([FromBody] ResultRequest request)
        {
            var userId = Convert.ToInt32(User.Identity.Name);
            var result = _resultService.Store(userId, request);
            return Ok(result);
        }
    }
}