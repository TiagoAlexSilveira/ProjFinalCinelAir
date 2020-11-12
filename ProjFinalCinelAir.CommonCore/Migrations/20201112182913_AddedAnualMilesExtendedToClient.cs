using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjFinalCinelAir.CommonCore.Migrations
{
    public partial class AddedAnualMilesExtendedToClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnnualMilesExtended",
                table: "Client",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnualMilesExtended",
                table: "Client");
        }
    }
}
