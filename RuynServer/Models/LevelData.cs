using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RuynServer.Models
{
    public class LevelData
    {
        [Key]
        public int Id { get; set; }

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
        [Column(TypeName = "TEXT")]
        public string? FileDataHash { get; set; } = string.Empty;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
        
    }
}
