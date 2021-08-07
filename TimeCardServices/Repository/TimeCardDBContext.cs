using Microsoft.EntityFrameworkCore;
using TimeCardServices.Model;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace TimeCardServices.Repository
{
    public class TimeCardDBContext : DbContext 
    {
        public TimeCardDBContext(DbContextOptions<TimeCardDBContext> options)
                : base(options)


        { }
         public DbSet<User> Users { get; set; }
        public DbSet<TimeCard> TimeCards { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimeCard>().HasKey(t => new { t.UserName, t.WeekStart });
            base.OnModelCreating(modelBuilder);
        }
    }
  
}
