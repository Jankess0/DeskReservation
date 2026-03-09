using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeskReservation.Migrations
{
    /// <inheritdoc />
    public partial class newSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Desks_Rooms_RoomId",
                table: "Desks");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Desks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Desks_Rooms_RoomId",
                table: "Desks",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Desks_Rooms_RoomId",
                table: "Desks");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Desks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Desks_Rooms_RoomId",
                table: "Desks",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }
    }
}
