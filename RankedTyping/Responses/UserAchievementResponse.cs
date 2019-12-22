using System.Collections.Generic;
using RankedTyping.Models;

namespace RankedTyping.Responses
{
    public class UserAchievementResponse
    {
        public List<Achievement> achievements { get; set; }
        public List<UserAchievement> user_achievements { get; set; }
    }
}