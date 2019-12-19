#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RankedTyping.Models
{
    [Table("results")]
    public class Result
    {
        [Column("id")] 
        [Key]
        public int Id { get; set; }

        [Column("user_id")]
        [JsonPropertyName("user_id")]
        public int? UserId { get; set; }
                
        [Column("test_id")]
        [JsonPropertyName("test_id")]
        public int TestId { get; set; }
        
        [Column("wpm")]
        public int Wpm { get; set; }
        
        [Column("good_keystrokes")]
        [JsonPropertyName("good_keystrokes")]
        public int GoodKeystrokes { get; set; }   
        
        [Column("bad_keystrokes")]
        [JsonPropertyName("bad_keystrokes")]
        public int BadKeystrokes { get; set; }   
        
        [Column("total_possible_keystrokes")]
        [JsonPropertyName("total_possible_keystrokes")]
        public int TotalPossibleKeystrokes { get; set; }   
        
        [Column("errors")]
        public int Errors { get; set; }   
        
        [Column("accuracy")]
        public decimal Accuracy { get; set; }   
        
        [Column("created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }   
        
        [Column("updated_at")]
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }   

        [JsonPropertyName("user")]
        public User? User { get; set; }
    }
}