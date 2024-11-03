using Microsoft.EntityFrameworkCore;
using RuynServer.Models;

namespace RuynServer.Data
{
    public class RuynServerContext(DbContextOptions<RuynServerContext> options) : DbContext(options)
    {
        public DbSet<RuynServer.Models.LevelData> LevelData { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LevelData>()
                .HasIndex(e => e.LevelPackName)
                .IsUnique();

            modelBuilder.Entity<LevelData>()
                .HasIndex(e => e.FileDataHash)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }

    }
}
