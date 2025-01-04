using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HfyClientApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStoryMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_StoryMetadata_FirstChapterId",
                table: "Chapters");

            migrationBuilder.DropTable(
                name: "StoryMetadata");

            migrationBuilder.AddColumn<string>(
                name: "CoverArtUrl",
                table: "Chapters",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverArtUrl",
                table: "Chapters");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_StoryMetadata_FirstChapterId",
                table: "Chapters",
                column: "FirstChapterId",
                principalTable: "StoryMetadata",
                principalColumn: "FirstChapterId");
        }
    }
}
