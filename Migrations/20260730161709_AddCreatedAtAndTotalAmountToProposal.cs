using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlHudhud.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtAndTotalAmountToProposal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Proposals",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Proposals",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Proposals");
        }
    }
}
