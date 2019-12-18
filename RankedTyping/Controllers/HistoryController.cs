using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RankedTyping.Models;
using RankedTyping.Utils;

namespace RankedTyping.Controllers
{
    [ApiController]
    [Route("/history")]
    public class HistoryController : ControllerBase
    {
        
        private readonly RankedContext _context;

        /**
         * Constructor
         */
        public HistoryController(RankedContext context)
        {
            _context = context;
        }
        
        // GET /
        [HttpGet]
        public async Task<OkObjectResult> List(int page = 1, int size = 20)
        {
            // Determine the number of records to skip
            var skip = (page - 1) * size;
            var take = size;

            // Select the records based on paging parameters
            var records = await _context.Results
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            // Get total number of records
            var count = await _context.Results.CountAsync();
            
            // Prepare paginated response.
            return Ok(new PagedResult<Result>
            {
                CurrentPage = page,
                FirstPage = 1,
                LastPage = (int) Math.Ceiling(Decimal.Divide(count, size)),
                NextPage = Math.Max(page + 1, 1),
                PreviousPage = Math.Max(page - 1, 1),
                PageSize = size,
                PageCount = count,
                Results = records
            });
        }
    }
}