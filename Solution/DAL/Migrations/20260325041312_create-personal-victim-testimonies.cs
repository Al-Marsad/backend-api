using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class createpersonalvictimtestimonies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "News",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "InitialDescription",
                table: "InitialIncidentReports",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.CreateTable(
                name: "PersonalVictimTestimonies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PersonalNarrative = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HardestMoment = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    InternationalDemand = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    IncidentId = table.Column<int>(type: "int", nullable: false),
                    VictimId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalVictimTestimonies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalVictimTestimonies_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonalVictimTestimonies_Victims_VictimId",
                        column: x => x.VictimId,
                        principalTable: "Victims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonalVictimTestimonies_IncidentId",
                table: "PersonalVictimTestimonies",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalVictimTestimonies_VictimId",
                table: "PersonalVictimTestimonies",
                column: "VictimId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonalVictimTestimonies");

            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "News",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "InitialDescription",
                table: "InitialIncidentReports",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);
        }
    }
}
