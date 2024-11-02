namespace RuynServer.Pocos.Responses
{
    public class LevelListResponse
    {
        public int Id { get; set; }

        public required string LevelPackName { get; set; }

        public required string Author { get; set; }

        public required int LevelCount { get; set; }

        public required int DownloadCount { get; set; }

        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
    }
}
