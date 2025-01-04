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

            migrationBuilder.AddColumn<string>(
                name: "CoverArtUrl",
                table: "Chapters",
                type: "text",
                nullable: true);

            migrationBuilder.Sql(
                """
                UPDATE "Chapters" AS "C"
                SET "C"."CoverArtUrl" = "SM"."CoverArtUrl"
                FROM "StoryMetadata" AS "SM"
                WHERE "C"."FirstChapterId" = "SM"."FirstChapterId"
                """
            );

            migrationBuilder.DropTable(
                name: "StoryMetadata");
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

            migrationBuilder.Sql(
                """
                INSERT INTO "StoryMetadata" ("FirstChapterId", "CoverArtUrl")
                SELECT DISTINCT ON ("FirstChapterId") "FirstChapterId", "CoverArtUrl"
                FROM "Chapters"
                """
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_StoryMetadata_FirstChapterId",
                table: "Chapters",
                column: "FirstChapterId",
                principalTable: "StoryMetadata",
                principalColumn: "FirstChapterId");
        }
    }
}
