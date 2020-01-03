using System;
using System.Collections.Generic;
using System.Linq;
using RankedTyping.Models;

namespace RankedTyping.Utils
{
    public class CheckForAchievements
    {
        private readonly RankedContext _context;

        private readonly int _userId;

        private int _newAchievementPoints;
        
        public CheckForAchievements(RankedContext context, int userId)
        {
            _context = context;
            _userId = userId;
            _newAchievementPoints = 0;
        }
        
        public void Check()
        {
            var stats = _context.Results
                .Where(r => r.UserId == _userId)
                .GroupBy(u => u.UserId)
                .Select(g => new { 
                    AverageWpm = g.Average(i => i.Wpm),
                    HighestWpm = g.Max(i => i.Wpm),
                    AverageAccuracy = g.Average(i => i.Accuracy),
                    TestsTaken = g.Count(),
                })
                .FirstOrDefault();

            var achievements = _context.Achievements
                .Where(a => a.AchievementTypeId == AchievementType.Count)
                .Where(a => a.Count <= stats.TestsTaken)
                .ToList();
            
            //Sum and prepare the missing achievements
            AddMissingAchievements(achievements);
            
            //Highest WPM achieve
            var wpmAchievements = _context.Achievements
                .Where(a => a.AchievementTypeId == AchievementType.Wpm)
                .Where(a => a.Count <= stats.HighestWpm)
                .ToList();
            
            AddMissingAchievements(wpmAchievements);
            
            //Reload user info
            var user = _context.Users.FirstOrDefault(m => m.Id == _userId);
            if (user != null && stats != null)
            {
                user.HighestWpm = Convert.ToInt16(stats.HighestWpm);
                user.AverageWpm = Convert.ToInt16(stats.AverageWpm);
                user.AverageAccuracy = (int) stats.AverageAccuracy;
                user.TestsTaken = stats.TestsTaken;
                user.AchievementPoints = user.AchievementPoints + _newAchievementPoints;
                user.UpdatedAt = DateTime.Now;

                //save
                _context.Users.Update(user);
                _context.SaveChanges();
            }
        }

        /**
         * Add Missing achievements records. Sum up the total of new achievement
         * points to be added later.
         */
        private void AddMissingAchievements(List<Achievement> achievements)
        {
            foreach(var achievement in achievements)
            {
                //create if it doesnt already exist.
                var exists = _context.UserAchievements
                    .Where(u => u.UserId == _userId)
                    .FirstOrDefault(u => u.AchievementId == achievement.Id);

                //Save if it doesnt exist
                if (exists == null)
                {
                    var newAchievement = new UserAchievement()
                    {
                        AchievementId = achievement.Id,
                        UserId = _userId,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    _context.UserAchievements.Add(newAchievement);
                    
                    _newAchievementPoints += achievement.Points;
                }
            }
            
        }
    }
}