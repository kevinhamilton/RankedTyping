using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RankedTyping.Models;
using RankedTyping.Responses;

namespace RankedTyping.Services
{
    public interface ILeaderboardService
    {
        public Task<LeaderboardResponse> List();
        public Task<List<User>> ListByAchievements();
    }

    public class LeaderboardService : ILeaderboardService
    {
        private readonly IMemoryCache _cache;

        private readonly RankedContext _context;

        public LeaderboardService(RankedContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        /**
         * List the top 10 users of the day, of all time and the last 10
         */
        public async Task<LeaderboardResponse> List()
        {
            var cacheKey = "leaderboard";
            
            LeaderboardResponse results;

            // Look for cache key.
            if (! _cache.TryGetValue(cacheKey, out results))
            {
                // Key not in cache, so generate cache data.
                var leaders = await _context.Results
                    .OrderByDescending(r => r.Wpm)
                    .ThenBy(r => r.Id)
                    .Include(r => r.User)
                    .Take(10)
                    .ToListAsync();

                var recent = await _context.Results
                    .OrderByDescending(r => r.Id)
                    .Include(r => r.User)
                    .Take(10)
                    .ToListAsync();

                var today = await _context.Results
                    .Where(r => Convert.ToDateTime(r.CreatedAt).CompareTo(DateTime.Now.AddDays(-1)) > 1)
                    .OrderByDescending(r => r.Id)
                    .Include(r => r.User)
                    .Take(10)
                    .ToListAsync();

                results = new LeaderboardResponse { Leaders = leaders, Recent = recent, Today = today };

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(10));

                // Save data in cache.
                _cache.Set(cacheKey, results, cacheEntryOptions);
            }

            return results;
        }

        /**
         * List winning users by achievement points
         */
        public async Task<List<User>> ListByAchievements()
        {
            return await _context.Users
                .OrderByDescending(u => u.AchievementPoints)
                .Take(10)
                .ToListAsync();
        }
    }
}