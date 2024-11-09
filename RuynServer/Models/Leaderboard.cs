using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RuynServer.Models
{    public class Leaderboard
    {   
        [MaxLength(50)]
        public string? UserName { get; set; }

        [MaxLength(100)]
        public required string ClientId { get; set; }

        public int Score { get; set; }

        public required string LevelPackName { get; set; }

        [JsonIgnore]
        public LevelData? LevelData { get; set; } = null;

        public int LevelNumber { get; set; }
    }
}

