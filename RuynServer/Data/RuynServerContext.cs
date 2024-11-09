using Microsoft.EntityFrameworkCore;
using RuynServer.Models;

namespace RuynServer.Data
{
    public class RuynServerContext(DbContextOptions<RuynServerContext> options) : DbContext(options)
    {
        public DbSet<RuynServer.Models.LevelData> LevelData { get; set; } = default!;
        public DbSet<RuynServer.Models.VoteJuntion> VoteJuntion { get; set; } = default!;
        public DbSet<RuynServer.Models.VoteType> Votes { get; set; } = default!;
        public DbSet<RuynServer.Models.Leaderboard> Leaderboard { get; set; } = default!;

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
                .HasIndex(e => e.FileDataHash)
                .IsUnique();

            modelBuilder.Entity<Leaderboard>().HasIndex(e => e.Score);

            modelBuilder.Entity<Leaderboard>().HasKey(x => new { x.ClientId, x.LevelPackName, x.LevelNumber });

            modelBuilder.Entity<VoteJuntion>()
              .HasKey(v => new { v.ClientId, v.LevelPackName });
            
            modelBuilder.Entity<VoteJuntion>()
                .HasOne(v => v.LevelData)
                .WithMany(l => l.VoteJunctions)
                .HasForeignKey(v => v.LevelPackName);


            base.OnModelCreating(modelBuilder);
        }

    }
}
