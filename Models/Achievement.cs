using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RankedTyping.Models
{
    [Table("achievements")]
    public class Achievement
    {
        [Column("id")] 
        public int Id { get; set; }
        
        [Column("title")] 
        public string Title { get; set; }
        
        [Column("description")] 
        public string Description { get; set; }
        
        [JsonPropertyName("achievement_type_id")]
        [Column("achievement_type_id")] 
        public int AchievementTypeId { get; set; }
        
        [Column("count")] 
        public int Count { get; set; }
        
        [Column("points")] 
        public int Points { get; set; }
        
        [Column("created_at")] 
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
        
        [Column("updated_at")] 
        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}