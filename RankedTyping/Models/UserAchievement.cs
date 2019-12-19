using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RankedTyping.Models
{
    [Table("user_achievements")]
    public class UserAchievement
    {
        [Column("user_id")]
        public int UserId { get; set; }
        
        [Column("achievement_id")]
        public int AchievementId { get; set; }
    }
}