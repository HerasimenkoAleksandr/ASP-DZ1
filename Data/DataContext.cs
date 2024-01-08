using Microsoft.EntityFrameworkCore;

namespace ASP_DZ1.Data
{
    public class DataContext : DbContext
    {
        public DbSet <Entities.User> Users {  get; set; }

        public DataContext (DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("ASP_DZ1");
        }

    }
}
