using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Investment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class modifiy_User_ChangePropertyName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "ValidationTokens");

            migrationBuilder.AddColumn<string>(
                name: "HashedToken",
                table: "ValidationTokens",
                type: "character varying(130)",
                unicode: false,
                maxLength: 130,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HashedToken",
                table: "ValidationTokens");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "ValidationTokens",
                type: "character varying(10)",
                unicode: false,
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }
    }
}
