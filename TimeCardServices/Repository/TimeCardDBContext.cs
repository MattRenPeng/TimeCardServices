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
         public Microsoft.EntityFrameworkCore.DbSet<User> Users { get; set; }
    }
  
}
