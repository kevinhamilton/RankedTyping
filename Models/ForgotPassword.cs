using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RankedTyping.Models
{
    [Table("forgot_passwords")]
    public class ForgotPassword
    {
        [Column("email")]
        [JsonPropertyName("email")]
        public string Email { get; set; }
        
        [Column("token")]
        [JsonPropertyName("token")]
        public string Token { get; set; }
        
        [Column("created_at")] 
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [Column("updated_at")] 
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }
        
    }
}