using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RankedTyping.Models
{
    [Table("users")]
    public class User
    {
        [Column("id")] 
        public int Id { get; set; }
        
        [Column("username")] 
        public string Username { get; set; }
        
        [Column("password")] 
        [JsonIgnore]
        public string Password { get; set; }
        
        [Column("remember_token")] 
        [JsonPropertyName("remember_token")]
        public string RememberToken { get; set; }
        
        [Column("average_wpm")] 
        [JsonPropertyName("average_wpm")]
        public int AverageWpm { get; set; }
        
        [Column("achievement_points")] 
        [JsonPropertyName("achievement_points")]
        public int AchievementPoints { get; set; }
        
        [Column("average_accuracy")] 
        [JsonPropertyName("average_accuracy")]
        public int AverageAccuracy { get; set; }
        
        public string Email { get; set; }
        
        [Column("email_md5")] 
        [JsonPropertyName("email_md5")]
        public string EmailMd5 { get; set; }
        
        [Column("highest_wpm")] 
        [JsonPropertyName("highest_wpm")]
        public int HighestWpm { get; set; }
        
        [Column("tests_taken")] 
        [JsonPropertyName("tests_taken")]
        public int TestsTaken { get; set; }
        
        [Column("created_at")] 
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [Column("updated_at")] 
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }
        
        [NotMapped]
        public string? Token { get; set; }
    }
}