using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeskReservation.Migrations
{
    /// <inheritdoc />
    public partial class Desk : Migration
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
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastStatusChangeDate",
                table: "Desks",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Desks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Desks_Rooms_RoomId",
                table: "Desks",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Desks_Rooms_RoomId",
                table: "Desks");

            migrationBuilder.DropColumn(
                name: "LastStatusChangeDate",
                table: "Desks");

            migrationBuilder.DropColumn(
                name: "Status",
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
    }
}
