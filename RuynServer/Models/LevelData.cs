using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RuynServer.Models
{
    public class LevelData
    {
        [Key] // Indicates that this property is the primary key
        public int Id { get; set; }

        [Required] // Ensures that this field cannot be null
        [Column(TypeName = "TEXT")] // Specifies the column type
        public required string FileName { get; set; }

        [Required] // Ensures that this field cannot be null
        [Column(TypeName = "BLOB")] // Specifies the column type for binary data
        public byte[] FileData { get; set; } = [];

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Automatically generates a value for this field
        public DateTime UploadDate { get; set; } = DateTime.UtcNow; // Default value set to current UTC time

    }
}
