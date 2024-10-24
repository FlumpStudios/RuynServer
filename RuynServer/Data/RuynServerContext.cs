using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using RuynServer.Models;

namespace RuynServer.Data
{
    public class RuynServerContext : DbContext
    {
        public RuynServerContext (DbContextOptions<RuynServerContext> options)
            : base(options)
        {
        }

        public DbSet<RuynServer.Models.LevelData> LevelData { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LevelData>()
                .HasIndex(e => e.LevelPackName)
                .IsUnique();
            base.OnModelCreating(modelBuilder);
        }

    }
}
