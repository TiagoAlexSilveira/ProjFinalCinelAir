using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjFinalCinelAir.CommonCore.Migrations
{
    public partial class AddedCardToClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "validity",
                table: "Card");

            migrationBuilder.AddColumn<int>(
                name: "CardId",
                table: "Client",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "Card",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Client_CardId",
                table: "Client",
                column: "CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Client_Card_CardId",
                table: "Client",
                column: "CardId",
                principalTable: "Card",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_Card_CardId",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_Client_CardId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "Card");

            migrationBuilder.AddColumn<DateTime>(
                name: "validity",
                table: "Card",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
