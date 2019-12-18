using Microsoft.EntityFrameworkCore;

namespace RankedTyping.Models
{
    public class RankedContext : DbContext
    {
        public RankedContext()
        {
            
        }

        public RankedContext(DbContextOptions<RankedContext> options) : base(options)
        {
            
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }
        
        public DbSet<Result> Results { get; set; }
        public DbSet<User> Users { get; set; }
    }
}