using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjFinalCinelAir.CommonCore.Migrations
{
    public partial class AddedCharityToPartnerAndNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isCharity",
                table: "Partner",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "Subject",
                table: "Notification",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isRepliedByEmployee",
                table: "Notification",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "isValidated",
                table: "AwardTicket",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isCharity",
                table: "Partner");

            migrationBuilder.DropColumn(
                name: "isRepliedByEmployee",
                table: "Notification");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "Notification",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<bool>(
                name: "isValidated",
                table: "AwardTicket",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool));
        }
    }
}
