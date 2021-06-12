using Microsoft.EntityFrameworkCore;

namespace FootballersCatalogue.Models
{
    public class CatalogueContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }

        public CatalogueContext(DbContextOptions<CatalogueContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
