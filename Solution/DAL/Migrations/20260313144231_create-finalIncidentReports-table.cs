using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class createfinalIncidentReportstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinalIncidentReport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    DocumentationConsent = table.Column<bool>(type: "bit", nullable: false),
                    PublicationConsent = table.Column<bool>(type: "bit", nullable: false),
                    IncidentId = table.Column<int>(type: "int", nullable: false),
                    FieldResearcherId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalIncidentReport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinalIncidentReport_FieldResearcherInfos_FieldResearcherId",
                        column: x => x.FieldResearcherId,
                        principalTable: "FieldResearcherInfos",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinalIncidentReport_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinalIncidentReport_FieldResearcherId",
                table: "FinalIncidentReport",
                column: "FieldResearcherId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalIncidentReport_IncidentId",
                table: "FinalIncidentReport",
                column: "IncidentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinalIncidentReport");
        }
    }
}
