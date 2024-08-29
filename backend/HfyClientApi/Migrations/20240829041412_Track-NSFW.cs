using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HfyClientApi.Migrations
{
    /// <inheritdoc />
    public partial class TrackNSFW : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNsfw",
                table: "Chapters",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNsfw",
                table: "Chapters");
        }
    }
}
