using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SouthSideK9Camp.Server.Migrations
{
    /// <inheritdoc />
    public partial class ProductionTesting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PuposeOfJoining",
                table: "Members",
                newName: "PurposeOfJoining");

            migrationBuilder.AlterColumn<string>(
                name: "Contact",
                table: "Clients",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PurposeOfJoining",
                table: "Members",
                newName: "PuposeOfJoining");

            migrationBuilder.AlterColumn<string>(
                name: "Contact",
                table: "Clients",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldMaxLength: 11);
        }
    }
}
