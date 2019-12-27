using System;
using System.Linq;
using RankedTyping.Models;
using RankedTyping.Request;
using RankedTyping.Utils;

namespace RankedTyping.Services
{
    public interface IResultService
    {
        public Result Store(int userId, ResultRequest request);
        public PagedResult<Result> List(int page = 1, int size = 20);
        public PagedResult<Result> UserHistory(int userId, int page = 1, int size = 20);
    }

    public class ResultService : IResultService
    {
        private readonly RankedContext _context;

        public ResultService(RankedContext context)
        {
            _context = context;
        }

        public Result Store(int userId, ResultRequest request)
        {
            var result = new Result()
            {
                UserId = (userId > 0) ? userId : null as int?,
                Wpm = Convert.ToInt32(request.wpm),
                TestId = request.test_id,
                GoodKeystrokes = request.good_keystrokes,
                BadKeystrokes = request.bad_keystrokes,
                TotalPossibleKeystrokes = request.total_possible_keystrokes,
                Errors = request.errors,
                Accuracy = request.accuracy,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Results.Add(result);
            
            _context.SaveChanges();

            //If a registered user did this, check for new achievements.
            if (userId > 0)
            {
                //todo: maybe abstract to achievement service
                var checker = new CheckForAchievements(_context, userId);
                checker.Check();
            }

            return result;
        }
        
        public PagedResult<Result> List(int page = 1, int size = 20)
        {
            // Determine the number of records to skip
            var skip = (page - 1) * size;
            var take = size;

            // Select the records based on paging parameters
            var records = _context.Results
                .Skip(skip)
                .Take(take)
                .ToList();

            // Get total number of records
            var count = _context.Results.Count();
            
            // Prepare paginated response.
            return new PagedResult<Result>
            {
                CurrentPage = page,
                FirstPage = 1,
                LastPage = (int) Math.Ceiling(Decimal.Divide(count, size)),
                NextPage = Math.Max(page + 1, 1),
                PreviousPage = Math.Max(page - 1, 1),
                PageSize = size,
                PageCount = count,
                Results = records
            };
        }
        
        public PagedResult<Result> UserHistory(int userId, int page = 1, int size = 20)
        {
            // Determine the number of records to skip
            var skip = (page - 1) * size;
            var take = size;

            // Select the records based on paging parameters
            var records = _context.Results
                .Where(r => r.UserId == userId)
                .OrderByDescending(o => o.Id)
                .Skip(skip)
                .Take(take)
                .ToList();

            // Get total number of records
            var count = _context.Results
                .Count(r => r.UserId == userId);
            
            // Prepare paginated response.
            return new PagedResult<Result>
            {
                CurrentPage = page,
                FirstPage = 1,
                LastPage = (int) Math.Ceiling(Decimal.Divide(count, size)),
                NextPage = Math.Max(page + 1, 1),
                PreviousPage = Math.Max(page - 1, 1),
                PageSize = size,
                PageCount = count,
                Results = records
            };
        }

    }

}