using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RuynServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LevelData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LevelPackName = table.Column<string>(type: "TINYTEXT", maxLength: 50, nullable: false),
                    Author = table.Column<string>(type: "TINYTEXT", maxLength: 50, nullable: false),
                    LevelCount = table.Column<int>(type: "TINYINT", nullable: false),
                    DownloadCount = table.Column<int>(type: "INT", nullable: false),
                    FileData = table.Column<byte[]>(type: "BLOB", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelData", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LevelData");
        }
    }
}
