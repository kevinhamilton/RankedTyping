using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RankedTyping.Models
{
    public class Language
    {
        [Column("id")]
        public int Id { get; set; }
        
        [Column("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [Column("created_at")] 
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [Column("updated_at")] 
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}