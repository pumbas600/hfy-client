using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HfyClientApi.Migrations
{
    /// <inheritdoc />
    public partial class StoryMetadataForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_StoryMetadata_FirstChapterId",
                table: "Chapters",
                column: "FirstChapterId",
                principalTable: "StoryMetadata",
                principalColumn: "FirstChapterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_StoryMetadata_FirstChapterId",
                table: "Chapters");
        }
    }
}
