using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Investment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class modifiy_Contract_addRateToUserContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SumRate",
                table: "UserContracts",
                type: "numeric(28,2)",
                precision: 28,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SumRate",
                table: "UserContracts");
        }
    }
}
