using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlHudhud.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToScopeOfWork : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ScopesOfWork",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ScopesOfWork");
        }
    }
}
