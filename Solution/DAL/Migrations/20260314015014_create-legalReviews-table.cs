using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class createlegalReviewstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinalIncidentReport_FieldResearcherInfos_FieldResearcherId",
                table: "FinalIncidentReport");

            migrationBuilder.DropForeignKey(
                name: "FK_FinalIncidentReport_Incidents_IncidentId",
                table: "FinalIncidentReport");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FinalIncidentReport",
                table: "FinalIncidentReport");

            migrationBuilder.RenameTable(
                name: "FinalIncidentReport",
                newName: "FinalIncidentReports");

            migrationBuilder.RenameIndex(
                name: "IX_FinalIncidentReport_IncidentId",
                table: "FinalIncidentReports",
                newName: "IX_FinalIncidentReports_IncidentId");

            migrationBuilder.RenameIndex(
                name: "IX_FinalIncidentReport_FieldResearcherId",
                table: "FinalIncidentReports",
                newName: "IX_FinalIncidentReports_FieldResearcherId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FinalIncidentReports",
                table: "FinalIncidentReports",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "LegalReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FinalIncidentReportId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegalReviews_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LegalReviews_FinalIncidentReports_FinalIncidentReportId",
                        column: x => x.FinalIncidentReportId,
                        principalTable: "FinalIncidentReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LegalReviews_AppUserId",
                table: "LegalReviews",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalReviews_FinalIncidentReportId",
                table: "LegalReviews",
                column: "FinalIncidentReportId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FinalIncidentReports_FieldResearcherInfos_FieldResearcherId",
                table: "FinalIncidentReports",
                column: "FieldResearcherId",
                principalTable: "FieldResearcherInfos",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FinalIncidentReports_Incidents_IncidentId",
                table: "FinalIncidentReports",
                column: "IncidentId",
                principalTable: "Incidents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinalIncidentReports_FieldResearcherInfos_FieldResearcherId",
                table: "FinalIncidentReports");

            migrationBuilder.DropForeignKey(
                name: "FK_FinalIncidentReports_Incidents_IncidentId",
                table: "FinalIncidentReports");

            migrationBuilder.DropTable(
                name: "LegalReviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FinalIncidentReports",
                table: "FinalIncidentReports");

            migrationBuilder.RenameTable(
                name: "FinalIncidentReports",
                newName: "FinalIncidentReport");

            migrationBuilder.RenameIndex(
                name: "IX_FinalIncidentReports_IncidentId",
                table: "FinalIncidentReport",
                newName: "IX_FinalIncidentReport_IncidentId");

            migrationBuilder.RenameIndex(
                name: "IX_FinalIncidentReports_FieldResearcherId",
                table: "FinalIncidentReport",
                newName: "IX_FinalIncidentReport_FieldResearcherId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FinalIncidentReport",
                table: "FinalIncidentReport",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FinalIncidentReport_FieldResearcherInfos_FieldResearcherId",
                table: "FinalIncidentReport",
                column: "FieldResearcherId",
                principalTable: "FieldResearcherInfos",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FinalIncidentReport_Incidents_IncidentId",
                table: "FinalIncidentReport",
                column: "IncidentId",
                principalTable: "Incidents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
