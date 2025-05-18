using Microsoft.EntityFrameworkCore;
using W31UL9_HSZF_2024252.Model;

namespace W31UL9_HSZF_2024252.Persistence.MsSql
{


    public class AppDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; } 
        public DbSet<Trip> Trips { get; set; }

        public AppDbContext()
        {
            
            //this.Database.EnsureDeleted();
           
            this.Database.EnsureCreated(); 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=carSharingDb;Integrated Security=True;MultipleActiveResultSets=true";
            optionsBuilder.UseSqlServer(connStr);
            base.OnConfiguring(optionsBuilder);
        }

    }
}
