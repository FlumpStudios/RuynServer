using Microsoft.EntityFrameworkCore;
using RuynServer.Models;

namespace RuynServer.Data
{
    public class RuynServerContext(DbContextOptions<RuynServerContext> options) : DbContext(options)
    {
        public DbSet<RuynServer.Models.LevelData> LevelData { get; set; } = default!;
        public DbSet<RuynServer.Models.VoteJuntion> VoteJuntion { get; set; } = default!;
        public DbSet<RuynServer.Models.VoteType> Votes { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VoteType>()
              .HasData(
                  new VoteType
                  {
                      VoteId = Enumerations.VotingType.None,
                      Name = Enumerations.VotingType.None.ToString()
                  },
                  new VoteType
                  {
                      VoteId = Enumerations.VotingType.Upvote,
                      Name = Enumerations.VotingType.Upvote.ToString()
                  },
                   new VoteType
                   {
                       VoteId = Enumerations.VotingType.Downvote,
                       Name = Enumerations.VotingType.Downvote.ToString()
                   }
              );

            modelBuilder.Entity<LevelData>()
                .HasIndex(e => e.LevelPackName)
                .IsUnique();

            modelBuilder.Entity<LevelData>()
                .HasIndex(e => e.FileDataHash)
                .IsUnique();


            modelBuilder.Entity<VoteJuntion>()
              .HasKey(v => new { v.ClientId, v.LevelDataId });


            modelBuilder.Entity<VoteJuntion>()
              .HasIndex(v => new { v.ClientId, v.LevelDataId })
              .IsUnique();
            
            modelBuilder.Entity<VoteJuntion>()
                .HasOne(v => v.LevelData)
                .WithMany(l => l.VoteJunctions)
                .HasForeignKey(v => v.LevelDataId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
