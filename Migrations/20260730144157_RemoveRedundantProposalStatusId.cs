using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlHudhud.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRedundantProposalStatusId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proposals_Proposal_Statuses_ProposalStatusId",
                table: "Proposals");

            migrationBuilder.DropIndex(
                name: "IX_Proposals_ProposalStatusId",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "ProposalStatusId",
                table: "Proposals");

            migrationBuilder.CreateIndex(
                name: "IX_Proposals_StatusId",
                table: "Proposals",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Proposals_Proposal_Statuses_StatusId",
                table: "Proposals",
                column: "StatusId",
                principalTable: "Proposal_Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proposals_Proposal_Statuses_StatusId",
                table: "Proposals");

            migrationBuilder.DropIndex(
                name: "IX_Proposals_StatusId",
                table: "Proposals");

            migrationBuilder.AddColumn<int>(
                name: "ProposalStatusId",
                table: "Proposals",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Proposals_ProposalStatusId",
                table: "Proposals",
                column: "ProposalStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Proposals_Proposal_Statuses_ProposalStatusId",
                table: "Proposals",
                column: "ProposalStatusId",
                principalTable: "Proposal_Statuses",
                principalColumn: "Id");
        }
    }
}
