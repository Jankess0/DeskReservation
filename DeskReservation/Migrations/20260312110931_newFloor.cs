using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeskReservation.Migrations
{
    /// <inheritdoc />
    public partial class newFloor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Floors",
                newName: "FloorNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FloorNumber",
                table: "Floors",
                newName: "Name");
        }
    }
}
