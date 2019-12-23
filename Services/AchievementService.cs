using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RankedTyping.Models;
using RankedTyping.Responses;

namespace RankedTyping.Services
{
    public interface IAchievementService
    {
        public List<Achievement> LoadAchievements();
        public UserAchievementResponse UserAchievements(int userId);
    }

    public class AchievementService : IAchievementService
    {
        private readonly RankedContext _context;
        
        private readonly IMemoryCache _cache;

        public AchievementService(RankedContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public UserAchievementResponse UserAchievements(int userId)
        {
            var response = new UserAchievementResponse();
            response.achievements = LoadAchievements();
            response.user_achievements = _context.UserAchievements
                .Where(u => u.UserId == userId)
                .ToList();
            return response;
        }

        public List<Achievement> LoadAchievements()
        {
            var cacheKey = "achievements";
            List<Achievement> results;
            
            // Look for cache key.
            if (! _cache.TryGetValue(cacheKey, out results))
            {
                // Key not in cache, so generate cache data.
                results = _context.Achievements
                    .OrderByDescending(a => a.Id)
                    .ToList();

                // Set cache options. I really don't want this to expire as new tests are never added.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30));

                // Save data in cache.
                _cache.Set(cacheKey, results, cacheEntryOptions);
            }
            
            return results;
        }
    }
}