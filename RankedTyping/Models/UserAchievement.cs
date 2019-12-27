using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RankedTyping.Models
{
    [Table("user_achievements")]
    public class UserAchievement
    {
        [Column("user_id")]
        public int UserId { get; set; }
        
        [Column("achievement_id")]
        public int AchievementId { get; set; }
        
        [Column("created_at")] 
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
        
        [Column("updated_at")] 
        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}