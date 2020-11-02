using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjFinalCinelAir.CommonCore.Migrations
{
    public partial class added_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CitizenCardNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TaxIdentificationNumber",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Client_Number",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Identification",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "JoinDate",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TaxNumber",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Mile_Bonus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Miles_Number = table.Column<decimal>(nullable: false),
                    Validity = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mile_Bonus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mile_Bonus_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mile_Status",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Miles_Number = table.Column<decimal>(nullable: false),
                    Validity = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mile_Status", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mile_Status_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rate",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true),
                    Percentage = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Travel_Ticket",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<int>(nullable: false),
                    Travel_Date = table.Column<DateTime>(nullable: false),
                    DepartureCity = table.Column<string>(nullable: true),
                    ArrivalCity = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    RateId = table.Column<int>(nullable: false),
                    Miles_StatusId = table.Column<int>(nullable: false),
                    Miles_BonusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Travel_Ticket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Travel_Ticket_Mile_Bonus_Miles_BonusId",
                        column: x => x.Miles_BonusId,
                        principalTable: "Mile_Bonus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Travel_Ticket_Mile_Status_Miles_StatusId",
                        column: x => x.Miles_StatusId,
                        principalTable: "Mile_Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Travel_Ticket_Rate_RateId",
                        column: x => x.RateId,
                        principalTable: "Rate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Travel_Ticket_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Historic_Status",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Start_Date = table.Column<DateTime>(nullable: false),
                    End_Date = table.Column<DateTime>(nullable: false),
                    isValidated = table.Column<bool>(nullable: false),
                    StatusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historic_Status", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Historic_Status_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Historic_Status_StatusId",
                table: "Historic_Status",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Mile_Bonus_UserId",
                table: "Mile_Bonus",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Mile_Status_UserId",
                table: "Mile_Status",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Travel_Ticket_Miles_BonusId",
                table: "Travel_Ticket",
                column: "Miles_BonusId");

            migrationBuilder.CreateIndex(
                name: "IX_Travel_Ticket_Miles_StatusId",
                table: "Travel_Ticket",
                column: "Miles_StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Travel_Ticket_RateId",
                table: "Travel_Ticket",
                column: "RateId");

            migrationBuilder.CreateIndex(
                name: "IX_Travel_Ticket_UserId",
                table: "Travel_Ticket",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Historic_Status");

            migrationBuilder.DropTable(
                name: "Travel_Ticket");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Mile_Bonus");

            migrationBuilder.DropTable(
                name: "Mile_Status");

            migrationBuilder.DropTable(
                name: "Rate");

            migrationBuilder.DropColumn(
                name: "Client_Number",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Identification",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "JoinDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TaxNumber",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "CitizenCardNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxIdentificationNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
