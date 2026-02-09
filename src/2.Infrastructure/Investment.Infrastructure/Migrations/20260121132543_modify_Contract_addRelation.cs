using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Investment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class modify_Contract_addRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "UserContracts",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "UserContracts",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AcceptedAt",
                table: "UserContracts",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UserContracts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserContracts_ContractMarketTypeId",
                table: "UserContracts",
                column: "ContractMarketTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserContracts_ContractMarketTypes_ContractMarketTypeId",
                table: "UserContracts",
                column: "ContractMarketTypeId",
                principalTable: "ContractMarketTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserContracts_ContractMarketTypes_ContractMarketTypeId",
                table: "UserContracts");

            migrationBuilder.DropIndex(
                name: "IX_UserContracts_ContractMarketTypeId",
                table: "UserContracts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserContracts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "UserContracts",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "UserContracts",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AcceptedAt",
                table: "UserContracts",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }
    }
}
