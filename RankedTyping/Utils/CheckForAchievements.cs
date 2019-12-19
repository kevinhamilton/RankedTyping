using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RankedTyping.Models;

namespace RankedTyping.Utils
{
    public class CheckForAchievements
    {
        private RankedContext _context;
        
        public CheckForAchievements(RankedContext context)
        {
            _context = context;
        }
        
        public void Check(int userId)
        {
            var newAchievementPoints = 0;

            var stats = _context.Results
                .Where(r => r.UserId == userId)
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
            
            foreach(var achievement in achievements)
            {
                //create if it doesnt already exist.
                var exists = _context.UserAchievements
                    .Where(u => u.UserId == userId)
                    .FirstOrDefault(u => u.AchievementId == achievement.Id);

                //Save if it doesnt exist
                if (exists == null)
                {
                    var newAchievement = new UserAchievement()
                    {
                        AchievementId = achievement.Id,
                        UserId = userId
                    };
                    _context.UserAchievements.Add(newAchievement);
                    
                    newAchievementPoints += achievement.Points;
                }
            }
            
            
            //Highest WPM achieve
            var wpmAchievements = _context.Achievements
                .Where(a => a.AchievementTypeId == AchievementType.Wpm)
                .Where(a => a.Count <= stats.HighestWpm)
                .ToList();
            
            foreach(var achievement in wpmAchievements)
            {
                //create if it doesnt already exist.
                var exists = _context.UserAchievements
                    .Where(u => u.UserId == userId)
                    .FirstOrDefault(u => u.AchievementId == achievement.Id);

                //Save if it doesnt exist
                if (exists == null)
                {
                    var newAchievement = new UserAchievement()
                    {
                        AchievementId = achievement.Id,
                        UserId = userId
                    };
                    _context.UserAchievements.Add(newAchievement);
                    
                    newAchievementPoints += achievement.Points;
                }
            }
            
            //Reload user info
            var user = _context.Users.FirstOrDefault(m => m.Id == userId);
            if (user != null && stats != null)
            {
                user.HighestWpm = Convert.ToInt16(stats.HighestWpm);
                user.AverageWpm = Convert.ToInt16(stats.AverageWpm);
                user.AverageAccuracy = (int) stats.AverageAccuracy;
                user.TestsTaken = stats.TestsTaken;
                user.AchievementPoints = user.AchievementPoints + newAchievementPoints;
                user.UpdatedAt = DateTime.Now;

                //save
                _context.Users.Update(user);
            }
        }
    }
}