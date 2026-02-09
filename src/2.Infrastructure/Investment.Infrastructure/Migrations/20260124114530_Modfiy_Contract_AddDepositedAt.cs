using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Investment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Modfiy_Contract_AddDepositedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeposit",
                table: "UserContractProfits");

            migrationBuilder.AddColumn<DateTime>(
                name: "DepositedAt",
                table: "UserContractProfits",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepositedAt",
                table: "UserContractProfits");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeposit",
                table: "UserContractProfits",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
