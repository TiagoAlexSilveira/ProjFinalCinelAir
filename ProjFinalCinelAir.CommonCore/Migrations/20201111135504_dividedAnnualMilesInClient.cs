using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjFinalCinelAir.CommonCore.Migrations
{
    public partial class dividedAnnualMilesInClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnualMilesShopped",
                table: "Client");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "Notification",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AnnualMilesBought",
                table: "Client",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AnnualMilesConverted",
                table: "Client",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AnnualMilesTransfered",
                table: "Client",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnualMilesBought",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "AnnualMilesConverted",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "AnnualMilesTransfered",
                table: "Client");

            migrationBuilder.AlterColumn<int>(
                name: "Subject",
                table: "Notification",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnnualMilesShopped",
                table: "Client",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
