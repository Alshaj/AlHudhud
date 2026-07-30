using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlHudhud.Migrations
{
    /// <inheritdoc />
    public partial class AddReceivedAndPendingToProposal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PendingAmount",
                table: "Proposals",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ReceivedFromClient",
                table: "Proposals",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PendingAmount",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "ReceivedFromClient",
                table: "Proposals");
        }
    }
}
