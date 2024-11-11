using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SouthSideK9Camp.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddidDeleteBool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Reservations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Reasons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProgressReports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "MembershipDues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Members",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Logs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Invoices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Dogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Contracts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Clients",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Reasons");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProgressReports");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MembershipDues");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Dogs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Clients");
        }
    }
}
