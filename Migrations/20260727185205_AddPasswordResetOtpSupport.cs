using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlHudhud.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordResetOtpSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordResetOtp",
                table: "Identity_Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordResetOtpExpiryTime",
                table: "Identity_Users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordResetOtp",
                table: "Identity_Users");

            migrationBuilder.DropColumn(
                name: "PasswordResetOtpExpiryTime",
                table: "Identity_Users");
        }
    }
}
