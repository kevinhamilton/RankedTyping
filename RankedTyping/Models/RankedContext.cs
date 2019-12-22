using Microsoft.EntityFrameworkCore;

namespace RankedTyping.Models
{
    public class RankedContext : DbContext
    {

        public RankedContext(DbContextOptions<RankedContext> options) : base(options)
        {
            
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }
        
        public DbSet<Result> Results { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        
        public DbSet<ForgotPassword> ForgotPasswords { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestType> TestTypes { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAchievement>()
                .HasKey(c => new { c.UserId, c.AchievementId });
            
            modelBuilder.Entity<ForgotPassword>()
                .HasKey(c => new { c.Email, c.Token });
        }
    }
}