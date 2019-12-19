using System.ComponentModel.DataAnnotations.Schema;

namespace RankedTyping.Models
{
    public class TestType
    {
        [Column("id")] 
        public int Id { get; set; }
    }
}