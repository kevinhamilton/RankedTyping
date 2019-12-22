using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RankedTyping.Models
{
    [Table("tests")]
    public class Test
    {
        [Column("id")] 
        public int Id { get; set; }
        
        [Column("test_type_id")] 
        [JsonPropertyName("test_type_id")] 
        public int TestTypeId { get; set; }
        
        [Column("words")] 
        [JsonPropertyName("words")] 
        public string Words { get; set; }
        
        [Column("language_id")] 
        [JsonPropertyName("language_id")] 
        public int LanguageId { get; set; }
        
        [Column("created_at")] 
        [JsonPropertyName("created_at")] 
        public DateTime CreatedAt { get; set; }
        
        [Column("updated_at")] 
        [JsonPropertyName("updated_at")] 
        public DateTime UpdatedAt { get; set; }
    }
}