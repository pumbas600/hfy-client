using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HfyClientApi.Migrations
{
    /// <inheritdoc />
    public partial class StoryMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProcessedAtUtc",
                table: "Chapters",
                newName: "SyncedAtUtc");

            migrationBuilder.CreateTable(
                name: "StoryMetadata",
                columns: table => new
                {
                    FirstChapterId = table.Column<string>(type: "text", nullable: false),
                    CoverArtUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryMetadata", x => x.FirstChapterId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_Subreddit",
                table: "Chapters",
                column: "Subreddit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoryMetadata");

            migrationBuilder.DropIndex(
                name: "IX_Chapters_Subreddit",
                table: "Chapters");

            migrationBuilder.RenameColumn(
                name: "SyncedAtUtc",
                table: "Chapters",
                newName: "ProcessedAtUtc");
        }
    }
}
