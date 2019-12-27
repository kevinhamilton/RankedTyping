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
        public LeaderboardResponse List();
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
        public LeaderboardResponse List()
        {
            // Key not in cache, so generate cache data.
            var leaders = _context.Results
                .OrderByDescending(r => r.Wpm)
                .ThenBy(r => r.Id)
                .Include(r => r.User)
                .Take(10)
                .ToList();

            var recent = _context.Results
                .OrderByDescending(r => r.Id)
                .Include(r => r.User)
                .Take(10)
                .ToList();

            var today = _context.Results
               // .Where(r => Convert.ToDateTime(r.CreatedAt).CompareTo( DateTime.Now.AddDays(-1) ) >= 0)
                .OrderByDescending(r => r.Id)
                .Include(r => r.User)
                .Take(10)
                .ToList();

            return new LeaderboardResponse { Leaders = leaders, Recent = recent, Today = today };
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