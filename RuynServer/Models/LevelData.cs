using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using RuynServer.Enumerations;

namespace RuynServer.Models
{

    public class VoteType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("Id")]

        public VotingType VoteId { get; set; }

        public string Name { get; set; } = string.Empty;
    }

    public class VoteJuntion
    {
        public string? ClientId { get; set; }
        public int LevelDataId { get; set; }
        public VotingType VoteID { get; set; }
        public LevelData? LevelData { get; set; }
        public VoteType? Votes { get; set; }
    }

    public class LevelData
    {
        [Key]
        [Column("Id")]
        public int LevelDataId { get; set; }

        public string? ClientId { get; set; }        
        
        [Required]
        [Column(TypeName = "TINYTEXT")]
        [MaxLength(50)]
        public required string LevelPackName { get; set; }

        [Required]
        [Column(TypeName = "TINYTEXT")]
        [MaxLength(50)]
        public required string Author { get; set; }

        [Required]
        [Column(TypeName = "TINYINT")]
        public required int LevelCount { get; set; }

        [Column(TypeName = "INT")]
        public required int DownloadCount { get; set; }

        [Required]
        [Column(TypeName = "BLOB")]
        public byte[] FileData { get; set; } = [];

        [JsonIgnore]
        [Column(TypeName = "INT")]
        public int Rank { get; set; }

        [JsonIgnore]
        [Column(TypeName = "TEXT")]
        public string? FileDataHash { get; set; } = string.Empty;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public ICollection<VoteJuntion> VoteJunctions { get; set; } = [];
    }
}
