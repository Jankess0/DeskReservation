using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeskReservation.Migrations
{
    /// <inheritdoc />
    public partial class Desk3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdminOnly",
                table: "Desks",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdminOnly",
                table: "Desks");
        }
    }
}
