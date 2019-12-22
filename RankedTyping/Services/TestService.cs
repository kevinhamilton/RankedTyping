using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using RankedTyping.Models;
using RankedTyping.Responses;

namespace RankedTyping.Services
{
    public interface ITestService
    {
        public TestResponse Fetch(int language = 1);
    }

    public class TestService : ITestService
    {
        private readonly IMemoryCache _cache;

        private readonly RankedContext _context;

        public TestService(RankedContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public TestResponse Fetch(int language = 1)
        {
            var cacheKey = "cache-lang-" + language;
            
            List<Test> cachedResults;

            // Look for cache key.
            if (! _cache.TryGetValue(cacheKey, out cachedResults))
            {
                // Key not in cache, so generate cache data.
                cachedResults = _context.Tests
                    .Where(t => t.LanguageId == language)
                    .ToList();

                // Set cache options. I really don't want this to expire as new tests are never added.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30));

                // Save data in cache.
                _cache.Set(cacheKey, cachedResults, cacheEntryOptions);
            }
            
            var random = new Random();
            var index = random.Next(cachedResults.Count);
            var record = cachedResults[index];
            
            var response = new TestResponse();
            if (record != null)
            {
                response.id = record.Id;
                response.language_id = record.LanguageId;
                response.words = record.Words.Split();
                response.test_type_id = record.TestTypeId;
            
                return response;
            }

            return null;
        }
    }
}