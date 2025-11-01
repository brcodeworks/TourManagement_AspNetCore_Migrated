using Microsoft.EntityFrameworkCore;
using Tour_Management.Models;

namespace Tour_Management.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Tour> Tours { get; set; }
    }
}
