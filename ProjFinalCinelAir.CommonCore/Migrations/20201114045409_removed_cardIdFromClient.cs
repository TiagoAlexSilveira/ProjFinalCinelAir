using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjFinalCinelAir.CommonCore.Migrations
{
    public partial class removed_cardIdFromClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CardId",
                table: "Client",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
    }
}
