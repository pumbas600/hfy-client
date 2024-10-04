using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HfyClientApi.Migrations
{
    /// <inheritdoc />
    public partial class WhitelistedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Name",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Name");

            migrationBuilder.CreateTable(
                name: "WhitelistedUsers",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhitelistedUsers", x => x.Name);
                    table.ForeignKey(
                        name: "FK_WhitelistedUsers_Users_Name",
                        column: x => x.Name,
                        principalTable: "Users",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WhitelistedUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Name",
                table: "Users",
                column: "Name",
                unique: true);
        }
    }
}
