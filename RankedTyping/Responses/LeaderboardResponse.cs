using System.Collections.Generic;
using RankedTyping.Models;

namespace RankedTyping.Responses
{
    public class LeaderboardResponse
    {
        public IList<Result> Leaders { get; set; } = new List<Result>();
        public IList<Result> Recent { get; set; } = new List<Result>();
        public IList<Result> Today { get; set; } = new List<Result>();
    }
}