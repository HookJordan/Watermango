using Microsoft.EntityFrameworkCore;

namespace Watermango.Models 
{
    public class PlantContext : DbContext 
    {
        public PlantContext(DbContextOptions<PlantContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Unique plant names
            modelBuilder.Entity<Plant>()
                .HasIndex(p => p.Name)
                .IsUnique();
        }

        public DbSet<Plant> Plants { get; set; }
    }
}