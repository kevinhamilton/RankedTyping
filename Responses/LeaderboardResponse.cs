using System.Collections.Generic;
using RankedTyping.Models;

namespace RankedTyping.Responses
{
    public class LeaderboardResponse
    {
        public List<Result> Leaders { get; set; }
        public List<Result> Recent { get; set; }
        public List<Result> Today { get; set; }
    }
}