using System.ComponentModel.DataAnnotations;

namespace RankedTyping.Request
{
    public class ResultRequest
    {
        [Required]
        public double wpm { get; set; }
        
        [Required]
        public int good_keystrokes { get; set; }
        
        [Required]
        public int bad_keystrokes { get; set; }
        
        [Required]
        public int total_possible_keystrokes { get; set; }
        
        [Required]
        public int errors { get; set; }
        
        [Required]
        public int test_id { get; set; }
        
        [Required]
        public decimal accuracy { get; set; }
    }
}