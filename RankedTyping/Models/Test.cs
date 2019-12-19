using System.ComponentModel.DataAnnotations.Schema;

namespace RankedTyping.Models
{
    public class Test
    {
        [Column("id")] 
        public int Id { get; set; }
    }
}