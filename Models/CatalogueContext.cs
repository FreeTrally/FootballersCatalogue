using Microsoft.EntityFrameworkCore;

namespace FootballersCatalogue.Models
{
    public class CatalogueContext : DbContext
    {
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Order> Orders { get; set; }

        public CatalogueContext(DbContextOptions<CatalogueContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
